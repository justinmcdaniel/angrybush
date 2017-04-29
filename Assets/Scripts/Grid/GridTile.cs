using UnityEngine;
using System.Collections;

public class GridTile : MonoBehaviour {

	public bool isCurrentTile;

	public int gridX;
	public int gridY;

	public MouseGridController mouseController;

	public GameObject character = null;

	// Use this for initialization
	void Awake () {
		mouseController = (MouseGridController)GameObject.Find("Main Camera").GetComponent (typeof(MouseGridController));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseEnter() {
		isCurrentTile = true;
		mouseController.mouseEnter (this.gridX, this.gridY);
	}
	void OnMouseExit() {
		isCurrentTile = false;
		mouseController.mouseExit ();
	}

	//Move to character
	void OnMouseUp() {
		mouseController.mouseUp ();
	}

	void OnMouseDown() {
		mouseController.mouseDown ();
	}
}
