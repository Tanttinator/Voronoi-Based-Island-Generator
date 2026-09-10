using UnityEngine;
using UnityEngine.UIElements;

public class HUDView
{
    Label woodCountLabel;

    public HUDView(VisualElement root)
    {
        woodCountLabel = root.Q<Label>("WoodCount");
    }

    public void SetWoodCount(int count)
    {
        woodCountLabel.text = $"Wood: {count}";
    }
}
