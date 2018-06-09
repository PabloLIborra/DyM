using System.Collections;
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
}
