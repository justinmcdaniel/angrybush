using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pollution : MonoBehaviour {

	public int id;

	public int health;
	public int strength;
	public int defense;
	public int magic;
	public int magicDefense;

	public int x;
	public int y;

	public int currentHealth;
	public Slider healthSlider;
	public Image damageImage;
	//public AudioClip deathClip;
	public float flashSpeed = 5f;
	public Color flashColor = new Color (1f, 0f, 0f, 0.1f);

	Animator anim;
	//AudioSource playerAudio;

	bool isDead;
	bool isDamaged;

	GameObject globalStats;
	Stats stats;

	Skill skill;

	// Use this for initialization
	void Awake () {
		globalStats = GameObject.Find ("globalStats");
		stats = (Stats)globalStats.GetComponent (typeof(Stats));

		currentHealth = health;

		skill = ((Skill)gameObject.GetComponent (typeof(Skill)));
	}

	// Update is called once per frame
	/*
	void Update () {
		if (isDamaged) {
			//damageImage.color = flashColor;
		} else {
			//damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}
		isDamaged = false;
	}
	*/

	public void TakeDamage (int amount, Enumerations.DamageType damageType) {
		int damage = 0;
		if (damageType == Enumerations.DamageType.Magic) {
			damage = amount * (stats.baseMagicDefense / magicDefense);
		}
		else if (damageType == Enumerations.DamageType.Physical) {
			damage = amount * (stats.baseDefence / defense);
		}

		isDamaged = true;

		currentHealth -= damage;

		//healthSlider.value = currentHealth;

		//Play damage audio

		if (currentHealth <= 0 && !isDead) {
			Death ();
		}
	}

	void Death() {
		isDead = true;

		//anim.SetTrigger ("Die");

		//playerAudio.clip = deathClip;
		//playerAudio.Play ();
		Object.Destroy(gameObject);
		((LevelManager)GameObject.Find ("Main Camera").GetComponent (typeof(LevelManager))).currentStage.pollutions.Remove (x.ToString () + y.ToString ());
	}
}
