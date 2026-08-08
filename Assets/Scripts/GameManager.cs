using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameViewResources viewResources;

    GameController controller;

    void Start()
    {
        controller = new GameController(viewResources);
        controller.StartGame();
    }

    void Update()
    {
        controller.Update();
    }
}
