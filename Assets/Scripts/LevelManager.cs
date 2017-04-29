using UnityEngine;
using System;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {
	
	public System.TimeSpan turnTime = new TimeSpan(0,0,5);
	public Stage[] stages;
	public Stage currentStage;

	public Dictionary <string, int> plantPositions;

	[Serializable]
	public struct Position {
		public string key;
		public int value;
	}

	public List<Position> plantStartingPositions;

	List<GameObject> plants;

	// Use this for initialization
	void Awake () {

		plantPositions = new Dictionary<string, int> () { };

		foreach (Position position in plantStartingPositions) {
			plantPositions [position.key] = position.value;
		}

		InitLevel ();

		//Assumes gameObject is "Main Camera"
		stages = (Stage[])gameObject.GetComponents(typeof(Stage));
		foreach (Stage stage in stages) {
			if (stage.activeStage) {
				currentStage = stage;
				break;
			}
		}

		plants = ((GameManager)GameObject.Find ("GameManager").GetComponent (typeof(GameManager))).plants;
	}

	void InitLevel() {
		// load plants
		// load pollutions


	}
	
	// Update is called once per frame
	void Update () {

	}

	void getAdjacentCharacter(string position) {
		if (currentStage.pollutions.ContainsKey (position)) {

		} else {

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
				GameObject leftGrid = GameObject.Find ("grid_tile_" + Convert.ToString (gridX+x) + Convert.ToString (gridY+y));
				if (((GridTile)leftGrid.GetComponent (typeof(GridTile))).character) {
					leftCharacter = ((GridTile)leftGrid.GetComponent (typeof(GridTile))).character;
				}
				x--;
			} while (leftCharacter.tag == "Pollution" && (gridX+x) >= 0);

			// Right
			if (leftCharacter.tag == "Plant") {
				GameObject rightCharacter = null;
				x = 1;
				y = 0;
				do {
					GameObject rightGrid = GameObject.Find ("grid_tile_" + Convert.ToString (gridX+x) + Convert.ToString (gridY+y));
					if (((GridTile)rightGrid.GetComponent (typeof(GridTile))).character) {
						rightCharacter = ((GridTile)rightGrid.GetComponent (typeof(GridTile))).character;
					}
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
				GameObject topGrid = GameObject.Find ("grid_tile_" + Convert.ToString (gridX+x) + Convert.ToString (gridY+y));
				if (((GridTile)topGrid.GetComponent (typeof(GridTile))).character) {
					topCharacter = ((GridTile)topGrid.GetComponent (typeof(GridTile))).character;
				}
				y--;
			} while (topCharacter.tag == "Pollution" && (gridY+y) >= 0);

			// Down
			if (topCharacter.tag == "Plant") {
				GameObject bottomCharacter = null;
				x = 0;
				y = 1;
				do {
					GameObject bottomGrid = GameObject.Find ("grid_tile_" + Convert.ToString (gridX+x) + Convert.ToString (gridY+y));
					if (((GridTile)bottomGrid.GetComponent (typeof(GridTile))).character) {
						bottomCharacter = ((GridTile)bottomGrid.GetComponent (typeof(GridTile))).character;
					}
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
