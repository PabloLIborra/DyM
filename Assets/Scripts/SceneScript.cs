using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//For reading and writing files
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//======================================
using System;
using System.Runtime.Hosting;
using System.Security.Cryptography;
using System.Collections.Specialized;
using System.Diagnostics;


[Serializable]
class GameData
{
	public int scene;
	public CharacterData[] players;
	public CharacterData[] enemies;
}

[Serializable]
class CharacterData
{
	public float health;
	public int stamina;
	public bool turn;
	public float position_x;
	public float position_y;
	public float position_z;
}

//======================================
public class SceneScript : MonoBehaviour
{
	public static int counter;

    float time = 1f;

	public int scene;
	public static bool loadedGame;

    public Material matPause;
    bool canvasPause = false;

    private void Update()
    {

        if (checkFileExists())
        {
            GameObject loadMatch = GameObject.Find("Cargar Partida Menu Boton");
            if(loadMatch != null)
            {
                loadMatch.GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            GameObject loadMatch = GameObject.Find("Cargar Partida Menu Boton");
            if (loadMatch != null)
            {
                loadMatch.GetComponent<Button>().interactable = false;
            }
        }


        if (canvasPause == false)
        {
            if(GameObject.Find("CanvasPause") != null)
            {
                GameObject.Find("CanvasPause").GetComponent<Canvas>().enabled = false;
            }
        }

		//If the game was loaded and the scene changed in the previous scene, load the game data now
		if (loadedGame && counter > 5) {
			LoadGameData ();

			loadedGame = false;
		}
		counter++;
    }

    public void LoadScene()
	{
		//State that the game is newly generated
		loadedGame = false;

        SceneManager.LoadScene(scene);
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

        ActionScript.players.Clear();
        ActionScript.enemies.Clear();
    }

    public void StartMatch()
    {
        //State that the game is newly generated
        loadedGame = false;

        SceneManager.LoadScene("Tutorial");
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

        ActionScript.players.Clear();
        ActionScript.enemies.Clear();
    }

    public void NextScene()
    {
        //State that the game is newly generated
        loadedGame = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
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

        ActionScript.players.Clear();
        ActionScript.enemies.Clear();
    }

    public void RechargeScene()
    {
        //State that the game is newly generated
        loadedGame = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

        ActionScript.players.Clear();
        ActionScript.enemies.Clear();
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

        ActionScript.players.Clear();
        ActionScript.enemies.Clear();
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

            GameObject canvas = GameObject.Find("CanvasPause");
            canvas.GetComponent<Canvas>().enabled = true;
            canvasPause = true;

            GameObject[] players = GameObject.FindGameObjectsWithTag("player");

            bool block = true;
            foreach (var player in players)
            {
                if(player.GetComponent<ActionScript>().turn == true)
                {
                    if(player.GetComponent<ActionScript>().moving == false)
                    {
                        if (player.GetComponent<ActionScript>().attacking == false)
                        {
                            block = false;
                        }
                    }
                }
            }

            GameObject saveButton = canvas.transform.FindChild("Save").gameObject;
            if (block == false)
            {
                if(saveButton != null)
                {
                    saveButton.GetComponent<Button>().interactable = true;
                }
            }
            else
            {
                if (saveButton != null)
                {
                    saveButton.GetComponent<Button>().interactable = false;
                }
            }

        }
        else if(time == 0f)
        {
            time = 1f;
            Time.timeScale = time;

            GameObject.Find("CanvasPause").GetComponent<Canvas>().enabled = false;
            canvasPause = false;
        }
    }

	//Click to load the game
	public void clickLoadGameData()
	{
        //Reading files
        BinaryFormatter bf = new BinaryFormatter (); 									  //Transcription to binary
		FileStream f = File.Open(Application.dataPath + "/Save/Game.dat", FileMode.Open); //File reading

		//Read data
		GameData data = (GameData) bf.Deserialize(f);

		//Close file
		f.Close();

		//LOAD DATA
		scene = data.scene;

		//LOAD SCENE
		LoadScene();

		//Claim the game is loaded and the variables need to be addressed
		counter = 0;
		loadedGame = true;
	}

