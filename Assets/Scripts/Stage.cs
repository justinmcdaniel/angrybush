using UnityEngine;
using System;
using System.Collections.Generic;

public class Stage : MonoBehaviour {

	public bool activeStage;

	public bool stageComplete;

	public Dictionary <string, GameObject> pollutions;

	[Serializable]
	public struct PollutionAndPosition {
		public string key;
		public GameObject value;
	}

	public List<PollutionAndPosition> pollutionsAndPositions;

	// Use this for initialization
	void Awake () {
		stageComplete = false;
		
		pollutions = new Dictionary<string, GameObject> () { };

		foreach (PollutionAndPosition pollutionAndPosition in pollutionsAndPositions) {
			GameObject targetGrid = GameObject.Find ("grid_tile_" + pollutionAndPosition.key);
			pollutionAndPosition.value.SetActive (true);
			pollutionAndPosition.value.transform.position = targetGrid.transform.position;

			Pollution pollutionScript = (Pollution)pollutionAndPosition.value.GetComponent(typeof(Pollution));
			pollutionScript.x = int.Parse(pollutionAndPosition.key[0].ToString());
			pollutionScript.y = int.Parse(pollutionAndPosition.key [1].ToString());

			pollutions.Add(pollutionAndPosition.key, pollutionAndPosition.value);

		}
	}
}
