using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour {

	public static Stats instance = null;

	public int baseHealth;
	public int baseStrength;
	public int baseDefence;
	public int baseMagic;
	public int baseMagicDefense;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != null) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}
}
