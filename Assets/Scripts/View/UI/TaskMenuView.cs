using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TaskMenuView
{
    VisualElement buttonsContainer;
    Dictionary<TaskType, Button> buttons = new Dictionary<TaskType, Button>();

    VisualTreeAsset buttonsTemplate;

    public Action<TaskType> OnTaskSelected;

    public TaskMenuView(VisualElement root, VisualTreeAsset buttonsTemplate)
    {
        buttonsContainer = root.Q("Content");
        this.buttonsTemplate = buttonsTemplate;
    }

    public void AddTask(TaskType task)
    {
        TemplateContainer instance = buttonsTemplate.Instantiate();
        Button button = instance.Q<Button>();
        button.text = task.ToString();
        button.clicked += () => SelectTask(task);
        buttons[task] = button;
        buttonsContainer.Add(instance);
    }

    void SelectTask(TaskType task)
    {
        OnTaskSelected?.Invoke(task);
    }
}
