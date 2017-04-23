using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    private Transform tutorialRoot;

    private void Start()
    {
        tutorialRoot = FindObjectOfType<Canvas>().transform.Find("Tutorial");
    }

    public void IntroNext()
    {
        tutorialRoot.Find("Intro").gameObject.SetActive(false);
        tutorialRoot.Find("Gameplay").gameObject.SetActive(true);
    }

    public void GameplayNext()
    {
        tutorialRoot.Find("Gameplay").gameObject.SetActive(false);
        tutorialRoot.Find("Enemies").gameObject.SetActive(true);
    }

    public void EnemiesPlay()
    {
        tutorialRoot.Find("Enemies").gameObject.SetActive(false);
        tutorialRoot.gameObject.SetActive(false);
        FindObjectOfType<Canvas>().transform.Find("HUD").gameObject.SetActive(true);
        GameManager.Instance.Pause(false, false);
    }

}
