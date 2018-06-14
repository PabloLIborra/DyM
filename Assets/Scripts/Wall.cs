using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;
using System;

public class Wall : MonoBehaviour {

	//Side to which this object is oriented
	public int side;
	//being: 1 where it is right now, an clockwise 2-3-4

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		//Camera position
		GameObject ar = GameObject.FindGameObjectWithTag("MainCamera");
		Vector3 pos = ar.transform.position;

		if(side == 1)
		{
			if (pos.z > 0) {
				gameObject.GetComponent<MeshRenderer> ().enabled = true;
			} else {
				gameObject.GetComponent<MeshRenderer> ().enabled = false;
			}
		}

		if(side == 2)
		{
			if (pos.x > 0) {
				gameObject.GetComponent<MeshRenderer> ().enabled = true;
			} else {
				gameObject.GetComponent<MeshRenderer> ().enabled = false;
			}
		}

		if(side == 3)
		{
			if (pos.z <= 0) {
				gameObject.GetComponent<MeshRenderer> ().enabled = true;
			} else {
				gameObject.GetComponent<MeshRenderer> ().enabled = false;
			}
		}

		if(side == 4)
		{
			if (pos.x <= 0) {
				gameObject.GetComponent<MeshRenderer> ().enabled = true;
			} else {
				gameObject.GetComponent<MeshRenderer> ().enabled = false;
			}
		}


	}
}
