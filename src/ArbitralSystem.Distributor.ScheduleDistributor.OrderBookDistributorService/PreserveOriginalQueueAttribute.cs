using System;
using System.Linq;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Models;
using ArbitralSystem.Distributor.ScheduleDistributor.OrderBookDistributorService.Options;
using Hangfire.States;
using Hangfire.Storage;

namespace ArbitralSystem.Distributor.ScheduleDistributor.OrderBookDistributorService
{
    internal class PreserveOriginalQueueAttribute : IElectStateFilter
    {
        private readonly ServerOptions _serverOptions;

        public PreserveOriginalQueueAttribute(ServerOptions options)
        {
            _serverOptions = options;
        }

        public void OnStateElection(ElectStateContext context)
        {
            
             if (context.CandidateState is EnqueuedState enqueuedState)
             {
                 if (context.BackgroundJob.Job.Args.First() is DistributorExchangePairs exchangePairInfo)
                 {
                     if (_serverOptions.ServerQueueName == exchangePairInfo.QueueName)
                     {
                         enqueuedState.Queue = exchangePairInfo.QueueName;
                     }
                 }
             }
        }
    }
}