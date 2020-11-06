using ArbitralSystem.Connectors.Core.Converters;

namespace ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.Common.Stubs
{
    internal class DtoConverterStub : IDtoConverter
    {
        public TDestination Convert<TSource, TDestination>(TSource source)
        {
            throw new StubNotSupportedException();
        }
    }
}