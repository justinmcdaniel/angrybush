using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour {

	public bool activeStage;


	//Dictionary<"XY", listIndex>
	public Dictionary <string, int> plantPositions;

	public GameObject pollution_1;
	public string pollution_1_position;
	public Dictionary <string, GameObject> pollutions;

	// Use this for initialization
	void Awake () {
		
		pollutions = new Dictionary<string, GameObject>() {
			{pollution_1_position, pollution_1}
		};
	}
}
