using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Loading : MonoBehaviour
{
    public static Loading Show()
    {
        var frame = ComponentLibrary.Get<Loading>();
        if (frame == null)
        {
            return null;
        }
        return frame;
    }

    public static Loading Show(Task task)
    {
        return Show()
            .AddTask(task);
    }

    public RectTransform Content;

    private Vector2 _syncedScreenSize;
    private readonly List<Task> _tasks = new();
    private bool _hasTask = false;

    public Loading AddTask(Task task)
    {
        _hasTask = true;
        _tasks.Add(task);
        return this;
    }

    public void Hide()
    {
        Return();
    }

    private void CheckTasks()
    {
        for (int i = 0; i < _tasks.Count; i++)
        {
            var task = _tasks[i];
            if (!task.IsCompleted)
            {
                continue;
            }

            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            _tasks.RemoveAt(i);
            i--;
        }

        if (_hasTask &&
            _tasks.Count == 0)
        {
            Return();
        }
    }

    private void OnEnable()
    {
        var canvas = FindObjectOfType<Canvas>();
        transform.SetParent(canvas.transform, false);
        var rectTransform = (RectTransform)transform;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.localScale = Vector3.one;
    }

    private void LateUpdate()
    {
        transform.SetAsLastSibling();
        Fit();
        CheckTasks();
    }

    private void Fit()
    {
        var screenSize = new Vector2(Screen.width, Screen.height);
        if (_syncedScreenSize == screenSize)
        {
            return;
        }
        _syncedScreenSize = screenSize;

        var scale = screenSize.y / 744;
        Content.localScale = Vector3.one * scale;
    }

    private void Return()
    {
        _hasTask = false;
        _tasks.Clear();
        ComponentLibrary.Return(this);
    }
}
