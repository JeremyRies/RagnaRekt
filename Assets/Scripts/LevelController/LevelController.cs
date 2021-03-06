﻿using UnityEngine.SceneManagement;

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
        SceneManager.LoadSceneAsync("LevelNeo", LoadSceneMode.Single);
    }
}
