using System.Collections.Generic;
using System.Threading.Tasks;
using ArbitralSystem.Connectors.Core.Models;


namespace ArbitralSystem.Connectors.Core.PublicConnectors
{
    public interface IPublicConnector
    {
        Task<long> GetServerTime();

        Task<IEnumerable<IPairInfo>> GetPairsInfo();
    }
}
