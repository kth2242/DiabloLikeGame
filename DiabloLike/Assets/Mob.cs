using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour {

	public float speed = 10f;
	public float range = 1.25f;
	private CharacterController controller;
	public Transform player;
	private Fighter opponent;
	public LevelSystem playerLevel;

	public AnimationClip run;
	public AnimationClip idle;
	public AnimationClip die;
	public AnimationClip attack;
	private Animation anim;

	public int maxHealth = 100;
	public int health;
	public int damage = 10;
	public int exp = 50;

	public float impactTime = 0.359f; // based on attacking animation percent (Frame 12)
	private bool impacted = false;
	private int stunTime;

	// Use this for initialization
	void Start () 
	{
		controller = GetComponent<CharacterController> ();	
		anim = GetComponent<Animation> ();
		opponent = player.GetComponent<Fighter> ();
		health = maxHealth;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!IsDead ())
		{
			if (stunTime <= 0) 
			{
				if (!InRange ())
					Chase ();
				else if (!opponent.IsDead ())
					Attack ();
				else
					anim.CrossFade (idle.name);
			}
		}
		else
		{
			anim.CrossFade (die.name);

			if (anim [die.name].time > 0.9 * anim [die.name].length)
			{
				playerLevel.exp += exp;
				Destroy (gameObject);
			}
		}
	}

	void Attack ()
	{
		anim.CrossFade (attack.name);

		// to make opponent get hit once per one attacking action not per every attacking action frame
		// if the time of the current playing attacking animation is over the time of the attacking animation's impact frame
		if (anim [attack.name].time > anim [attack.name].length * impactTime && !impacted
			&& anim[attack.name].time < 0.9 * anim[attack.name].length)
		{
			opponent.GetHit (damage);
			impacted = true;
		}

		// if attacking animation is almost ended
		if (anim[attack.name].time > 0.9 * anim[attack.name].length)
		{
			impacted = false;
		}
	}

	public bool InRange()
	{
		if (Vector3.Distance (transform.position, player.position) < range)
			return true;
		else
			return false;
	}

	public void GetHit(int damage)
	{
		// to show the health bar reduction
		opponent.opponent = gameObject;

		health -= damage;

		if (health < 0)
			health = 0;
	}

	public void GetStun(int seconds)
	{
		stunTime = seconds;
		CancelInvoke ("StunCountDown");
		InvokeRepeating ("StunCountDown", 1, 1);
	}

	void StunCountDown()
	{
		--stunTime;

		if (stunTime == 0)
			CancelInvoke ("StunCountDown");
	}

	void Chase()
	{
		transform.LookAt (player.position);
		controller.SimpleMove (transform.forward * speed);
		anim.CrossFade (run.name);
	}	

	bool IsDead()
	{
		if (health <= 0)
			return true;
		else
			return false;
	}

	void OnMouseOver()
	{
		//player.GetComponent<Fighter> ().opponent = gameObject;
		opponent.opponent = gameObject;
	}
}
