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
            if (!attacking)
            {
                FindAttackTile();
                CheckMouse();
            }
            else
            {
                Attack();
            }

            if(lastMove == true && attacking == false)
            {
                attackButton = false;
                lastMove = false;
            }
        }
    }

    public void clickedWalk()
    {
        if(!moving && attackButton == false && Time.timeScale > 0 && turn)
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
        if (!attacking && walkButton == false && Time.timeScale > 0 && turn)
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
        if (!moving && Time.timeScale > 0 && turn)
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
                if(hit.collider.tag == "Tile" || hit.collider.tag == "Enemy")
                {
                    Tile t;
                    if (hit.collider.tag == "Enemy")
                    {
                        t = hit.collider.GetComponent<ActionScript>().currentTile;
                        if(t == null)
                        {
                            t = hit.collider.GetComponent<ActionScript>().actualTargetTile;
                        }
                    }
                    else
                    {
                        t = hit.collider.GetComponent<Tile>();
                    }
                    
                    
                    if (t != null && t.go)
                    {
                        MoveToTile(t);
                    }
                    else if(t != null && t.attack)
                    {
                        AttackToTile(t);
                    }
                }
                else if(hit.collider.tag == "player")
                {
                    Tile t = hit.collider.GetComponent<ActionScript>().currentTile;
                    if (t == null)
                    {
                        t = hit.collider.GetComponent<ActionScript>().actualTargetTile;
                    }
                    t.failAttack = true;
                }
            }
        }
    }

}
