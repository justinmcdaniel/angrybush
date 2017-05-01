using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneSelection : MonoBehaviour {

	public void gotoIntroScene() {
		SceneManager.LoadScene ("Intro");
	}

	public void gotoMenuScene() {
		SceneManager.LoadScene ("MainMenu");
	}

	public void gotoLevel1() {
		SceneManager.LoadScene ("Level1");
	}

	public void gotoCampaign() {
		SceneManager.LoadScene ("Campaign");
	}
}
