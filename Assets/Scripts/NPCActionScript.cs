using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCActionScript : ActionScript {

    GameObject target;

	// Use this for initialization
	void Start () {
		Init();
	}
	
	// Update is called once per frame
	void Update () {

        float stamina = gameObject.GetComponent<StatsScript>().stamina;
        if(stamina <= 0.0 && moving == false && attacking == false && turn)
        {
            this.GetComponent<StatsScript>().ResetStamina();
            TurnManager.EndTurn();
        }
        if (turn)
        {
            if(moving || attacking)
            {
                if(moving)
                {
                    Move();
                }
                else if(attacking)
                {
                    Attack();
                }
            }
            else
            {
                FindNearestTarget();
                FindAttackTile();
                Tile TargetTile = GetTargetTile(target);
                if(TargetTile.attack)
                {
                    AttackToTile(TargetTile);
                }
                else if (!moving)
                {
                    CalculatePath();
                    FindGoTile();
                    actualTargetTile.target = true;
                }
            }

            StatsScript stats = gameObject.GetComponent<StatsScript>();
            //Debug.Log("Stamina: " + stats.stamina);
            //Debug.Log("Moving: " + moving);
            //Debug.Log("Attacking: " + attacking);
            /*
            FindNearestTarget();
            FindAttackTile();
            Tile TargetTile = GetTargetTile(target);
            if(TargetTile.attack)
            {
                AttackToTile(TargetTile);
            }
            else if (!moving)
            {
				CalculatePath();
                FindGoTile();
                actualTargetTile.target = true;
            }
            else
            {
                Move();
            }
            */

            if(lastMove == true && moving == false)
            {
                lastMove = false;
            }
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
