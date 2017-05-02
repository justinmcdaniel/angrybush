using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
	
	public System.TimeSpan turnTime = new TimeSpan(0,0,5);
	public Stage[] stages;
	int currentStageIndex = 0;
	public Stage currentStage;
	public int turnLength_Seconds = 3;

	public Dictionary <string, int> plantPositions;
	GameManager gameManager;
	MouseGridController mouseGridController;
		
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
		mouseGridController = (MouseGridController)gameObject.GetComponent (typeof(MouseGridController));

		foreach (GameObject canvasObjects in GameObject.FindGameObjectsWithTag("Finish")) {
			canvasObjects.GetComponent<Canvas> ().worldCamera = Camera.main;
		}

		//Assumes gameObject is "Main Camera"
		//stages = gameObject.GetComponents<Stage>();
		currentStage = stages [currentStageIndex];


		plantPositions = new Dictionary<string, int> ();
		foreach (Position plantPos in plantStartingPositions) {
			GameObject targetGrid = GameObject.Find ("grid_tile_" + plantPos.key);
			Debug.Log ("grid_tile_" + plantPos.key);
			GameObject plant = gameManager.plants [plantPos.value];
			Plant plantScr = (Plant)plant.GetComponent (typeof(Plant));
			plant.SetActive (true);
			plant.transform.position = targetGrid.transform.position;
			plantPositions.Add (plantPos.key, plantPos.value);
			plantScr.x = int.Parse (plantPos.key [0].ToString ());
			plantScr.y = int.Parse (plantPos.key [1].ToString ());
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
		if (isPlayerTurn && playerHasStartedTurn) {
			if (DateTime.Now > turnStartTime.AddSeconds (turnLength_Seconds)) {
				playerMoveEnd (true);
			}
		}
	}

	private bool isPlayerTurn = true;
	private bool playerHasStartedTurn = false;
	public bool IsPlayerTurn { get { return isPlayerTurn; } }
	private DateTime turnStartTime;

	public void startPlayerTurn() {
		isPlayerTurn = true;
		playerHasStartedTurn = false;
	}

	public void playerMoveStart() {
		playerHasStartedTurn = true;
		turnStartTime = DateTime.Now;
	}

	public void playerMoveEnd(bool timedOut) {
		Debug.Log ("Player turn end.");
		if (timedOut) {
			mouseGridController.mouseUp();
		}
		isPlayerTurn = false;
		PlayerCombat ();
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
			Plant plantScr = (Plant)plant.GetComponent (typeof(Plant));
			GameObject toGridTile = GameObject.Find ("grid_tile_" + toKey);
			//plant.transform.position = GameObject.Find ("grid_tile_" + toKey).transform.position;
			plantScr.LerpTo (toGridTile.transform.position.x, toGridTile.transform.position.y);
			plantScr.x = toGridX;
			plantScr.y = toGridY;
		}
	}

	public void movePollutionToPosition_Freely(GameObject pollution, int fromGridX, int fromGridY, int toGridX, int toGridY) {
		string fromKey = fromGridX.ToString () + fromGridY.ToString ();
		string toKey = toGridX.ToString () + toGridY.ToString ();
		string[] keysDebug = new string[currentStage.pollutions.Keys.Count];
		currentStage.pollutions.Keys.CopyTo (keysDebug, 0);
		if (currentStage.pollutions.ContainsKey(fromKey)) {
			currentStage.pollutions.Remove (fromKey);
			currentStage.pollutions.Add (toKey, pollution);
			Pollution pollutionScr = (Pollution)pollution.GetComponent (typeof(Pollution));
			GameObject toGridTile = GameObject.Find ("grid_tile_" + toKey);
			//pollution.transform.position = GameObject.Find ("grid_tile_" + toKey).transform.position;
			pollutionScr.LerpTo (toGridTile.transform.position.x, toGridTile.transform.position.y);
			pollutionScr.x = toGridX;
			pollutionScr.y = toGridY;
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
			Plant plantScr = (Plant)plant.GetComponent (typeof(Plant));
			plantScr.LerpTo (toGridTile.transform.position.x, toGridTile.transform.position.y);
			//plant.transform.position = toGridTile.transform.position;
			plantScr.x = toGridX;
			plantScr.y = toGridY;
			Plant toCharScr = (Plant)toCharacter.GetComponent (typeof(Plant));
			//toCharacter.transform.position = fromGridTile.transform.position;
			toCharScr.LerpTo (fromGridTile.transform.position.x, fromGridTile.transform.position.y);
			toCharScr.x = fromGridX;
			toCharScr.y = fromGridY;

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
		} else if (sourceCharacter.tag == targetCharacter.tag) {
			return Enumerations.MoveType.Swap;
		} else {
			return Enumerations.MoveType.Blocked;
		}
	}

	public void PlayerCombat() {
		string[] keysDebug = new string[this.currentStage.pollutions.Keys.Count];
		this.currentStage.pollutions.Keys.CopyTo (keysDebug, 0);
		KeyValuePair<string, GameObject> pollutionObject;
		foreach (string key in keysDebug) {
			pollutionObject = new KeyValuePair<string,GameObject> (key, currentStage.pollutions [key]);
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

			if (pollution.isDead) {
				currentStage.pollutions.Remove (pollutionObject.Key);
				GameObject.DestroyObject(pollutionObject.Value);

				if (currentStage.pollutions.Count == 0) {
					Debug.Log ("done");
					currentStage.enabled = false;
					currentStageIndex += 1;
					if (currentStageIndex >= stages.Length) {
						levelWin ();
					} else {
						currentStage = stages [currentStageIndex];
					}
					break;
				}
			}
		}
		AIMove ();
	}

	public void AiCombat() {
		string[] keysDebugPlant = new string[this.plantPositions.Keys.Count];
		this.plantPositions.Keys.CopyTo (keysDebugPlant, 0);
		KeyValuePair<string, int> plantPosition;
		foreach (string keyPlant in keysDebugPlant) {
			plantPosition = new KeyValuePair<string,int> (keyPlant, plantPositions [keyPlant]);
			int damage = 0;
			Plant plant = ((Plant) plants[plantPosition.Value].GetComponent(typeof(Plant)));

			int gridX = plant.x; 
			int gridY = plant.y;
			int x;
			int y;

			// Left
			GameObject leftCharacter = null;
			x = -1;
			y = 0;
			do {
				leftCharacter = getCharacterAtGridPosition(gridX+x, gridY+y);
				x--;
			} while (leftCharacter != null && leftCharacter.tag == "Plant" && (gridX+x) >= 0);

			// Right
			if (leftCharacter != null && leftCharacter.tag == "Pollution") {
				GameObject rightCharacter = null;
				x = 1;
				y = 0;
				do {
					rightCharacter = getCharacterAtGridPosition(gridX+x, gridY+y);
					if (rightCharacter != null) {
					}
					x++;
				} while (rightCharacter != null && rightCharacter.tag == "Plant" && (gridX+x) <= 7);

				if (rightCharacter != null && rightCharacter.tag == "Pollution") {
					Pollution pollutionRight = ((Pollution)rightCharacter.GetComponent (typeof(Pollution)));
					Debug.Log ("Damage: Right");
					plant.TakeDamage(pollutionRight.DoDamage (), pollutionRight.DoDamageType());
					Pollution pollutionLeft = ((Pollution)leftCharacter.GetComponent (typeof(Pollution)));
					Debug.Log ("Damage: Left");
					plant.TakeDamage(pollutionLeft.DoDamage (), pollutionLeft.DoDamageType());
				}
			}

			// Up
			GameObject topCharacter = null;
			x = 0;
			y = -1;
			do {
				topCharacter = getCharacterAtGridPosition(gridX+x, gridY+y);
				y--;
			} while (topCharacter != null && topCharacter.tag == "Plant" && (gridY+y) >= 0);

			// Down
			if (topCharacter != null && topCharacter.tag == "Pollution") {
				GameObject bottomCharacter = null;
				x = 0;
				y = 1;
				do {
					bottomCharacter = getCharacterAtGridPosition(gridX+x, gridY+y);
					y++;
				} while (bottomCharacter != null && bottomCharacter.tag == "Plant" && (gridX+y) <= 7);

				if (bottomCharacter != null && bottomCharacter.tag == "Pollution") {
					Pollution pollutionTop = ((Pollution)topCharacter.GetComponent (typeof(Pollution)));
					plant.TakeDamage(pollutionTop.DoDamage (),pollutionTop.DoDamageType());
					Pollution pollutionBottom = ((Pollution)bottomCharacter.GetComponent (typeof(Pollution)));
					plant.TakeDamage(pollutionBottom.DoDamage (), pollutionBottom.DoDamageType());
				}
			}

			if (plant.isDead) {
				plantPositions.Remove (plantPosition.Key);

				if (plantPositions.Count == 0) {
					levelLose ();
					break;
				}
			}
		}

		startPlayerTurn ();
		//System.Threading.Thread.Sleep (1000);
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

	int calcDistance (int x1, int y1, int x2, int y2) {
		return Math.Abs (x1 - x2) + Math.Abs (y1 - y2);
	}

	void AIMove() {
		Debug.Log ("AI Move Initiated");
		string[] keysDebug = new string[this.currentStage.pollutions.Keys.Count];
		this.currentStage.pollutions.Keys.CopyTo (keysDebug, 0);
		KeyValuePair<string, GameObject> pollutionObject;
		foreach (string key in keysDebug) {
			pollutionObject = new KeyValuePair<string,GameObject> (key, currentStage.pollutions [key]);
			string bestPosition = "";
			string backupPosition = "";
			string[] keysDebugPlant = new string[this.plantPositions.Keys.Count];
			this.plantPositions.Keys.CopyTo (keysDebugPlant, 0);
			KeyValuePair<string, int> plantLocation;
			foreach (string keyPlant in keysDebugPlant) {
				plantLocation = new KeyValuePair<string,int> (keyPlant, plantPositions [keyPlant]);
				int plantX = int.Parse(plantLocation.Key [0].ToString());
				int plantY = int.Parse(plantLocation.Key [1].ToString());

				// get Left and Right Characters
				if ((plantX - 1) >= 0 && (plantX + 1) <= 7) {
					GameObject leftChar = getCharacterAtGridPosition (plantX - 1, plantY);
					GameObject rightChar = getCharacterAtGridPosition (plantX + 1, plantY);

					if (leftChar == null) {
						if (backupPosition == "") {
							backupPosition = (plantX - 1).ToString () + plantY.ToString ();
						} else if (
							calcDistance(
							int.Parse(pollutionObject.Key[0].ToString()), 
							int.Parse(pollutionObject.Key[1].ToString()), 
							int.Parse(backupPosition[0].ToString()), 
							int.Parse(backupPosition[1].ToString())) < 
							calcDistance(
							int.Parse(pollutionObject.Key[0].ToString()), 
							int.Parse(pollutionObject.Key[1].ToString()),
							plantX - 1,
							plantY))
						{
							backupPosition = (plantX - 1).ToString () + plantY.ToString ();
						}

						// Look to Right
						if (rightChar != null && rightChar.tag == "Pollution" && rightChar.name != pollutionObject.Value.name) {
							bestPosition = (plantX - 1).ToString () + plantY.ToString ();
							break;
						} else if (rightChar != null && rightChar.tag == "Plant") {
							int i = 2;
							bool done = false;
							do {
								if ((plantX + i) <= 7) {
									rightChar = getCharacterAtGridPosition (plantX + i, plantY);

									if (rightChar != null && rightChar.tag == "Pollution" && rightChar.name != pollutionObject.Value.name) {
										bestPosition = (plantX - 1).ToString () + plantY.ToString ();
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
						if (backupPosition == "") {
							backupPosition = (plantX + 1).ToString () + plantY.ToString ();
						} else if (
							calcDistance(
								int.Parse(pollutionObject.Key[0].ToString()), 
								int.Parse(pollutionObject.Key[1].ToString()), 
								int.Parse(backupPosition[0].ToString()), 
								int.Parse(backupPosition[1].ToString())) < 
							calcDistance(
								int.Parse(pollutionObject.Key[0].ToString()), 
								int.Parse(pollutionObject.Key[1].ToString()),
								plantX + 1,
								plantY))
						{
							backupPosition = (plantX + 1).ToString () + plantY.ToString ();
						}

						// Look to Left
						if (leftChar != null && leftChar.tag == "Pollution" && leftChar.name != pollutionObject.Value.name) {
							bestPosition = (plantX + 1).ToString () + plantY.ToString ();
							break;
						} else if (leftChar != null && leftChar.tag == "Plant") {
							int i = 2;
							bool done = false;
							do {
								if ((plantX - i) >= 0) {
									leftChar = getCharacterAtGridPosition (plantX - i, plantY);

									if (leftChar != null && leftChar.tag == "Pollution" && leftChar.name != pollutionObject.Value.name) {
										bestPosition = (plantX + 1).ToString () + plantY.ToString ();
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
						if (backupPosition == "") {
							backupPosition = plantX.ToString () + (plantY - 1).ToString ();
						} else if (
							calcDistance(
								int.Parse(pollutionObject.Key[0].ToString()), 
								int.Parse(pollutionObject.Key[1].ToString()), 
								int.Parse(backupPosition[0].ToString()), 
								int.Parse(backupPosition[1].ToString())) < 
							calcDistance(
								int.Parse(pollutionObject.Key[0].ToString()), 
								int.Parse(pollutionObject.Key[1].ToString()),
								plantX,
								plantY - 1))
						{
							backupPosition = plantX.ToString () + (plantY - 1).ToString ();
						}

						// Look to Down
						if (bottomChar != null && bottomChar.tag == "Pollution" && bottomChar.name != pollutionObject.Value.name) {
							bestPosition = plantX.ToString () + (plantY - 1).ToString ();
							break;
						} else if (bottomChar != null && bottomChar.tag == "Plant") {
							int i = 2;
							bool done = false;
							do {
								if ((plantY + i) <= 5) {
									bottomChar = getCharacterAtGridPosition (plantX, plantY + 1);

									if (bottomChar != null && bottomChar.tag == "Pollution" && bottomChar.name != pollutionObject.Value.name) {
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
						if (backupPosition == "") {
							backupPosition = plantX.ToString () + (plantY + 1).ToString ();
						} else if (
							calcDistance(
								int.Parse(pollutionObject.Key[0].ToString()), 
								int.Parse(pollutionObject.Key[1].ToString()), 
								int.Parse(backupPosition[0].ToString()), 
								int.Parse(backupPosition[1].ToString())) < 
							calcDistance(
								int.Parse(pollutionObject.Key[0].ToString()), 
								int.Parse(pollutionObject.Key[1].ToString()),
								plantX,
								plantY + 1))
						{
							backupPosition = plantX.ToString () + (plantY + 1).ToString ();
						}

						// Look to Up
						if (topChar != null && topChar.tag == "Pollution" && topChar.name != pollutionObject.Value.name) {
							bestPosition = plantX.ToString () + (plantY + 1).ToString ();
							break;
						} else if (topChar != null && topChar.tag == "Plant") {
							int i = 2;
							bool done = false;
							do {
								if ((plantY - i) >= 0) {
									topChar = getCharacterAtGridPosition (plantX, plantY - 1);

									if (topChar != null && topChar.tag == "Pollution" && topChar.name != pollutionObject.Value.name) {
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
			if (bestPosition == "") {
				if (backupPosition == "") {
					bestPosition = pollutionObject.Key;
				} else {
					bestPosition = backupPosition;
				}
			}

			movePollutionToPosition_Freely (pollutionObject.Value, int.Parse (pollutionObject.Key [0].ToString ()), int.Parse (pollutionObject.Key [1].ToString ()), int.Parse (bestPosition [0].ToString ()), int.Parse (bestPosition [1].ToString ()));
		}
		AiCombat ();
	}

	void levelWin () {
		foreach (GameObject plantObject in gameManager.plants) {
			plantObject.SetActive (false);
		}
		SceneManager.LoadScene ("LevelWin");
		Debug.Log ("Win");
	}

	void levelLose () {
		SceneManager.LoadScene("LevelLose");
	}

	void turn () {
		StartCoroutine (seqTurn());
	}

	IEnumerator seqTurn() {
		PlayerCombat ();
		yield return new WaitForSeconds(2F);

		AIMove ();
		yield return new WaitForSeconds(1F);

		AiCombat ();
		yield return new WaitForSeconds(2F);
	}
}
