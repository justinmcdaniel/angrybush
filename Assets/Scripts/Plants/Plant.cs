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

	public float flashSpeed = 5f;
	public Color flashColor = new Color (1f, 0f, 0f, 0.1f);

	Animator anim;
	//AudioSource playerAudio;

	public bool isDead;
	bool isDamaged;

	GameObject globalStats;
	Stats stats;

	Skill skill;

	Image healthBar;
	Image healthBarBackground;

	public GameObject popupText;

	// Use this for initialization
	void Awake () {
		globalStats = GameObject.Find ("globalStats");
		stats = (Stats)globalStats.GetComponent (typeof(Stats));

		currentHealth = health;

		skill = ((Skill)gameObject.GetComponent (typeof(Skill)));

		healthBar = transform.FindChild ("PollutionCanvas").FindChild ("HealthBar").FindChild ("Health").GetComponent<Image> ();
		healthBarBackground = transform.FindChild ("PollutionCanvas").FindChild ("HealthBar").GetComponent<Image> ();
		healthBar.enabled = false;
		healthBarBackground.enabled = false;
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
	private Vector3 lerpFromPosition;
	private Vector3 lerpToPosition;
	void Update() {
		if (lerpFromPosition != lerpToPosition) {
			gameObject.transform.position = Vector3.Lerp (lerpFromPosition, lerpToPosition, flashSpeed * Time.deltaTime);
		} 
	}

	public void LerpTo(Vector3 lerpToPosition) {
		if (lerpToPosition != lerpFromPosition) {
			gameObject.transform.position = lerpToPosition;
		}
		lerpFromPosition = gameObject.transform.position;
		lerpToPosition = lerpToPosition;
	}

	void InitPopupText (string damage) {
		GameObject temp = Instantiate (popupText) as GameObject;
		temp.SetActive (true);
		RectTransform tempRect = temp.GetComponent<RectTransform> ();
		temp.transform.SetParent (transform.FindChild ("PlantCanvas"));
		tempRect.transform.localPosition = popupText.transform.localPosition;
		tempRect.transform.localScale = popupText.transform.localScale;

		((Text) temp.GetComponent(typeof(Text))).text = damage;
		((Animator)temp.GetComponent (typeof(Animator))).SetTrigger ("Damage");
		Destroy (temp.gameObject, 2);
	}

	public int DoDamage () {

		int damage = 0;
		if (skill.damageType == Enumerations.DamageType.Magic) {
			damage = skill.damage * (magic / stats.baseMagic);
		}
		else if (skill.damageType == Enumerations.DamageType.Physical) {
			damage = skill.damage * (strength / stats.baseStrength);
		}
		Debug.Log (damage);
		return damage;
	}

	public Enumerations.DamageType DoDamageType () {
		return skill.damageType;
	}

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

		InitPopupText (damage.ToString());

		healthBar.fillAmount = (float)currentHealth / (float)health;
		healthBar.enabled = true;
		healthBarBackground.enabled = true;

		//healthSlider.value = currentHealth;

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
