using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public void LoadScene()
    {
        SceneManager.LoadScene(1);
        GameManager.Instance.Init();
    }

    public void Exit()
    {
        Application.Quit();
    }

}
