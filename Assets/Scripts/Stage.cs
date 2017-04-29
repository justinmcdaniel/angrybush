using UnityEngine;
using System;
using System.Collections.Generic;

public class Stage : MonoBehaviour {

	public bool activeStage;

	public Dictionary <string, GameObject> pollutions;

	[Serializable]
	public struct PollutionAndPosition {
		public string key;
		public GameObject value;
	}

	public List<PollutionAndPosition> pollutionsAndPositions;

	// Use this for initialization
	void Awake () {
		
		pollutions = new Dictionary<string, GameObject> () { };

		foreach (PollutionAndPosition pollutionAndPosition in pollutionsAndPositions) {
			pollutions.Add(pollutionAndPosition.key, pollutionAndPosition.value);
		}
	}
}
