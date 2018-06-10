using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionScript : ActionScript
{

    public bool walkButton = false;
    public bool attackButton = false;

    // Use this for initialization
    void Start ()
    {
        Init();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //WALK
        if (walkButton == true && Time.timeScale > 0 && turn)
        {
            if (!moving)
            {
                FindGoTile();
                CheckMouse();
            }
            else
            {
                Move();
            }

            if(lastMove == true && moving == false)
            {
                walkButton = false;
                lastMove = false;
            }
        }

        //ATTACK
        if (attackButton == true && Time.timeScale > 0 && turn)
        {
            FindAttackTile();
        }
    }

    public void clickedWalk()
    {
        if(!moving && attackButton == false && Time.timeScale > 0)
        {
            if (walkButton == false)
            {
                walkButton = true;
            }
            else
            {
                walkButton = false;
                RemoveSelectableTiles();
            }
        }     
    }

    public void clickedAttack()
    {
        if (!moving && walkButton == false && Time.timeScale > 0)
        {
            if (attackButton == false)
            {
                attackButton = true;
            }
            else
            {
                attackButton = false;
                RemoveSelectableTiles();
            }
        }
    }

    public void clickedTurn()
    {
        if (!moving && Time.timeScale > 0)
        {
            this.GetComponent<StatsScript>().ResetStamina();
            TurnManager.EndTurn();
            attackButton = false;
            walkButton = false;
        }
    }

    void CheckMouse()
    {
        if(Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.tag == "Tile")
                {
                    Tile t = hit.collider.GetComponent<Tile>();

                    
                    if (t.go)
                    {
                        MoveToTile(t);
                    }
                }
            }
        }
    }

}
