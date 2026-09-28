using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] float speed = 1f;

    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * speed);
    }
}
