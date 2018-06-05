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
        if (walkButton == true)
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
        }
    }

    public void clickedWalk()
    {
        if (walkButton == false)
        {
            walkButton = true;
        }
        else
        {
            walkButton = false;
            Restart();
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

                    if(t.go)
                    {
                        MoveToTile(t);
                    }
                }
            }
        }
    }

    public void clickedAttack()
    {
        if (attackButton == false)
        {
            attackButton = true;
        }
        else
        {
            attackButton = false;
        }
        this.GetComponent<PlayerStatsScript>().UseStamina(4f);
        this.GetComponent<PlayerStatsScript>().Damage(20f);
    }

}
