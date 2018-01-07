using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour {

	public GameObject opponent;
	public AnimationClip attack;
	public AnimationClip die;
	private Animation anim;
	public static bool isAttacking = false;
	public static bool isDead = false;
	public int damage = 10;
	public float impactTime = 0.359f; // based on attacking animation percent (Frame 12)
	private bool impacted = false;
	public float range = 1.25f;
	public int maxHealth = 300;
	public int health;

	private bool isDieAnimStarted = false;
	private bool isDieAnimEnded = false;

	public float combatEscapeTime = 10f;
	public float countDown;

	public bool isSpecialAttackOn = false;

	// Use this for initialization
	void Start () 
	{
		health = maxHealth;
		anim = GetComponent<Animation>();
	}

	// Update is called once per frame
	void Update ()
	{
		if(!isSpecialAttackOn)
			AttackUpdate (0, 1f, KeyCode.Space);

		if (IsDead () && !isDieAnimEnded)
			Die ();
	}

	public void AttackUpdate(int stunSeconds, float scaleDamage, KeyCode key)
	{
		if (Input.GetKey(key) && opponent != null
			&& !IsDead() && InRange())
		{
			anim.CrossFade (attack.name);
			isAttacking = true;	
			transform.LookAt (opponent.transform.position);
		}

		// if attacking animation is almost ended
		if (anim[attack.name].time > 0.9 * anim[attack.name].length)
		{
			isAttacking = false;
			impacted = false;

			if (isSpecialAttackOn) 
				isSpecialAttackOn = false;
		}

		Impact(stunSeconds, scaleDamage);
	}

	public void ResetAttackUpdate()
	{
		isAttacking = false;

		// to make the inner code be called once
		if (!isSpecialAttackOn) 
		{
			impacted = false;
			anim.Stop (attack.name);
		}
	}

	void Impact(int stunSeconds, float scaleDamage)
	{
		if ((opponent != null) && anim.IsPlaying (attack.name) && impacted == false)
		{
			// to make opponent get hit once per one attacking action not per every attacking action frame
			// if the time of the current playing attacking animation is over the time of the attacking animation's impact frame
			if (anim [attack.name].time > anim [attack.name].length * impactTime
			  && anim[attack.name].time < 0.9 * anim[attack.name].length)
			{
				countDown = combatEscapeTime;
				CancelInvoke ("CombatCountDown"); // if there are InvokeRepeating with a function(method) name "CombatCountDown", cancel it
				InvokeRepeating ("CombatCountDown", 1, 1);  // parameter : function(method) name,
															//            delay time before start (0 : start immediately, 1 : start after 1 second to prevent count down start at (combatEscapeTime-1)),
															//            how frequently invoke (1 : per a second)
				opponent.GetComponent<Mob>().GetHit((float)damage*scaleDamage);
				opponent.GetComponent<Mob>().GetStun(stunSeconds);
				impacted = true;
			}
		}
	}

	void CombatCountDown()
	{
		--countDown;

		if (countDown == 0)
			CancelInvoke ("CombatCountDown"); // stop InvokeRepeating with a function(method) name "CombatCountDown"
	}

	bool InRange()
	{
		if (Vector3.Distance (transform.position, opponent.transform.position) <= range)
			return true;
		else
			return false;
	}

	public void GetHit(int damage)
	{
		health -= damage;

		if (health < 0)
			health = 0;
	}

	public bool IsDead()
	{
		if (health <= 0)
			isDead = true;
		else
			isDead = false;

		return isDead;
	}

	void Die()
	{
		if (!isDieAnimStarted) 
		{
			anim.Play (die.name);
			isDieAnimStarted = true;
		}

		if (isDieAnimStarted && !anim.IsPlaying (die.name))
		{
			isDieAnimEnded = true;
		}
	}
}
