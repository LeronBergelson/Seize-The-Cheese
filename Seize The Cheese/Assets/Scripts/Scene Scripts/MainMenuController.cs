﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    LevelStorage levelStorage = null;

    //defining menus
    public GameObject BaseMenu;
    public GameObject SettingsMenu;
    public GameObject LoadgameMenu;

    private void Start()
    {
        levelStorage = GameObject.Find("LevelStorage").GetComponent<LevelStorage>();

    }
    public void MoveToMainGame()
    {
        //don't destroy
        DontDestroyOnLoad(levelStorage.gameObject);

        //load MainGame (needs to be changed to level loading
        SceneManager.LoadScene(1);
    }
    public void MoveToLoadGame()
    {
        BaseMenu.gameObject.SetActive(false);
        LoadgameMenu.gameObject.SetActive(true);
    }
    public void MoveToSettings()
    {
        BaseMenu.gameObject.SetActive(false);
        SettingsMenu.gameObject.SetActive(true);
    }
    public void MoveToAbout()
    {

    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void MovetoHome()
    {
        BaseMenu.gameObject.SetActive(true);
        SettingsMenu.gameObject.SetActive(false);
        LoadgameMenu.gameObject.SetActive(false);
    }
}
