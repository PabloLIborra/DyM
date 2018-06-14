using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionScript : ActionScript
{

    public bool walkButton = false;
    public bool attackButton = false;

    GameObject healthBar = null;
    GameObject staminaBar = null;

    // Use this for initialization
    void Start ()
    {
        Init();
	}
	
	// Update is called once per frame
	void Update ()
    {
        CheckMouse();
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
        if (!moving && attackButton == false && Time.timeScale > 0 && turn)
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
            gameObject.GetComponent<StatsScript>().Damage(10f);
        }
    }

    public void clickedTurn()
    {
        if (!moving && !attacking && Time.timeScale > 0 && turn)
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
                        setActiveBar(hit.collider.gameObject, true);
                    }
                    else
                    {
                        t = hit.collider.GetComponent<Tile>();
                        if(t.npc != null && t.npc.tag == "Enemy")
                        {
                            setActiveBar(t.npc, true);
                        }
                        else
                        {
                            setActiveBar(null, false);
                        }
                    }
                    

                    if (t != null && t.go && !moving && turn)
                    {
                        MoveToTile(t);
                    }
                    else if(t != null && t.attack && !attacking && turn)
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

                    setActiveBar(null, false);
                }
                else
                {
                    setActiveBar(null, false);
                }
            }
            else
            {
                setActiveBar(null, false);
            }
        }
    }

    public void setActiveBar(GameObject npc, bool active)
    {
        if(npc != null && active == true)
        {
            Debug.Log(npc);
            healthBar = npc.GetComponent<StatsScript>().healthBar.gameObject;
            healthBar.SetActive(true);
            staminaBar = npc.GetComponent<StatsScript>().staminaBar.gameObject;
            staminaBar.SetActive(true);
            npc.GetComponent<StatsScript>().updateVisibleBar();
        }
        else
        {
            if (healthBar != null)
            {
                healthBar.SetActive(false);
                healthBar = null;
            }
            if (staminaBar != null)
            {
                staminaBar.SetActive(false);
                staminaBar = null;
            }
        }
    }

}
