using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    public int move = 5;
    public float distJump = 2;
    public float moveSpeed = 2;
    public bool moving = false;

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

    public void MoveToTile(Tile tile)
    {
        tile.target = true;
        moving = true;
        stack.Clear();

        Tile next = tile;
        while(next != null)
        {
            stack.Push(next);
            next = next.parent;
        }
    }

    public void Move()
    {
        if(stack.Count > 0)
        {
            Tile t = stack.Peek();
            Vector3 pos = t.transform.position;

            pos.y += half;

            if(Vector3.Distance(transform.position, pos) >= 0.05f)
            {
                calculateHeading(pos);
                setHorizontalVelocity();

                transform.forward = heading;
                transform.position += speed * Time.deltaTime;
            }
            else
            {
                transform.position = pos;
                stack.Pop();
            }
        }
        else
        {   
            RemoveSelectableTiles();
            moving = false;
        }
    }

    protected void RemoveSelectableTiles()
    {
        if(currentTile != null)
        {
            currentTile.target = false;
            currentTile = null;
        }

        foreach (Tile tile in selectTiles)
        {
            tile.Restart();
        }

        selectTiles.Clear();
    }

    void calculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }
    
    void setHorizontalVelocity()
    {
        speed = heading * moveSpeed;
    }
}
