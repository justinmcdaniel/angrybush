using UnityEngine;
using System.Collections.Generic;

public class Stage : MonoBehaviour {

	public GameObject plant_1;
	public string plant_1_position;
	public GameObject plant_2;
	public string plant_2_position;
	public Dictionary <string, GameObject> plants;

	public GameObject pollution_1;
	public string pollution_1_position;
	public Dictionary <string, GameObject> pollutions;

	// Use this for initialization
	void Awake () {
		plants = new Dictionary<string, GameObject>() {
			{plant_1_position, plant_1},
			{plant_2_position, plant_2}
		};
		pollutions = new Dictionary<string, GameObject>() {
			{pollution_1_position, pollution_1}
		};
	}
}
