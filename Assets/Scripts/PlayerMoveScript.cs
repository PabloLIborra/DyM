using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoveScript : MoveScript
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
        if (attackButton == true && Time.timeScale > 0)
        {
           
        }
    }

    public void clickedWalk()
    {
        if(!moving && Time.timeScale > 0)
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

                    //Here we calculate how much stamina use
                    StatsScript stats = gameObject.GetComponent<StatsScript>();
                    int useStamina = 0;
                    Tile tile = t;
                    for (int i = 0; i < move && tile.parent != null; i++)
                    {
                        tile = t.parent;
                        useStamina++;
                    }

                    if (t.go && stats.stamina >= useStamina)
                    {
                        stats.UseStamina((float)useStamina);
                        MoveToTile(t);
                    }
                }
            }
        }
    }

    public void clickedAttack()
    {
        if(!moving && Time.timeScale > 0)
        {
            if (attackButton == false)
            {
                attackButton = true;
            }
            else
            {
                attackButton = false;
            }
            this.GetComponent<PlayerStatsScript>().UseStamina(1f);
            this.GetComponent<PlayerStatsScript>().Damage(20f);
        }
    }

}
