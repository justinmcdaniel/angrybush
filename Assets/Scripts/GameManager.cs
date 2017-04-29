using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public List<GameObject> plants;

	public static GameManager instance = null;
	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != null) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void gotoDebugGrid() {
		SceneManager.LoadScene ("Grid");
	}

	public void gotoDebugCombat() {
		SceneManager.LoadScene ("Combat");
	}
}
