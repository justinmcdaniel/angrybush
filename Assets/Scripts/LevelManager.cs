using UnityEngine;
using System;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {
	
	public System.TimeSpan turnTime = new TimeSpan(0,0,5);
	public Stage[] stages;
	public Stage currentStage;

	public Dictionary <string, int> plantPositions;
	GameManager gameManager;

	[Serializable]
	public struct Position {
		public string key;
		public int value;
	}

	public List<Position> plantStartingPositions;

	List<GameObject> plants { get { return gameManager.plants; } }

	// Use this for initialization
	void Awake () {

		gameManager = (GameManager)GameObject.Find ("GameManager").GetComponent (typeof(GameManager));

		//Assumes gameObject is "Main Camera"
		stages = gameObject.GetComponents<Stage>();
		foreach (Stage stage in stages) {
			if (stage.activeStage) {
				currentStage = stage;
				break;
			}
		}


		plantPositions = new Dictionary<string, int> ();
		foreach (Position plantPos in plantStartingPositions) {
			plantPositions.Add (plantPos.key, plantPos.value);
		}
	}

	void Start() {
		InitLevel ();
	}

	void InitLevel() {
		// load plants
		// load pollutions

	}
	
	// Update is called once per frame
	void Update () {

	}

	public GameObject getCharacterAtGridPosition (int gridX, int gridY) {
		string targetKey = gridX.ToString () + gridY.ToString ();
		if (this.plantPositions.ContainsKey (targetKey)) {
			int plantListPosition = this.plantPositions [targetKey];
			return gameManager.plants [plantListPosition];
		} else if (this.currentStage.pollutions.ContainsKey (targetKey)) {
			return this.currentStage.pollutions [targetKey];
		} else {
			return null;
		}

	}

	public void Combat() {
		Debug.Log ("Combat Initiated");
		foreach (KeyValuePair<string, GameObject> pollutionObject in currentStage.pollutions) {
			int damage = 0;
			Pollution pollution = ((Pollution) pollutionObject.Value.GetComponent (typeof(Pollution)));

			int gridX = pollution.x; 
			int gridY = pollution.y;
			int x;
			int y;

			// Left
			GameObject leftCharacter = null;
			x = -1;
			y = 0;
			do {
				leftCharacter = getCharacterAtGridPosition(gridX+x, gridY+y);
				x--;
			} while (leftCharacter.tag == "Pollution" && (gridX+x) >= 0);

			// Right
			if (leftCharacter.tag == "Plant") {
				GameObject rightCharacter = null;
				x = 1;
				y = 0;
				do {
					rightCharacter = getCharacterAtGridPosition(gridX+x, gridY+y);
					x++;
				} while (rightCharacter.tag == "Pollution" && (gridX+x) <= 7);

				if (rightCharacter.tag == "Plant") {
					Plant plantRight = ((Plant)rightCharacter.GetComponent (typeof(Plant)));
					pollution.TakeDamage(plantRight.DoDamage ());
					Plant plantLeft = ((Plant)leftCharacter.GetComponent (typeof(Plant)));
					pollution.TakeDamage(plantLeft.DoDamage ());
				}
			}
						
			// Up
			GameObject topCharacter = null;
			x = 0;
			y = -1;
			do {
				topCharacter = getCharacterAtGridPosition(gridX+x, gridY+y);
				y--;
			} while (topCharacter.tag == "Pollution" && (gridY+y) >= 0);

			// Down
			if (topCharacter.tag == "Plant") {
				GameObject bottomCharacter = null;
				x = 0;
				y = 1;
				do {
					bottomCharacter = getCharacterAtGridPosition(gridX+x, gridY+y);
					y++;
				} while (bottomCharacter.tag == "Pollution" && (gridX+y) <= 7);

				if (bottomCharacter.tag == "Plant") {
					Plant plantTop = ((Plant)topCharacter.GetComponent (typeof(Plant)));
					pollution.TakeDamage(plantTop.DoDamage ());
					Plant plantBottom = ((Plant)bottomCharacter.GetComponent (typeof(Plant)));
					pollution.TakeDamage(plantBottom.DoDamage ());
				}
			}
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
		//updateBoard ();
	}

	void moveCharacter(GameObject selectedCharacter, int newX, int newY) {
		//assumes selectedCharacter still has oldX, oldY
		//Dictionary<string, GameObject> localStage = stage.plants[newX.ToString() + newY.ToString()];
			
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
