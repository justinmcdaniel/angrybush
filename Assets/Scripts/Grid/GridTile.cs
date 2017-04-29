using UnityEngine;
using System.Collections;

public class GridTile : MonoBehaviour {

	public int gridX;
	public int gridY;

	public MouseController mouseController;

	public GameObject character = null;

	// Use this for initialization
	void Awake () {
		mouseController = (MouseController)GameObject.Find ("MouseController").GetComponent (typeof(MouseController));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseEnter() {
		mouseController.gridX = this.gridX;
		mouseController.gridY = this.gridY;
		Debug.Log ("X: " + this.gridX + ", Y: " + this.gridY);
	}
	void OnMouseExit() {
		mouseController.gridX = -1;
		mouseController.gridY = -1;
	}

	//Move to character
	void OnMouseUp() {
		mouseController.mouseIsDown = false;
	}

	void OnMouseDown() {
		mouseController.mouseIsDown = true;
	}
}
