using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool walk = true;
    public bool actual = false;
    public bool target = false;
    public bool go = false;
    public bool obstacle = false;

    public List<Tile> adjList = new List<Tile>();

    //About tile we are 
    public bool visited = false;
    public Tile parent = null;
    public int dist = 0;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
    //We draw boxes according the player position
	void Update ()
    {
        if(actual == true)  //Where we are
        {
            this.GetComponent<Renderer>().material.color = Color.cyan;
        }
        else if (target == true)    //Where we are going
        {
            this.GetComponent<Renderer>().material.color = Color.green;
        }
        else if (go == true)    //Where we can go
        {
            this.GetComponent<Renderer>().material.color = Color.red;
        }
        else    //Rest of boxes
        {
            this.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    //Reset the variables.
    public void Restart()
    {
        adjList.Clear();

        actual = false;
        target = false;
        go = false;

        visited = false;
        parent = null;
        dist = 0;
    }

    //Check again the neighbors according to new tile where we are
    public void Neighbors(float distJump)
    {
        Restart();

        checkTile(Vector3.forward, distJump);
        checkTile(-Vector3.forward, distJump);
        checkTile(Vector3.right, distJump);
        checkTile(-Vector3.right, distJump);
    }

    //We use a raycast to check the diferents boxes around us
    public void checkTile(Vector3 dir, float distJump)  
    {
        Vector3 half = new Vector3(0.25f, (1 + distJump)/2.0f, 0.25f);
        Collider[] coll = Physics.OverlapBox(transform.position + dir, half);

        for (int i = 0; i < coll.Length; i++)
        {
            Tile tile = coll[i].GetComponent<Tile>();
            if(tile != null && tile.walk == true && tile.obstacle == false)
            {
                RaycastHit ray;
                if (Physics.Raycast(tile.transform.position, Vector3.up, out ray, 1) == false)
                {
                    adjList.Add(tile);
                }
            }
        }
    }
}
