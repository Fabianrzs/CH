using GA2.Transversals.Commons.BackgroundTask;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections.Generic;
using System.Linq;

namespace GA2.Apis.GA2.BackgroundServices
{
    class GeneracionReporteBackgroundService : BackgroundService
    {
        private readonly ILogger<GeneracionReporteBackgroundService> logger;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IBackgroundReportQueue taskQueue;

        public GeneracionReporteBackgroundService(IServiceScopeFactory scopeFactory, IBackgroundReportQueue taskQueue,
            ILogger<GeneracionReporteBackgroundService> logger)
        {
            this.scopeFactory = scopeFactory;
            this.taskQueue = taskQueue;
            this.logger = logger;
        }
        public override async Task StartAsync(CancellationToken stoppingToken)
        {
            await base.StartAsync(stoppingToken);
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var task = new List<Task>();

            foreach (var _ in Enumerable.Range(1, 3))
                task.Add(ProcessTaskQueueAsync(stoppingToken));

            await Task.WhenAll(task);
        }

        private async Task ProcessTaskQueueAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                try
                {
                    var workItem = await taskQueue.DequeueAsync(stoppingToken);
                    await using var scope = scopeFactory.CreateAsyncScope();
                    await workItem(scope.ServiceProvider, stoppingToken);
                }
                catch (OperationCanceledException)
                {

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error occurred executing task work item.");
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }
    }
}
