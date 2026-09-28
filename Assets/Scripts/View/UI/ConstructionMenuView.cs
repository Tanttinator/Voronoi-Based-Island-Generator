using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ConstructionMenuView
{
    VisualElement buttonsContainer;
    Dictionary<Building, Button> buttons = new Dictionary<Building, Button>();

    VisualTreeAsset buttonsTemplate;

    public Action<Building> OnBuildingSelected;

    public ConstructionMenuView(VisualElement root, VisualTreeAsset buttonsTemplate)
    {
        buttonsContainer = root.Q("Content");
        this.buttonsTemplate = buttonsTemplate;
    }

    public void AddBuilding(Building building)
    {
        TemplateContainer instance = buttonsTemplate.Instantiate();
        Button button = instance.Q<Button>();
        button.text = building.name;
        button.clicked += () => SelectBuilding(building);
        buttons[building] = button;
        buttonsContainer.Add(instance);
    }

    void SelectBuilding(Building building)
    {
        OnBuildingSelected?.Invoke(building);
    }
}
