using UnityEngine;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public System.TimeSpan turnTime = new TimeSpan(0,0,5);

	GameObject[] pollutions;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != null) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);

		pollutions = GameObject.FindGameObjectsWithTag ("Pollution");

		InitGame ();
	}

	void InitGame() {
		// load plants
		// load pollutions
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Combat() {
		foreach (GameObject pollution in pollutions) {
			
		}
	}

	void PlayerMove() {
		bool end = false;
		DateTime start = System.DateTime.Now;
		do {
			if (System.DateTime.Now - start >= turnTime) {
				end = true;
			}
			// if (Mouse changes from down to up)
			// end = true;

		} while (end);
	}

	void AIMove() {

	}

	bool PlayerWin () {
		return false;
	}

	bool PlayerLose () {
		return false;
	}
}
