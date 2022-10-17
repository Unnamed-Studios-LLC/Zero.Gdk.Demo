using System.Collections.Generic;
using UnityEngine;

namespace AnimateInternal
{
    public class AnimateInstance : MonoBehaviour
    {
        private List<AnimateTask> _tasks = new List<AnimateTask>();
        private List<AnimateTask> _tasksNext = new List<AnimateTask>();

        public void AddTask(AnimateTask task)
        {
            _tasksNext.Add(task);
        }

        private void Update()
        {
            (_tasksNext, _tasks) = (_tasks, _tasksNext);
            _tasksNext.Clear();

            for (int i = 0; i < _tasks.Count; i++)
            {
                var task = _tasks[i];
                if (task.Update())
                {
                    continue;
                }
                _tasksNext.Add(task);
            }
        }
    }
}
