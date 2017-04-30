using UnityEngine;
using System.Collections;

public class MouseGridController : MonoBehaviour {

	public int GridX { get { return this.gridX; } }
	public int GridY { get { return this.gridY; } }
	public bool MouseIsDown { get { return this.MouseIsDown; } }
	public bool isBlocked = false;

	private int gridX;
	private int gridY;
	private int plantHoldGridX;
	private int plantHoldGridY;
	private bool mouseIsDown;
	private GameObject currentCharacter;
	private LevelManager levelManager;
	private GameManager gameManager;
	private GameObject mainCamera;

	// Use this for initialization
	void Awake () {
		mainCamera = gameObject;
		levelManager = (LevelManager)mainCamera.GetComponent (typeof(LevelManager));
		gameManager = (GameManager)GameObject.Find("GameManager").GetComponent (typeof(GameManager));	

		gridX = -1;
		gridY = -1;
		mouseIsDown = false;
		this.isBlocked = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void mouseEnter(int toGridX, int toGridY) {


		if (this.isBlocked) {
			if (this.plantHoldGridX == toGridX && this.plantHoldGridY == toGridY) {
				this.isBlocked = false;
			}
		} 

		if (!this.isBlocked) {	
			this.plantHoldGridX = this.gridX;
			this.plantHoldGridY = this.gridY;
		}

		this.gridX = toGridX;
		this.gridY = toGridY;

		GameObject targetGrid = GameObject.Find ("grid_tile_" + this.gridX.ToString() + this.gridY.ToString());

		if (this.mouseIsDown && this.currentCharacter != null && !this.isBlocked) {
			Enumerations.MoveType moveType = levelManager.getMoveTypeToGridPosition (this.gridX, this.gridY, this.plantHoldGridX, this.plantHoldGridY, this.currentCharacter);
			Debug.Log (moveType);
			if (Enumerations.MoveType.Free == moveType) {
				this.levelManager.movePlantToPosition_Freely (this.currentCharacter, this.plantHoldGridX, this.plantHoldGridY, this.gridX, this.gridY);
			} else if (Enumerations.MoveType.Swap == moveType) {
				this.levelManager.movePlantToPosition_Swap (this.currentCharacter, this.plantHoldGridX, this.plantHoldGridY, this.gridX, this.gridY);
			} else { 
				this.isBlocked = true;
			}
		}

	}

	public void mouseExit() {
		

		//todo: remember last good position when hitting walls.
	}

	public void mouseUp() {
		if (levelManager.IsPlayerTurn && this.mouseIsDown && this.currentCharacter != null) {
			this.mouseIsDown = false;
			this.currentCharacter = null;
			this.isBlocked = false;
			levelManager.playerMoveEnd (false);
		}
	}

	public void mouseDown() {
		if (levelManager.IsPlayerTurn) {
			levelManager.playerMoveStart ();
			this.mouseIsDown = true;
			this.currentCharacter = levelManager.getCharacterAtGridPosition (this.gridX, this.gridY);
			if (this.currentCharacter != null && this.currentCharacter.tag == "Pollution") {
				this.currentCharacter = null;
			} else {
				//no char here
			}
		}
	}
}
