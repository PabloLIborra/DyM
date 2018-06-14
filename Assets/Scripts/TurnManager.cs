using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

	static Dictionary<string, List<ActionScript>> units = new Dictionary<string, List<ActionScript>>();
	static Queue<string> turnKey = new Queue<string>();
	static Queue<ActionScript> turnTeam = new Queue<ActionScript>();
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(turnTeam.Count == 0)
		{
			InitTeamTurnQueue();
		}
	}

	static void InitTeamTurnQueue()
	{
		List<ActionScript> teamList = units[turnKey.Peek()];

		foreach(ActionScript unit in teamList)
		{
			turnTeam.Enqueue(unit);
		}

		StartTurn();
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
