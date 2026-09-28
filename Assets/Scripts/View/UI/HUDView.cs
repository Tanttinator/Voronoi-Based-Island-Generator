using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDView
{
    public ConstructionMenuView ConstructionMenu { get; private set; }
    public TaskMenuView TaskMenu { get; private set; }

    VisualElement resourcesContainer;
    Dictionary<Resource, Label> resourceCounters = new Dictionary<Resource, Label>();

    VisualTreeAsset resourceCountTemplate;

    public IEnumerable<Resource> DisplayResources => resourceCounters.Keys;

    public HUDView(VisualElement root, VisualTreeAsset resourceCountTemplate, VisualTreeAsset constructionMenuItemTemplate)
    {
        ConstructionMenu = new ConstructionMenuView(root.Q("ConstructionMenu"), constructionMenuItemTemplate);
        TaskMenu = new TaskMenuView(root.Q("TaskMenu"), constructionMenuItemTemplate);

        resourcesContainer = root.Q<VisualElement>("Resources");
        this.resourceCountTemplate = resourceCountTemplate;
    }

    public void CreateResourceCounter(Resource resource)
    {
        TemplateContainer instance = resourceCountTemplate.Instantiate();
        resourceCounters[resource] = instance.Q<Label>("ResourceCount");
        resourcesContainer.Add(instance);
    }

    public bool HasCounterForResource(Resource resource) => resourceCounters.ContainsKey(resource);

    public void UpdateResourceCounter(Resource resource, int count)
    {
        resourceCounters[resource].text = $"{resource.name}: {count}";
    }
}
