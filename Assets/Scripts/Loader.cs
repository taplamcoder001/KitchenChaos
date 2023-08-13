using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader{
    
    public enum Scene{
        MainMenuScene,
        GameScene,
        LoadScene,
    }

    private static Scene targetSence;

    public static void Load(Scene targetScence)
    {
        Loader.targetSence = targetScence;

        SceneManager.LoadScene(Scene.LoadScene.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetSence.ToString());
    }
}
