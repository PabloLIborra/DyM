using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCActionScript : ActionScript {

    GameObject target;

    bool distAttack = false;
    bool meleAttack = false;

    private void Start()
    {
        Init();
        enemies.Add(gameObject);
    }

    // Update is called once per frame
    void Update ()
    {
        if (!checkHealth() && gameObject.activeSelf == true)
        {
            int stamina = gameObject.GetComponent<StatsScript>().stamina;
            if (stamina <= 0.0 && moving == false && attacking == false && turn)
            {
                moving = false;
                distAttack = false;
                attacking = false;
                meleAttack = false;
                this.GetComponent<StatsScript>().ResetStamina();
                TurnManager.EndTurn();
            }
            if (turn)
            {
                if (moving || attacking)
                {
                    if (moving)
                    {
                        Move();
                    }
                    else if (attacking)
                    {
                        if(distAttack)
                        {
                            DistAttack();
                        }
                        else if (meleAttack)
                        {
                            MeleAttack();
                        }
                    }
                }
                else
                {
                    if (distAttack == false)
                    {
                        FindNearestTarget();
                        FindDistAttackTile();
                        Tile TargetTile = GetTargetTile(target);
                        if (TargetTile.attack && stamina >= distAttackCost && meleAttack == false)
                        {
                            DistAttackToTile(TargetTile);
                            distAttack = true;
                        }
                    }
                    
                    if(distAttack == false)
                    {
                        FindAttackTile();
                        Tile TargetTile = GetTargetTile(target);
                        if (TargetTile.attack && stamina >= meleAttackCost)
                        {
                            MeleAttackToTile(TargetTile);
                            meleAttack = true;
                        }
                    }

                    if (!moving && distAttack == false && meleAttack == false)
                    {
                        CalculatePath();
                        FindGoTile();
                        if(actualTargetTile != null)
                        {
                            actualTargetTile.target = true;
                        }
                        else
                        {
                            this.GetComponent<StatsScript>().ResetStamina();
                            moving = false;
                            distAttack = false;
                            attacking = false;
                            meleAttack = false;
                            RemoveSelectableTiles();
                            TurnManager.EndTurn();
                        }
                    }
                }

                StatsScript stats = gameObject.GetComponent<StatsScript>();

                if (lastMove == true && moving == false)
                {
                    lastMove = false;
                }
            }
        }
        else if (gameObject.activeSelf == false)
        {
            currentTile.Restart(gameObject, true);
        }
    }

	public void CalculatePath()
    {
        Tile TargetTile = GetTargetTile(target);
        FindPath(TargetTile);
    }

    public void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("player");

        GameObject nearest = null;
        float distance = Mathf.Infinity;

        foreach(GameObject obj in targets)
        {
            float d = Vector3.Distance(transform.position, obj.transform.position);
            
            if(d < distance)
            {
                distance = d;
                nearest = obj;
            }
        }


        target = nearest;
    }
}
