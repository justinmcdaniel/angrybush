using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Plant : MonoBehaviour {

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

	// Use this for initialization
	void Awake () {
		globalStats = GameObject.Find ("globalStats");
		stats = (Stats)globalStats.GetComponent (typeof(Stats));

		anim = GetComponent <Animator> ();
		//playerAudio = GetComponent <AudioSource> ();
		currentHealth = health / stats.baseHealth;
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

	public int DoDamage () {
		Skill skill = ((Skill)gameObject.GetComponent (typeof(Skill)));

		int damage = 0;
		if (skill.damageType == Enumerations.DamageType.Magic) {
			damage = skill.damage * (magic / stats.baseMagic);
		}
		else if (skill.damageType == Enumerations.DamageType.Physical) {
			damage = skill.damage * (strength / stats.baseStrength);
		}
		return damage;
	}

	public void TakeDamage (int amount, Skill skill) {
		int damage = 0;
		if (skill.damageType == Enumerations.DamageType.Magic) {
			damage = amount * (stats.baseMagicDefense / magicDefense);
		}
		else if (skill.damageType == Enumerations.DamageType.Physical) {
			damage = amount * (stats.baseDefence / defense);
		}

		isDamaged = true;

		currentHealth -= damage;

		healthSlider.value = currentHealth;

		//Play damage audio

		if (currentHealth <= 0 && !isDead) {
			Death ();
		}
	}

	void Death() {
		isDead = true;

		anim.SetTrigger ("Die");

		//playerAudio.clip = deathClip;
		//playerAudio.Play ();
	}
}
