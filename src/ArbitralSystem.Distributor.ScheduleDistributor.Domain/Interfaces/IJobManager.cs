using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Interfaces
{
    public interface IJobManager
    {
        string Enqueue(string queueName, Expression<Func<Task>> expression);
        void Delete(string id);
    }
}