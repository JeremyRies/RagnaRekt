using NDream.AirConsole;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField]private Object MenuControllerHtml;
    [SerializeField]private Object GameControllerHtml;

    private static LevelController _instance;

    public static LevelController Instance
    {
        get { return _instance; }
    }

    private void Start()
    {
        _instance = this;
    }

    public void LoadMenuScene()
    {
        AirConsole.instance.controllerHtml = MenuControllerHtml;
        SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
    }

    public void LoadGameScene()
    {
        AirConsole.instance.controllerHtml = GameControllerHtml;
        SceneManager.LoadSceneAsync("LevelNeo", LoadSceneMode.Single);
    }
}
