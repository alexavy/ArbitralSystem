using ArbitralSystem.Connectors.Core.Exceptions;
using CryptoExchange.Net.Objects;

namespace ArbitralSystem.Connectors.CryptoExchange.PublicConnectors
{
    internal abstract class BasePublicConnector
    {
        public void ValidateResponse<T>(WebCallResult<T> result)
        {
            if (!result.Success)
                throw new RestClientException(result.Error?.Message);
        }
        
        public void ValidateResponse<T>(CallResult<T> result)
        {
            if (!result.Success)
                throw new RestClientException(result.Error?.Message);
        }
    }
}