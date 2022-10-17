using System.Collections.Generic;

namespace AnimateInternal
{
    public abstract class AnimateTaskCollection : AnimateTask
    {
        private readonly List<AnimateTask> _tasks = new();

        public int TaskCount => _tasks.Count;

        public virtual AnimateTaskCollection Add(AnimateTask task)
        {
            _tasks.Add(task);
            return this;
        }

        protected AnimateTask GetTask(int index)
        {
            return _tasks[index];
        }

        protected void SetupCollection()
        {
            _tasks.Clear();

            SetupTask();
        }
    }
}
