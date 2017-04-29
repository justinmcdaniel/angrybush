using UnityEngine;
using System.Collections;

public class CombatText : MonoBehaviour {

	public GUIText myGUIText;
	public int guiTime = 2;

	public void DisplayDamage(string damageMessage) {
		myGUIText.text = damageMessage;

		StartCoroutine (GUIDisplayTimer ());
	}

	IEnumerator GUIDisplayTimer() {
		yield return new WaitForSeconds (guiTime);

		Destroy (gameObject);
	}
}
