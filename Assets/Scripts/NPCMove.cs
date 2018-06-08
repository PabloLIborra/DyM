using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMove : MoveScript {

    GameObject target;

	// Use this for initialization
	void Start () {
		Init();
	}
	
	// Update is called once per frame
	void Update () {
		if (turn)
        {
            if (!moving)
            {
                Debug.Log("Entra");
				FindNearestTarget();
                Debug.Log("Aqui si");
				CalculatePath();
                Debug.Log("Aqui bien");
                FindGoTile();
                actualTargetTile.target = true;
            }
            else
            {
                Move();
            }

            if(lastMove == true && moving == false)
            {
                lastMove = false;
            }
        }
	}

	void CalculatePath()
    {
        Tile TargetTile = GetTargetTile(target);
        FindPath(TargetTile);
    }

    void FindNearestTarget()
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
