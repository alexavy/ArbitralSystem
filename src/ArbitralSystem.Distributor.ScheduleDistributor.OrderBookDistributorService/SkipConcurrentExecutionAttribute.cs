using System;
using ArbitralSystem.Common.Logger;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Logging;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;

namespace ArbitralSystem.Distributor.ScheduleDistributor.OrderBookDistributorService
{
    public class LogEverythingAttribute : JobFilterAttribute, IClientFilter, IServerFilter, IElectStateFilter, IApplyStateFilter
    {
        private readonly ILogger _logger;

        public LogEverythingAttribute(ILogger logger)
        {
            _logger = logger;
        }


        public void OnCreating(CreatingContext context)
        {
            _logger.Information("MON:(OnCreating) Canceled: {can}"
                ,context.Canceled);
            //Logger.InfoFormat("Creating a job based on method `{0}`...", context.Job.Method.Name);
        }

        public void OnCreated(CreatedContext context)
        {
            
            _logger.Information("MON:(OnCreated) Canceled: {ccan}, Exception: {@ex} is canceled {@can}"
                ,context.Canceled, context.Exception ,  context.BackgroundJob.Job.Args[1]);
            /*Logger.InfoFormat(
                "Job that is based on method `{0}` has been created with id `{1}`",
                context.Job.Method.Name,
                context.BackgroundJob?.Id);*/
        }

        public void OnPerforming(PerformingContext context)
        {
            _logger.Information("MON:(OnPerforming) Canceled: {old}, is canceled {@can}"
                ,context.Canceled,  context.BackgroundJob.Job.Args[1]);
            ;
            //Logger.InfoFormat("Starting to perform job `{0}`", context.BackgroundJob.Id);
        }

        public void OnPerformed(PerformedContext context)
        {
            _logger.Information("MON:(OnPerformed) Canceled: {old}, Exception: {@ex}, Rez: {@rez} is canceled {@can}"
                ,context.Canceled,context.Exception, context.Result , context.BackgroundJob.Job.Args[1]);
            //Logger.InfoFormat("Job `{0}` has been performed", context.BackgroundJob.Id);
        }

        public void OnStateElection(ElectStateContext context)
        {
            
            _logger.Information("MON:(OnStateElection) CurrentState: {@old}, CandidateState: {@new}, is canceled {@can}"
                ,context.CurrentState, context.CandidateState, context.BackgroundJob.Job.Args[1]);
            
           /* if (context.CandidateState is ProcessingState && context.CurrentState == "Processing")
            {
                _logger.Information("MON:(OnStateElection) Set as deleted");
                context.CandidateState = new DeletedState();
            }*/

        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            _logger.Information("MON:(OnStateApplied) Old state: {@old}, New state: {@new}, is canceled {@can}"
                ,context.OldStateName, context.NewState, context.BackgroundJob.Job.Args[1]);

        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            _logger.Information("MON:(OnStateUnApplied) Old state: {@old}, New state: {@new}, is canceled {@can}"
                ,context.OldStateName, context.NewState, context.BackgroundJob.Job.Args[1]);

        }
    }


    public class SkipConcurrentExecutionAttribute : JobFilterAttribute, IServerFilter, IElectStateFilter
    {
        private readonly ILogger _logger;

        private readonly int _timeoutInSeconds;

        public SkipConcurrentExecutionAttribute(int timeoutInSeconds, ILogger logger)
        {
            if (timeoutInSeconds < 0) throw new ArgumentException("Timeout argument value should be greater that zero.");
            _logger = logger;
            _timeoutInSeconds = timeoutInSeconds;
        }


        public void OnPerforming(PerformingContext filterContext)
        {
            _logger.Information("SkipConcurrent, On Performing started: {@filterContext}");
            var resource = String.Format(
                "{0}.{1}",
                filterContext.Job.Type.FullName,
                filterContext.Job.Method.Name);


            var timeout = TimeSpan.FromSeconds(_timeoutInSeconds);

            try
            {
                var distributedLock = filterContext.Connection.AcquireDistributedLock(resource, timeout);
                filterContext.Items["DistributedLock"] = distributedLock;
            }
            catch (Exception)
            {
                filterContext.Canceled = true;
                _logger.Fatal("Cancelling run for {0} job, id: {1} ", resource, filterContext.JobId);
            }
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            _logger.Information("On Performed started: {@filterContext}");
            if (!filterContext.Items.ContainsKey("DistributedLock"))
            {
                throw new InvalidOperationException("Can not release a distributed lock: it was not acquired.");
            }

            var distributedLock = (IDisposable) filterContext.Items["DistributedLock"];
            distributedLock.Dispose();
        }

        public void OnStateElection(ElectStateContext context)
        {
        }
    }
}