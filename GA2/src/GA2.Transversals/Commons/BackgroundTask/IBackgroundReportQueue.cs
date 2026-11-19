using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Transversals.Commons.BackgroundTask
{
    public interface IBackgroundReportQueue
    {
        ValueTask QueueBackgroundWorkItemAsync(Func<IServiceProvider, CancellationToken, ValueTask> workItem);

        ValueTask<Func<IServiceProvider, CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken);
    }
}
