using System.Threading.Tasks;

namespace AnimateInternal
{
    public abstract class AnimateTask
    {
        private TaskCompletionSource<bool> _completedSource;

        public bool Canceled { get; private set; }

        public void Cancel()
        {
            Canceled = true;
        }

        public AnimateTask Play()
        {
            Animate.Play(this);
            return this;
        }

        public bool Update()
        {
            var finished = Canceled || DoUpdate();
            if (finished)
            {
                _completedSource?.TrySetResult(!Canceled);
            }
            return finished;
        }

        public Task<bool> WaitAsync()
        {
            _completedSource ??= new TaskCompletionSource<bool>();
            return _completedSource.Task;
        }

        protected abstract bool DoUpdate();

        protected void SetupTask()
        {
            _completedSource = null;
            Canceled = false;
        }
    }
}
