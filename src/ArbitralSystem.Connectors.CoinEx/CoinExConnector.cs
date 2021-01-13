using System.Collections.Generic;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Connectors.CoinEx.Models;
using ArbitralSystem.Connectors.Core;
using ArbitralSystem.Connectors.Core.Common;
using ArbitralSystem.Domain.MarketInfo;
using RestSharp;

namespace ArbitralSystem.Connectors.CoinEx
{
    //https://api.coinex.com/v1
    public class CoinExConnector : BaseExchangeRestClient, ICoinExConnector
    {
        private const string Version = "v1";
        private readonly ILogger _logger;

        public CoinExConnector(IExchangeConnectionInfo[] exchangeConnectionInfo, ILogger logger)
            : base(exchangeConnectionInfo)
        {
            _logger = logger;
        }

        protected override Exchange Exchange => Exchange.CoinEx;
        
        public async Task<IResponse<IEnumerable<string>>> GetMarketList()
        {
            var restRequest = new RestRequest($"{Version}/market/list");
            restRequest.Method = Method.GET;

            var response = await ExecuteRequestWithTimeOut(restRequest, ConnectionInfo.DefaultTimeOutInMs);
            return DeserializeResponse<IEnumerable<string>>(response, "data");
        }
        
        public async Task<IResponse<MarketInfo>> GetMarketSingleInfo(string symbol)
        {
            var restRequest = new RestRequest($"{Version}/market/detail");
            restRequest.Method = Method.GET;

            restRequest.AddQueryParameter("market", symbol);

            var response = await ExecuteRequestWithTimeOut(restRequest, ConnectionInfo.DefaultTimeOutInMs);
            return DeserializeResponse<MarketInfo>(response, "data");
        }
    }
}