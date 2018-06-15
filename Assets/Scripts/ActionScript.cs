using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ActionScript : MonoBehaviour
{
    public bool turn = false;
    public int move = 5;
    private int maxMoveAI = 5;
    public float distJump = 2;
    public int distMeleAttack = 2;
    public int distDistanceAttack = 2;
    public float moveSpeed = 2;
    public bool moving = false;
    public bool attacking = false;
    public int raycast = 1;

    public int meleAttackCost = 3;
    public int distAttackCost = 3;

    Vector3 speed = new Vector3();
    Vector3 heading = new Vector3();

    List<Tile> selectTiles = new List<Tile>();
    GameObject[] tiles;

    Stack<Tile> stack = new Stack<Tile>();
    public Tile currentTile;

    float half = 0;

    public Tile actualTargetTile;

    protected bool lastMove = false;          //Use on Player Move Script to uncheck Button

    public bool firstEnter = false;

    public static List<GameObject> players = new List<GameObject>();
    public static List<GameObject> enemies = new List<GameObject>();

    protected bool meleAttackDo = false;

    //Check the tiles and fill the array
    public void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        half = this.GetComponent<Collider>().bounds.extents.y;

        TurnManager.AddUnit(this);
        
        currentTile = GetTargetTile(gameObject);
        currentTile.npc = gameObject;

        maxMoveAI = move;
    }

    protected bool checkHealth()
    {
        //DEFEAT
        int countKills = 0;
        foreach (var player in players)
        {
            if(player.GetComponent<StatsScript>().health <= 0)
            {
                if (player.activeSelf == false)
                {
                    countKills++;
                }
            }
        }
        if (countKills == players.Count)
        {
            Canvas endCanvas = GameObject.Find("EndMatchCanvas").GetComponent<Canvas>();
            endCanvas.enabled = true;
            endCanvas.transform.GetChild(1).gameObject.SetActive(true);
			GameObject musMan = GameObject.Find ("MusicManager");
            musMan.transform.GetChild(0).GetComponent<AudioSource>().Stop();
            musMan.transform.GetChild(2).GetComponent<AudioSource>().Play();
            moving = false;
            attacking = false;
            return true;
        }

        //VICTORY
        countKills = 0;
        foreach (var enemy in enemies)
        {
            if (enemy.GetComponent<StatsScript>().health <= 0)
            {
                if (enemy.activeSelf == false)
                {
                    countKills++;
                }
            }
        }
        if (countKills == enemies.Count)
        {
            Canvas endCanvas = GameObject.Find("EndMatchCanvas").GetComponent<Canvas>();
            endCanvas.enabled = true;
            endCanvas.transform.GetChild(0).gameObject.SetActive(true);
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
            {
				GameObject musMan = GameObject.Find ("MusicManager");
                musMan.transform.GetChild(0).GetComponent<AudioSource>().Stop();
                musMan.transform.GetChild(1).GetComponent<AudioSource>().Play();
                GameObject nextScene = GameObject.Find("NextLevelButton");
                nextScene.GetComponent<Button>().interactable = false;
            }
            moving = false;
            attacking = false;
            return true;
        }
        return false;
    }

    //Take actual tile and draw the box
    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.actual = true;                  //This draw the box where we are
        currentTile.npc = gameObject;
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
    public void ProcessAdjList(float distJump, Tile target, bool attack)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            Tile t = tiles[i].GetComponent<Tile>();
            if(attack == false)
            {
                t.Neighbors(distJump, target);
            }
            else
            {
                t.NeighborsAttack(distJump, target);
            }
            
        }
    }

    //Use the stack to now  the order of tiles and draw them according to their state
    public void FindGoTile()
    {
        ProcessAdjList(distJump, null, false);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;

        while(process.Count > 0)
        {
            Tile t = process.Dequeue();

            selectTiles.Add(t);
            t.go = true;

            int stamina = gameObject.GetComponent<StatsScript>().stamina;

            move = stamina;

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

    public void FindAttackTile()
    {
        ProcessAdjList(distJump, null, true);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            selectTiles.Add(t);
            t.attack = true;

            if (t.dist < distMeleAttack)
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

    public void FindDistAttackTile()
    {
        ProcessAdjList(distJump, null, true);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            selectTiles.Add(t);
            t.attack = true;

            if (t.dist < distDistanceAttack && t.dist > distMeleAttack)
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
            else if(t.dist <= distMeleAttack)
            {
                t.attack = false;
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
        //Here we calculate how much stamina use
        StatsScript stats = gameObject.GetComponent<StatsScript>();
        int wasted = tile.dist;

        //////////Case to Enemy
        if(gameObject.tag == "Enemy")
        {
            if(stats.stamina - move >= 0)
            {
                wasted = move;
            }
            else if(stats.stamina - move > -move)
            {
                move = (int)stats.stamina;
                NPCActionScript npcAction = gameObject.GetComponent<NPCActionScript>();
                npcAction.FindNearestTarget();
                npcAction.CalculatePath();
                wasted = (int)stats.stamina;
                move = maxMoveAI;
                return;
            }
            else
            {
                wasted = -1;
            }
        }
        /////////////////

        if (stats.stamina >= wasted && wasted != -1)
        {
            stats.UseStamina(wasted);
            tile.target = true;
            moving = true;
            stack.Clear();

            Tile next = tile;
            while (next != null)
            {
                stack.Push(next);
                next = next.parent;
            }
        }
        
    }

    public void MeleAttackToTile(Tile tile)
    {
        StatsScript stats = gameObject.GetComponent<StatsScript>();

        if (tile.npc != null && tile.npc != gameObject)
        {
            if (stats.stamina >= meleAttackCost)
            {
                stats.UseStamina(meleAttackCost);

                tile.target = true;
                attacking = true;
                stack.Clear();

                stack.Push(tile);

            }
            else
            {
                return;
            }
        }
        else
        {
            tile.failAttack = true;
        }
        

    }

    public void DistAttackToTile(Tile tile)
    {
        StatsScript stats = gameObject.GetComponent<StatsScript>();

        if (tile.npc != null && tile.npc != gameObject)
        {
            if (stats.stamina >= distAttackCost)
            {
                stats.UseStamina(distAttackCost);

                tile.target = true;
                attacking = true;
                stack.Clear();

                stack.Push(tile);

            }
            else
            {
                return;
            }
        }
        else
        {
            tile.failAttack = true;
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
                if (stack.Count == 1)
                {
                    t.npc = gameObject;
                    actualTargetTile = t;
                }
                stack.Pop();
            }
        }
        else
        {   
            moving = false; 
            RemoveSelectableTiles();
            move = maxMoveAI;
        }
    }

    public void MeleAttack()
    {
        if (stack.Count > 0)
        {
            Tile t = stack.Peek();
            int playerDmg = gameObject.GetComponent<StatsScript>().meleAttackDmg;
            t.npc.GetComponent<StatsScript>().Damage(playerDmg);
            lastMove = true;
            stack.Pop();
        }
        else
        {
            RemoveSelectableTiles();
            attacking = false;
            meleAttackDo = true;
        }
    }
    
    public void DistAttack()
    {
        if (stack.Count > 0)
        {
            Tile t = stack.Peek();
            int playerDmg = gameObject.GetComponent<StatsScript>().distAttackDmg;
            t.npc.GetComponent<StatsScript>().Damage(playerDmg);
            lastMove = true;
            stack.Pop();
        }
        else
        {
            RemoveSelectableTiles();
            attacking = false;
        }
    }

    protected void RemoveSelectableTiles()
    {
        currentTile = GetTargetTile(gameObject);

        foreach (Tile tile in selectTiles)
        {
            if (tile != currentTile)
            {
                tile.Restart(gameObject, true);
            }
            else
            {
                tile.Restart(gameObject, false);
            }
        }

        if (currentTile != null)
        {
            currentTile.target = false;
            currentTile = null;
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
        ProcessAdjList(distJump, target, false);
        GetCurrentTile();

        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(currentTile);
        currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position);
        currentTile.f = currentTile.h;

        while (openList.Count > 0)
        {
            Tile t = FindLowestF(openList);
            
            closedList.Add(t);
            if (t == target)
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
