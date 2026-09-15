using UnityEngine;

public class TaskZoneIndicatorView
{
    GameObject instance;
    MeshRenderer icon;
    LineRenderer border;

    public TaskZoneIndicatorView(GameObject indicatorPrefab, Vector3 position, Vector3[] borderPoints, Texture2D iconTexture, Color color)
    {
        instance = GameObject.Instantiate(indicatorPrefab, position, Quaternion.identity);
        icon = instance.GetComponentInChildren<MeshRenderer>();

        border = instance.GetComponentInChildren<LineRenderer>();
        border.positionCount = borderPoints.Length;
        border.SetPositions(borderPoints);

        Material iconMaterial = icon.material;
        iconMaterial.mainTexture = iconTexture;
        iconMaterial.color = color;

        Material borderMaterial = border.material;
        borderMaterial.color = color;
    }

    public void Destroy()
    {
        GameObject.Destroy(instance);
    }
}
