namespace Batchworker
{
    using Business.Services;
    using Shared;
    using System;
    using System.Collections.Concurrent;
    using System.ServiceProcess;
    using System.Threading;
    using System.Threading.Tasks;

    public partial class BatchworkerService : ServiceBase
    {
        // Concurrent queue allows to add new items dynamically and thread safe
        private ConcurrentQueue<Action<DispatchService>> queue = new ConcurrentQueue<Action<DispatchService>>();
        private DispatchService dispatchService = new DispatchService();
        private const int dispatchIntervall = 1000; // ms

        public BatchworkerService()
        {
            InitializeComponent();
        }

        Action<DispatchService>[] actions = new Action<DispatchService>[]
        {
            (d) => d.CancelCancelling(),
            (d) => d.DeleteDeleting(),
            (d) => d.Dispatch(App.Config.SealingSlabChunkSize),
            (d) => Thread.Sleep(dispatchIntervall)
        };

        protected override void OnStart(string[] args)
        {
            // Start asynchronously, otherwise the windows service won't be started correctly.
            new Task(() =>
            {
                try
                {
                    App.Logger.Info("Batchworker started.");

                    // Initial Queue
                    queue.Enqueue((d) => d.ReEnqueueRunning());

                    Action<DispatchService> currentAction = null;

                    while (queue.TryDequeue(out currentAction))
                    {
                        try
                        {
                            currentAction(dispatchService);
                        }
                        catch (Exception ex)
                        {
                            App.Logger.Error(ex);
                        }

                        // Add the action loop again
                        if (queue.IsEmpty)
                        {
                            foreach(var action in actions)
                            {
                                queue.Enqueue(action);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    App.Logger.Error(ex);
                }
            }).Start();
        }

        protected override void OnStop()
        {
            App.Logger.Info("Batchworker stopped.");
        }
    }
}