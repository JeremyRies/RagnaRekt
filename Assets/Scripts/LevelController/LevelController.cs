using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController
{
    private static LevelController _instance = null;

    public static LevelController GetInstance()
    {
        if (_instance == null)
            _instance = new LevelController();

        return _instance;
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync("JoshWithCollider 1", LoadSceneMode.Single);
    }
}
