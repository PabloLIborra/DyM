using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

	public static Dictionary<string, List<ActionScript>> units = new Dictionary<string, List<ActionScript>>();
    public static Queue<string> turnKey = new Queue<string>();
    public static Queue<ActionScript> turnTeam = new Queue<ActionScript>();

    // Use this for initialization
    void Start () {
        
	}
    void Init()
    {
        
    }

    // Update is called once per frame
    void Update () {
		if(turnTeam.Count == 0)
		{
			InitTeamTurnQueue();/*
            ActionScript turn = turnTeam.Dequeue();
            while (turn.gameObject.tag != "player")
            {
                EndTurn();
                InitTeamTurnQueue();
            }*/
        }
	}

	static void InitTeamTurnQueue()
	{
        if(turnKey.Count > 0)
        {
            List<ActionScript> teamList = units[turnKey.Peek()];

            if (teamList != null)
            {
                foreach (ActionScript unit in teamList)
                {
                    turnTeam.Enqueue(unit);
                }
                StartTurn();
            }
        }
	}

	public static void StartTurn()
	{
		if(turnTeam.Count > 0)
		{
			turnTeam.Peek().BeginTurn();
		}
	}

	public static void EndTurn()
	{
		ActionScript unit = turnTeam.Dequeue();
		unit.EndTurn();

		if(turnTeam.Count > 0)
		{
			StartTurn();
		}
		else
		{
			string team = turnKey.Dequeue();
			turnKey.Enqueue(team);
			InitTeamTurnQueue();
		}
	}

	public static void AddUnit(ActionScript unit)
	{
		List<ActionScript> list;
		if(!units.ContainsKey(unit.tag))
		{
			list = new List<ActionScript>();
			units[unit.tag] = list;

			if(!turnKey.Contains(unit.tag))
            {
                turnKey.Enqueue(unit.tag);
			}
		}
		else
		{
			list = units[unit.tag];
		}

		list.Add(unit);
	}
}
