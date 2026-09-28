using UnityEngine;
using UnityEngine.InputSystem;

public class InputProvider
{
    public bool Clicked { get; private set; }
    public bool Held { get; private set; }
    public float Scroll { get; private set; }
    public Vector3 MouseWorldPos { get; private set; }

    Plane interactionPlane = new Plane(Vector3.up, 0f);

    public void Update()
    {
        Clicked = Mouse.current.leftButton.wasPressedThisFrame;
        Held = Mouse.current.leftButton.isPressed;
        Scroll = Mouse.current.scroll.value.y;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
        if (interactionPlane.Raycast(ray, out float dist))
        {
            MouseWorldPos = ray.GetPoint(dist);
        }
        else
        {
            MouseWorldPos = new Vector3(-1000, -1000, -1000);
        }
    }
}
