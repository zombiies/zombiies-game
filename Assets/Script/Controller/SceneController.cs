using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    public void OpenScene(SCENE_NAME _name, Action _success = null)
    {
        SceneManager.LoadScene(_name.ToString(), LoadSceneMode.Single);
        if (_success != null)
            _success();
    }
}
public enum SCENE_NAME
{
    Login,
    Home,
    Game
}
