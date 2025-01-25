using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        {
            DontDestroyOnLoad(gameObject);
        }
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void Level()
    {
        SceneManager.LoadScene("Level");
    }
    public void Win()
    {
        SceneManager.LoadScene("Win");
    }
    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void Mainmenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void salir()
    {
        Application.Quit();
    }

}



