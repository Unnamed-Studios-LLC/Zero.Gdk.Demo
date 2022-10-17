using System.Collections.Generic;

namespace AnimateInternal
{
    public class AnimateGroup : AnimateTaskCollection
    {
        private readonly List<bool> _done = new();

        public override AnimateTaskCollection Add(AnimateTask task)
        {
            _done.Add(false);
            return base.Add(task);
        }

        public void Setup()
        {
            _done.Clear();

            SetupCollection();
        }

        protected override bool DoUpdate()
        {
            var done = true;
            for (int i = 0; i < TaskCount; i++)
            {
                if (_done[i])
                {
                    continue;
                }

                var task = GetTask(i);
                if (task.Update())
                {
                    _done[i] = true;
                }
                else
                {
                    done = false;
                }
            }

            return done;
        }
    }
}
