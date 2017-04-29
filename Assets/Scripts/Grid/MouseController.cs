using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {

	public int gridX;
	public int gridY;
	public bool mouseIsDown;
	public GameObject selectedCharacter;

	// Use this for initialization
	void Awake () {
		gridX = -1;
		gridY = -1;
		mouseIsDown = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void click(GameObject character) {
		
	}
}
