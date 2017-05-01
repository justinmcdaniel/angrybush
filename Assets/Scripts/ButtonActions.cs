using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanges : MonoBehaviour {

	public void gotoIntroScene() {
		SceneManager.LoadScene ("Intro");
	}

	public void gotoMenuScene() {
		SceneManager.LoadScene ("LevelSelection");
	}

	public void gotoLevel1() {
		SceneManager.LoadScene ("Level1");
	}
}
