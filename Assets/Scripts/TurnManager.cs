using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

	static Dictionary<string, List<MoveScript>> units = new Dictionary<string, List<MoveScript>>();
	static Queue<string> turnKey = new Queue<string>();
	static Queue<MoveScript> turnTeam = new Queue<MoveScript>();
	
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
		List<MoveScript> teamList = units[turnKey.Peek()];

		foreach(MoveScript unit in teamList)
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
		MoveScript unit = turnTeam.Dequeue();
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

	public static void AddUnit(MoveScript unit)
	{
		List<MoveScript> list;

		if(!units.ContainsKey(unit.tag))
		{
			list = new List<MoveScript>();
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
