using Control.Airconsole;
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
        SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
        SetupAirconsole();
        AirConsole.instance.controllerHtml = MenuControllerHtml;
    }

    private void SetupAirconsole()
    {
        var airconsoleObject = new GameObject("Airconsole");

       // airconsoleObject.AddComponent<AirConsole>();
        airconsoleObject.AddComponent<AirConsoleConnectionService>();
    }

    public void LoadGameScene()
    {
        AirConsole.instance.controllerHtml = GameControllerHtml;
        SceneManager.LoadSceneAsync("LevelNeo", LoadSceneMode.Single);
    }
}
