using System;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Connectors.Core
{

    public class ExchangeConnectionInfo : IExchangeConnectionInfo
    {
        private Exchange _exchange;

        private string _baseRestUrl;

        private string _apiKey;

        private string _apiSecret;

        private int _defaultTimeOutInMs = 3000;

        public ExchangeConnectionInfo()
        {
        }

        public ExchangeConnectionInfo(string apiKey, string apiSecret)
        {
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
                throw new NullReferenceException("Api key or api secret is null or empty");

            _apiKey = apiKey;
            _apiSecret = apiSecret;
        }

        public ExchangeConnectionInfo(string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
                throw new NullReferenceException("BaseUrl is null or empty");

            _baseRestUrl = baseUrl;
        }

        public ExchangeConnectionInfo(string baseUrl, string apiKey, string apiSecret)
        {
            if (string.IsNullOrEmpty(baseUrl) ||
                string.IsNullOrEmpty(apiKey) ||
                string.IsNullOrEmpty(apiSecret))
                throw new NullReferenceException("Api key or api secret or baseUrl is null or empty");

            _baseRestUrl = baseUrl;

            _apiKey = apiKey;
            _apiSecret = apiSecret;
        }

        public Exchange Exchange { get => _exchange; set => _exchange = value; }

        public string BaseRestUrl { get => _baseRestUrl; set => _baseRestUrl = value; }

        public string ApiKey { get => _apiKey; }

        public string ApiSecret { get => _apiSecret; }

        public int DefaultTimeOutInMs { get => _defaultTimeOutInMs; set => _defaultTimeOutInMs = value; }

       
    }
}
