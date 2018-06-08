using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    public bool turn = false;
    public int move = 5;
    public float distJump = 2;
    public float moveSpeed = 2;
    public bool moving = false;
    public int raycast = 1;

    Vector3 speed = new Vector3();
    Vector3 heading = new Vector3();

    List<Tile> selectTiles = new List<Tile>();
    GameObject[] tiles;

    Stack<Tile> stack = new Stack<Tile>();
    Tile currentTile;

    float half = 0;

    public Tile actualTargetTile;

    protected bool lastMove = false;          //Use on Player Move Script to uncheck Button


    //Check the tiles and fill the array
    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        half = this.GetComponent<Collider>().bounds.extents.y;

        TurnManager.AddUnit(this);
    }

    //Take actual tile and draw the box
    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject); 
        currentTile.actual = true;                  //This draw the box where we are
    }

    //Use a raycast to check tile under the player
    public Tile GetTargetTile(GameObject t)
    {
        RaycastHit ray;

        Tile tile = null;

        if (Physics.Raycast(t.transform.position, Vector3.down, out ray, raycast))
        {
            tile = ray.collider.GetComponent<Tile>();
        }
        return tile;
    }

    //Check neighbors
    public void ProcessAdjList(float distJump, Tile target)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            Tile t = tiles[i].GetComponent<Tile>();
            t.Neighbors(distJump, target);
        }
    }

    //Use the stack to now  the order of tiles and draw them according to their state
    public void FindGoTile()
    {
        ProcessAdjList(distJump, null);
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

    public void Restart()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            Tile t = tiles[i].GetComponent<Tile>();
            t.Restart();
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

                lastMove = true;
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

            TurnManager.EndTurn();
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

    protected Tile FindLowestF(List<Tile> list)
    {
        Tile lowest = list[0];

        foreach(Tile t in list)
        {
            if(t.f < lowest.f)
            {
                lowest = t;
            }
        }

        list.Remove(lowest);

        return lowest;
    }

    protected Tile FindEndTile(Tile t)
    {
        Stack<Tile> tempPath = new Stack<Tile>();

        Tile next = t.parent;
        while(next != null)
        {
            tempPath.Push(next);
            next = next.parent;
        }

        if(tempPath.Count <= move)
        {
            return t.parent;
        }

        Tile endTile = null;
        for(int i = 0; i <= move; i++)
        {
            endTile = tempPath.Pop();
        }

        return endTile;
    }

    protected void FindPath(Tile target)
    {
        ProcessAdjList(distJump, target);
        GetCurrentTile();

        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(currentTile);
        currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position);
        currentTile.f = currentTile.h;

        while(openList.Count > 0)
        {
            Tile t = FindLowestF(openList);

            closedList.Add(t);

            if(t == target)
            {
                actualTargetTile = FindEndTile(t);
                MoveToTile(actualTargetTile);
                return;
            }

            foreach(Tile tile in t.adjList)
            {
                if(closedList.Contains(tile))
                {
                    //Do nothing, already processed
                }
                else if(openList.Contains(tile))
                {
                    float tempG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);

                    if(tempG < tile.g)
                    {
                        tile.parent = t;
                        tile.g = tempG;
                        tile.f = tile.g + tile.h;
                    }
                }
                else
                {
                    tile.parent = t;

                    tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                    tile.h = Vector3.Distance(tile.transform.position, target.transform.position);
                    tile.f = tile.g + tile.h;

                    openList.Add(tile);
                }


            }
        }

    }

    public void BeginTurn()
    {
        turn = true;
    }

    public void EndTurn()
    {
        turn = false;
    }
}
