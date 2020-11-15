using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Connectors.Core.Common;
using ArbitralSystem.Connectors.Core.Exceptions;
using ArbitralSystem.Connectors.Core.Helpers;
using ArbitralSystem.Domain.MarketInfo;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ArbitralSystem.Connectors.Core
{
    public abstract class BaseRestClient
    {
        protected RestClient Client { get; set; }
        protected IConnectionInfo ConnectionInfo { get;  set; }
        protected  RestClient BuildRestClient(string baseUrl)
        {
            var client = new RestClient(baseUrl);
            client.AddDefaultHeader("Accept", "application/json");
            return client;
        }
        protected virtual IResponse<TResponse> DeserializeResponse<TResponse>(
            [NotNull] IResponse<IRestResponse> response)
        {
            var validationResult = ValidateResponse<TResponse>(response);

            if (!validationResult.IsSuccess)
                return validationResult;

            var restResponse = response.Data;

            try
            {
                var data = JsonConvert.DeserializeObject<TResponse>(restResponse.Content);
                return new Response<TResponse>(data);
            }
            catch (Exception ex)
            {
                var exception =
                    new RestClientException($"Error while parsing response , Content : {restResponse.Content}", ex);
                return new Response<TResponse>(exception);
            }
        }
        
        protected virtual IResponse<TOut> DeserializeResponse<TIn, TOut>(
            [NotNull] IResponse<IRestResponse> response) where TIn: TOut
        {
            var validationResult = ValidateResponse<TOut>(response);

            if (!validationResult.IsSuccess)
                return validationResult;

            var restResponse = response.Data;

            try
            {
                var data = (TOut)JsonConvert.DeserializeObject<TIn>(restResponse.Content);
                return new Response<TOut>(data);
            }
            catch (Exception ex)
            {
                var exception =
                    new RestClientException($"Error while parsing response , Content : {restResponse.Content}", ex);
                return new Response<TOut>(exception);
            }
        }
        

        protected virtual IResponse<TResponse> DeserializeResponse<TResponse>(
            [NotNull] IResponse<IRestResponse> response, [NotNull] string jObject)
        {
            var validationResult = ValidateResponse<TResponse>(response);

            if (!validationResult.IsSuccess)
                return validationResult;

            var restResponse = response.Data;

            var jMessage = JObject.Parse(restResponse.Content);

            if (!jMessage.ContainsKey(jObject))
            {
                var message = $"Result key {jObject} not found and error key missed";
                var exception = new RestClientException($"{message}");
                return new Response<TResponse>(exception);
            }

            try
            {
                var objectMessage = jMessage[jObject].ToObject<TResponse>();
                return new Response<TResponse>(objectMessage);
            }
            catch (Exception ex)
            {
                var message = $"Cannot deserialize response {restResponse.Content}";
                var exception = new RestClientException($"{message}", ex);
                return new Response<TResponse>(exception);
            }
        }

        private IResponse<TResponse> ValidateResponse<TResponse>([NotNull] IResponse<IRestResponse> response)
        {
            if (!response.IsSuccess)
            {
                return new Response<TResponse>(response.Exception);
            }

            var restResponse = response.Data;
            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                if (!restResponse.Content.IsValidJson())
                {
                    var exception =
                        new RestClientException($"Response content is not valid. Uri: {restResponse.ResponseUri}");
                    return new Response<TResponse>(exception);
                }

                return new Response<TResponse>();
            }

            {
                var logMessage = $"response status: {restResponse.Content}.";
                var exception = new UnexpectedRestClientException(logMessage, restResponse.ErrorException);
                return new Response<TResponse>(exception);
            }
        }

        protected async Task<IResponse<IRestResponse>> ExecuteRequestWithTimeOut(IRestRequest restRequest,
            int milliseconds)
        {
            try
            {
                var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(milliseconds));
                var response = await Client.ExecuteTaskAsync(restRequest, cts.Token);
                return new Response<IRestResponse>(response);
            }
            catch (TaskCanceledException ex)
            {
                var exception = new RestClientException($"Timout in {restRequest.Resource}", ex);
                return new Response<IRestResponse>(exception);
            }
        }
    }

    public abstract class BaseApiRestClient : BaseRestClient
    {
        protected BaseApiRestClient([NotNull] IConnectionInfo connectionInfo)
        {
            ConnectionInfo = connectionInfo;
            Client = BuildRestClient(connectionInfo.BaseRestUrl);
        }
    }
    
    public abstract class BaseExchangeRestClient : BaseRestClient
    {
        protected BaseExchangeRestClient([NotNull] IExchangeConnectionInfo[] exchangeConnectionInfo) 
        {
            var tempConnectionInfo = exchangeConnectionInfo.SingleOrDefault(o => o.Exchange == Exchange);

            if (tempConnectionInfo == null || tempConnectionInfo.Exchange == Exchange.Undefined)
                throw new RestClientException($"Connection info was not found or undefined");

            ConnectionInfo = tempConnectionInfo;
            Client = BuildRestClient(tempConnectionInfo.BaseRestUrl);
        }
        protected abstract Exchange Exchange { get; }
    }
}