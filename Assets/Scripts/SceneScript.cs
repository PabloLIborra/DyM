﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneScript : MonoBehaviour
{

    float time = 1f;

    public int scene = 0;
    public Material matPause;
    bool canvasPause = false;

    private void Update()
    {
        if(canvasPause == false)
        {
            if(GameObject.Find("CanvasPause") != null)
            {
                GameObject.Find("CanvasPause").GetComponent<Canvas>().enabled = false;
            }
        } 
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(scene);
        time = 1f;
        Time.timeScale = time;

        if (GameObject.Find("CanvasPause") != null)
        {
            GameObject.Find("CanvasPause").GetComponent<Canvas>().enabled = false;
        }
        canvasPause = false;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        time = 1f;
        Time.timeScale = time;

        if (GameObject.Find("CanvasPause") != null)
        {
            GameObject.Find("CanvasPause").GetComponent<Canvas>().enabled = false;
        }
        canvasPause = false;
        TurnManager.units.Clear();
        TurnManager.turnKey.Clear();
        TurnManager.turnTeam.Clear();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        if(time == 1f)
        {
            time = 0f;
            Time.timeScale = time;

            GameObject.Find("CanvasPause").GetComponent<Canvas>().enabled = true;
            canvasPause = true;

        }
        else if(time == 0f)
        {
            time = 1f;
            Time.timeScale = time;

            GameObject.Find("CanvasPause").GetComponent<Canvas>().enabled = false;
            canvasPause = false;
        }
    }

    public void LeftRotate()
    {
        GameObject map = GameObject.FindGameObjectWithTag("Game");
        map.transform.Rotate(0,90,0);
    }
    public void RightRotate()
    {
        GameObject map = GameObject.FindGameObjectWithTag("Game");
        map.transform.Rotate(0, -90, 0);
    }
}
