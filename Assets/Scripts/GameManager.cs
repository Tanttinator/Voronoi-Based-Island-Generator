using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameConfig config;
    [SerializeField] GameViewResources viewResources;

    GameController controller;

    void Start()
    {
        controller = new GameController(config, viewResources);
        controller.StartGame();
        StartCoroutine(controller.PlaySequences());
    }

    void Update()
    {
        controller.Update();
    }
}
