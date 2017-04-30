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
			GameObject targetGrid = GameObject.Find ("grid_tile_" + plantPos.key);
			GameObject plant = gameManager.plants [plantPos.value];
			plant.SetActive (true);
			plant.transform.position = targetGrid.transform.position;
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
		} else if (this.currentStage.pollutions != null && this.currentStage.pollutions.ContainsKey (targetKey)) {
			return this.currentStage.pollutions [targetKey];
		} else {
			return null;
		}
	}

	public void movePlantToPosition_Freely(GameObject plant, int fromGridX, int fromGridY, int toGridX, int toGridY) {
		string fromKey = fromGridX.ToString () + fromGridY.ToString ();
		string toKey = toGridX.ToString () + toGridY.ToString ();
		string[] keysDebug = new string[this.plantPositions.Keys.Count];
		this.plantPositions.Keys.CopyTo (keysDebug, 0);
		Debug.Log (String.Join (",", keysDebug));
		Debug.Log ("ToKey: " + toKey + ", FromKey: " + fromKey);
		if (this.plantPositions.ContainsKey(fromKey)) {
			Debug.Log ("contains the key");
			int plantIndex = this.plantPositions [fromKey];
			this.plantPositions.Remove (fromKey);
			this.plantPositions.Add (toKey, plantIndex);
			plant.transform.position = GameObject.Find ("grid_tile_" + toKey).transform.position;
		}
	}

	public void movePlantToPosition_Swap(GameObject plant, int fromGridX, int fromGridY, int toGridX, int toGridY) {
		string fromKey = fromGridX.ToString () + fromGridY.ToString ();
		string toKey = toGridX.ToString () + toGridY.ToString ();
		GameObject fromGridTile = GameObject.Find ("grid_tile_" + fromKey);
		GameObject toGridTile = GameObject.Find ("grid_tile_" + toKey);
		GameObject toCharacter = this.getCharacterAtGridPosition (toGridX, toGridY);
		if (this.plantPositions.ContainsKey (fromKey) && this.plantPositions.ContainsKey (toKey)) {
			int plantIndex_From = this.plantPositions [fromKey];
			int plantIndex_To = this.plantPositions [toKey];
			this.plantPositions.Remove (fromKey);
			this.plantPositions.Remove (toKey);
			this.plantPositions.Add (toKey, plantIndex_From);
			this.plantPositions.Add (fromKey, plantIndex_To);
			toCharacter.transform.position = fromGridTile.transform.position;
			plant.transform.position = toGridTile.transform.position;
		} else {
			throw new Exception ("Swapping plants error: fromKey or toKey does not exist in dictionary");
		}
	}



	public Enumerations.MoveType getMoveTypeToGridPosition (int targetGridX, int targetGridY, int sourceGridX, int sourceGridY, GameObject sourceCharacter) {
		GameObject targetCharacter = this.getCharacterAtGridPosition (targetGridX, targetGridY);
		if (targetGridX == sourceGridX && targetGridY == sourceGridY) {
			return Enumerations.MoveType.None;
		} else if (targetCharacter == null) {
			return Enumerations.MoveType.Free;
		} else if (targetCharacter.tag == targetCharacter.tag) {
			return Enumerations.MoveType.Swap;
		} else {
			return Enumerations.MoveType.Blocked;
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
			} while (leftCharacter != null && leftCharacter.tag == "Pollution" && (gridX+x) >= 0);

			// Right
			if (leftCharacter != null && leftCharacter.tag == "Plant") {
				GameObject rightCharacter = null;
				x = 1;
				y = 0;
				do {
					rightCharacter = getCharacterAtGridPosition(gridX+x, gridY+y);
					if (rightCharacter != null) {
					}
					x++;
				} while (rightCharacter != null && rightCharacter.tag == "Pollution" && (gridX+x) <= 7);

				if (rightCharacter != null && rightCharacter.tag == "Plant") {
					Plant plantRight = ((Plant)rightCharacter.GetComponent (typeof(Plant)));
					pollution.TakeDamage(plantRight.DoDamage (), plantRight.DoDamageType());
					Plant plantLeft = ((Plant)leftCharacter.GetComponent (typeof(Plant)));
					Debug.Log ("Test");
					pollution.TakeDamage(plantLeft.DoDamage (), plantLeft.DoDamageType());
				}
			}
						
			// Up
			GameObject topCharacter = null;
			x = 0;
			y = -1;
			do {
				topCharacter = getCharacterAtGridPosition(gridX+x, gridY+y);
				y--;
			} while (topCharacter != null && topCharacter.tag == "Pollution" && (gridY+y) >= 0);

			// Down
			if (topCharacter != null && topCharacter.tag == "Plant") {
				GameObject bottomCharacter = null;
				x = 0;
				y = 1;
				do {
					bottomCharacter = getCharacterAtGridPosition(gridX+x, gridY+y);
					y++;
				} while (bottomCharacter != null && bottomCharacter.tag == "Pollution" && (gridX+y) <= 7);

				if (bottomCharacter != null && bottomCharacter.tag == "Plant") {
					Plant plantTop = ((Plant)topCharacter.GetComponent (typeof(Plant)));
					pollution.TakeDamage(plantTop.DoDamage (), plantTop.DoDamageType());
					Plant plantBottom = ((Plant)bottomCharacter.GetComponent (typeof(Plant)));
					pollution.TakeDamage(plantBottom.DoDamage (), plantBottom.DoDamageType());
				}
			}
		}

		AIMove ();
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
		Debug.Log ("AI Move Initiated");
		foreach (KeyValuePair<string, GameObject> pollutionObject in currentStage.pollutions) {
			Debug.Log ("Pollution: " + pollutionObject.Key);
			string bestPosition = "";
			foreach (KeyValuePair<string, int> plantLocation in plantPositions) {
				Debug.Log ("Plant: " + plantLocation.Key);
				int plantX = int.Parse(plantLocation.Key [0].ToString());
				int plantY = int.Parse(plantLocation.Key [1].ToString());

				// get Left and Right Characters
				if ((plantX - 1) >= 0 && (plantX + 1) <= 7) {
					GameObject leftChar = getCharacterAtGridPosition (plantX - 1, plantY);
					GameObject rightChar = getCharacterAtGridPosition (plantX + 1, plantY);

					if (leftChar == null) {
						// Look to Right
						if (rightChar != null && rightChar.tag == "Pollution") {
							bestPosition = (plantX + 1).ToString () + plantY.ToString ();
							break;
						} else if (rightChar != null && rightChar.tag == "Plant") {
							int i = 2;
							bool done = false;
							do {
								if ((plantX + i) <= 7) {
									rightChar = getCharacterAtGridPosition (plantX + i, plantY);

									if (rightChar != null && rightChar.tag == "Pollution") {
										bestPosition = (plantX + 1).ToString () + plantY.ToString ();
										break;
									} else if (rightChar == null) {
										done = true;
									}
									i++;
								} else {
									done = true;
								}
							} while (!done);
						}
					} else if (rightChar == null) {
						// Look to Left
						if (leftChar != null && leftChar.tag == "Pollution") {
							bestPosition = (plantX + 1).ToString () + plantY.ToString ();
							break;
						} else if (leftChar != null && leftChar.tag == "Plant") {
							int i = 2;
							bool done = false;
							do {
								if ((plantX - i) >= 0) {
									leftChar = getCharacterAtGridPosition (plantX - i, plantY);

									if (leftChar != null && leftChar.tag == "Pollution") {
										bestPosition = (plantX - 1).ToString () + plantY.ToString ();
										break;
									} else if (leftChar == null) {
										done = true;
									}
									i++;
								} else {
									done = true;
								}
							} while (!done);
						}
					}
				}

				// get Top and Bottom Characters
				if ((plantY - 1) >= 0 && (plantY + 1) <= 5) {
					GameObject topChar = getCharacterAtGridPosition (plantX, plantY - 1);
					GameObject bottomChar = getCharacterAtGridPosition (plantX, plantY + 1);

					if (topChar == null) {
						// Look to Down
						if (bottomChar != null && bottomChar.tag == "Pollution") {
							bestPosition = plantX.ToString () + (plantY - 1).ToString ();
							break;
						} else if (bottomChar != null && bottomChar.tag == "Plant") {
							int i = 2;
							bool done = false;
							do {
								if ((plantY + i) <= 5) {
									bottomChar = getCharacterAtGridPosition (plantX, plantY + 1);

									if (bottomChar != null && bottomChar.tag == "Pollution") {
										bestPosition = plantX.ToString () + (plantY - 1).ToString ();
										break;
									} else if (bottomChar == null) {
										done = true;
									}
									i++;
								} else {
									done = true;
								}
							} while (!done);
						}
					} else if (bottomChar == null) {
						// Look to Up
						if (topChar != null && topChar.tag == "Pollution") {
							bestPosition = plantX.ToString () + (plantY + 1).ToString ();
							break;
						} else if (topChar != null && topChar.tag == "Plant") {
							int i = 2;
							bool done = false;
							do {
								if ((plantY - i) >= 0) {
									topChar = getCharacterAtGridPosition (plantX, plantY - 1);

									if (topChar != null && topChar.tag == "Pollution") {
										bestPosition = plantX.ToString () + (plantY + 1).ToString ();
										break;
									} else if (topChar == null) {
										done = true;
									}
									i++;
								} else {
									done = true;
								}
							} while (!done);
						}
					}
				}
			}
			Debug.Log ("Best Position: " + bestPosition);
		}
	}

	bool PlayerWin () {
		return false;
	}

	bool PlayerLose () {
		return false;
	}
}
