using System.Threading;

namespace TheProjectGame.Messaging
{
    public interface ITaskCanceller
    {
        void CancelTasks();
    }

    public interface ITaskCancellationTokenProvider
    {
        CancellationToken Token { get; }
    }

    class TaskCancellation : ITaskCanceller, ITaskCancellationTokenProvider
    {
        private CancellationTokenSource cancellationTokenSource;

        public CancellationToken Token { get; private set; }

        public TaskCancellation()
        {
            RegenerateToken();
        }

        private void RegenerateToken()
        {
            cancellationTokenSource = new CancellationTokenSource();
            Token = cancellationTokenSource.Token;
        }

        public void CancelTasks()
        {
            cancellationTokenSource.Cancel();

            RegenerateToken();
        }
    }
}
