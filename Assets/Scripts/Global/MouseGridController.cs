using UnityEngine;
using System.Collections;

public class MouseGridController : MonoBehaviour {

	public int GridX { get { return this.gridX; } }
	public int GridY { get { return this.gridY; } }
	public bool MouseIsDown {get { return this.MouseIsDown; } }

	private int gridX;
	private int gridY;
	private int lastGoodGridX;
	private int lastGoodGridY;
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
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void mouseEnter(int gridX, int gridY) {
		this.gridX = gridX;
		this.gridY = gridY;
		Debug.Log ("X: " + this.gridX + ", Y: " + this.gridY);

		if (this.mouseIsDown && this.currentCharacter != null) {
			GameObject targetGrid = GameObject.Find ("grid_tile_" + this.gridX.ToString() + this.gridY.ToString());
			this.currentCharacter.transform.position = targetGrid.transform.position;

		}
	}

	public void mouseExit() {
		
		this.gridX = -1;
		this.gridY = -1;

		//todo: remember last good position when hitting walls.
	}

	public void mouseUp() {
		this.mouseIsDown = false;
		this.currentCharacter = null;
		levelManager.Combat ();
		//todo: call character drop code here
	}

	public void mouseDown() {
		this.mouseIsDown = true;
		this.currentCharacter = levelManager.getCharacterAtGridPosition (this.gridX, this.gridY);
		if (this.currentCharacter != null) {
			Debug.Log (((Skill)this.currentCharacter.GetComponent (typeof(Skill))).damage.ToString ());
		} else {
			Debug.Log ("no char here");
		}

		//todo: call character picked up here
	}
}
