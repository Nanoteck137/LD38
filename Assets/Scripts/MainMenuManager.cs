using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public static bool selectMenu = false;

    private void OnEnable()
    {
        ShowSelectMenu(selectMenu);
        selectMenu = false;
    }

    public void OnStartGame()
    {
        ShowSelectMenu(true);
    }

    public void OnSelectEasy()
    {
        GameManager.difficulty = Difficulty.Easy;
        StartGame();
    }

    public void OnSelectMedium()
    {
        GameManager.difficulty = Difficulty.Medium;
        StartGame();
    }

    public void OnSelectHard()
    {
        GameManager.difficulty = Difficulty.Hard;
        StartGame();
    }

    public void OnSelectExtreme()
    {
        GameManager.difficulty = Difficulty.Extreme;
        StartGame();
    }

    public void OnSelectBack()
    {
        ShowSelectMenu(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("_SCENE_");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ShowSelectMenu(bool enable)
    {
        if(enable)
        {
            FindObjectOfType<Canvas>().transform.Find("Menu").gameObject.SetActive(false);
            FindObjectOfType<Canvas>().transform.Find("Notes").gameObject.SetActive(false);
            FindObjectOfType<Canvas>().transform.Find("Select Difficulty").gameObject.SetActive(true);
        }
        else
        {
            FindObjectOfType<Canvas>().transform.Find("Menu").gameObject.SetActive(true);
            FindObjectOfType<Canvas>().transform.Find("Notes").gameObject.SetActive(true);
            FindObjectOfType<Canvas>().transform.Find("Select Difficulty").gameObject.SetActive(false);
        }
    }

}