	//Check if file exists
	public bool checkFileExists()
	{
		if (File.Exists (Application.dataPath + "/Save/Game.dat")) {
			return true;
		}

		return false;
	}

	//Load raw data from file
	public void LoadGameData()
	{
		//Check existence
		if(File.Exists(Application.dataPath + "/Save/Game.dat"))
		{
			//Clean all tiles before reassigning positions
			GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
			foreach (GameObject tile in tiles) {
				tile.GetComponent<Tile>().Restart (null, true);
			}

			//Reading files
			BinaryFormatter bf = new BinaryFormatter (); 									  //Transcription to binary
			FileStream f = File.Open(Application.dataPath + "/Save/Game.dat", FileMode.Open); //File reading

			//Read data
			GameData data = (GameData) bf.Deserialize(f);

			//Close file
			f.Close();

			//SET DATA AFTER LOADING THE SCENE
			GameObject[] players = GameObject.FindGameObjectsWithTag("player");
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

			CharacterData[] playersData = data.players; 
			CharacterData[] enemiesData = data.enemies; 

			for(int i = 0; i < playersData.Length; i++)
			{
				players[i].GetComponent<PlayerStatsScript>().health     = playersData[i].health;
				players[i].GetComponent<PlayerStatsScript>().stamina    = playersData[i].stamina;
				players[i].GetComponent<PlayerActionScript>().turn      = playersData[i].turn;
				Vector3 position = new Vector3 (playersData [i].position_x, playersData [i].position_y, playersData [i].position_z);
				players [i].transform.position = position;
                players[i].GetComponent<ActionScript>().Init();


            }

			for(int i = 0; i < enemiesData.Length; i++)
			{
				enemies[i].GetComponent<NPCStatsScript>().health = enemiesData [i].health; 
				enemies[i].GetComponent<NPCStatsScript>().stamina = enemiesData [i].stamina;
				enemies[i].GetComponent<NPCActionScript>().turn = enemiesData [i].turn;
				Vector3 position = new Vector3 (enemiesData [i].position_x, enemiesData [i].position_y, enemiesData [i].position_z);
				enemies[i].transform.position = position;
                enemies[i].GetComponent<ActionScript>().Init();
            }
		}
	}

	//Click to save the data
	public void clickSaveGameData()
	{
		//Open data readers
		BinaryFormatter bf = new BinaryFormatter (); 					     //Transcription to binary
		FileStream f = File.Create(Application.dataPath + "/Save/Game.dat"); //File reading

		//Create data file
		GameData data = new GameData();

		//Fill data
		//data.scene = scene;
		data.scene = SceneManager.GetActiveScene().buildIndex;

		GameObject[] players = GameObject.FindGameObjectsWithTag("player");
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

		CharacterData[] playersData = new CharacterData[players.Length]; 
		CharacterData[] enemiesData = new CharacterData[enemies.Length]; 

		for(int i = 0; i < players.Length; i++)
		{
			playersData [i] = new CharacterData ();
			playersData[i].health 	  = players[i].GetComponent<PlayerStatsScript>().health;
			playersData[i].stamina    = players[i].GetComponent<PlayerStatsScript>().stamina;
			playersData[i].turn       = players[i].GetComponent<PlayerActionScript>().turn;
			playersData[i].position_x = players[i].transform.position.x;
			playersData[i].position_y = players[i].transform.position.y;
			playersData[i].position_z = players[i].transform.position.z;

		}

		for(int i = 0; i < enemies.Length; i++)
		{
			enemiesData[i] = new CharacterData ();
			enemiesData[i].health 	  = enemies[i].GetComponent<NPCStatsScript>().health;
			enemiesData[i].stamina    = enemies[i].GetComponent<NPCStatsScript>().stamina;
			enemiesData[i].turn       = enemies[i].GetComponent<NPCActionScript>().turn;
			enemiesData[i].position_x = enemies[i].transform.position.x;
			enemiesData[i].position_y = enemies[i].transform.position.y;
			enemiesData[i].position_z = enemies[i].transform.position.z;
		}

		data.players = playersData;
		data.enemies = enemiesData;

		//Write file
		bf.Serialize(f, data);

		//Wrapping methods and files
		f.Close ();

		//Close pause menu
		PauseGame();
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
