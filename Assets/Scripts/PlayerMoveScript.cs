using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveScript : MoveScript
{

	// Use this for initialization
	void Start ()
    {
        Init();
	}
	
	// Update is called once per frame
	void Update ()
    {
        FindGoTile();
    }
}
