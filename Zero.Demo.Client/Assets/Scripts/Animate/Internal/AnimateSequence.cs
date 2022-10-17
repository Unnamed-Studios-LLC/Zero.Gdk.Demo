namespace AnimateInternal
{
    public class AnimateSequence : AnimateTaskCollection
    {
        private int _currentTaskIndex = 0;

        public void Setup()
        {
            _currentTaskIndex = 0;

            SetupCollection();
        }

        protected override bool DoUpdate()
        {
            var task = GetTask(_currentTaskIndex);
            if (!task.Update())
            {
                return false;
            }

            return ++_currentTaskIndex >= TaskCount;
        }
    }
}
