using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    public int move = 5;
    public float distJump = 2;
    public float moveSpeed = 2;

    Vector3 speed = new Vector3();
    Vector3 heading = new Vector3();

    List<Tile> selectTiles = new List<Tile>();
    GameObject[] tiles;

    Stack<Tile> stack = new Stack<Tile>();
    Tile currentTile;

    float half = 0;

    //Check the tiles and fill the array
    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        half = this.GetComponent<Collider>().bounds.extents.y;
    }

    //Take actual tile and draw the box
    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject); 
        currentTile.actual = true;                  //This draw the box where we are
    }

    //Use a raycast to check tile under the player
    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit ray;

        Tile tile = null;

        Debug.Log(Vector3.down);

        if (Physics.Raycast(target.transform.position, Vector3.down, out ray, 1))
        {
            tile = ray.collider.GetComponent<Tile>();
        }
        return tile;
    }

    //Check neighbors
    public void ProcessAdjList()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            Tile t = tiles[i].GetComponent<Tile>();
            t.Neighbors(distJump);
        }
    }

    //Use the stack to now  the order of tiles and draw them according to their state
    public void FindGoTile()
    {
        ProcessAdjList();
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;

        while(process.Count > 0)
        {
            Tile t = process.Dequeue();
            selectTiles.Add(t);
            t.go = true;

            if (t.dist < move)
            {
                for (int i = 0; i < t.adjList.Count; i++)
                {
                    Tile tile = t.adjList[i];
                    if (tile.visited == false)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.dist = 1 + t.dist;

                        process.Enqueue(tile);
                    }
                }
            }
        }
    }
}
