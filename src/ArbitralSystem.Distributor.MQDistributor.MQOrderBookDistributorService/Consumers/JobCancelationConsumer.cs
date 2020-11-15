using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.Distributor.MQDistributor.MQOrderBookDistributorService.Jobs;
using ArbitralSystem.Messaging.Messages;
using JetBrains.Annotations;
using MassTransit;

namespace ArbitralSystem.Distributor.MQDistributor.MQOrderBookDistributorService.Consumers
{
    [UsedImplicitly]
    public class JobCancellationConsumer : IConsumer<IStopOrderBookDistribution>
    {
        private readonly JobManager _jobManager;
        private readonly ILogger _logger;
        
        public JobCancellationConsumer(JobManager jobManager, ILogger logger)
        {
            Preconditions.CheckNotNull(jobManager, logger);
            _jobManager = jobManager;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<IStopOrderBookDistribution> context)
        {
            var botName = context.Message.DistributorId;
            if (_jobManager.IsExist(botName))
            {
                _logger.Information($"Job cancellation handled, Job with name: {botName} exist, cancellation started.");
                await _jobManager.CancelJob(context.Message.DistributorId);
            }
            else
            {
                _logger.Information($"Job cancellation handled, Job with name: {botName} not exist");
            }
        }
    }
}