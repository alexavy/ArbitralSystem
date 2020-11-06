using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Interfaces;
using Hangfire;
using Hangfire.Server;
using Hangfire.States;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Persistence.Repositories
{
    public class JobManagerRepository : IJobManager
    {
        public string Enqueue(string queueName, Expression<Func<Task>> expression)
        {
            return BackgroundJob.Enqueue(expression);
            //var client = new BackgroundJobClient(JobStorage.Current);
            //var state = new EnqueuedState(queueName);
            //return client.Create(expression, state);
        }
        
        public void Delete(string id)
        {
            BackgroundJob.Delete(id);
        }
    }
}