using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneSelection : MonoBehaviour {

	public void gotoIntro() {
		SceneManager.LoadScene ("Intro");
	}

	public void gotoMenu() {
		SceneManager.LoadScene ("MainMenu");
	}

	public void gotoLevel1() {
		SceneManager.LoadScene ("Level1");
	}

	public void gotoCampaign() {
		SceneManager.LoadScene ("Campaign");
	}

	public void gotoLevel1Intro() {
		SceneManager.LoadScene ("Level1Intro");
	}
}
