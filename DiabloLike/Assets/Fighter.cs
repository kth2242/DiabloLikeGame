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
	[SerializeField] private int health = 100;

	private bool isDieAnimStarted = false;
	private bool isDieAnimEnded = false;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Space) && opponent != null)
		{
			if (InRange())
			{
				anim.CrossFade (attack.name);
				isAttacking = true;	
				transform.LookAt (opponent.transform.position);
			}
		}

		// if attacking animation is almost ended
		if (anim[attack.name].time > 0.9 * anim[attack.name].length)
		{
			isAttacking = false;
			impacted = false;
		}

		Impact();

		if (IsDead () && !isDieAnimEnded)
			Die ();
	}

	void Impact()
	{
		if ((opponent != null) && anim.IsPlaying (attack.name) && impacted == false)
		{
			// to make opponent get hit once per one attacking action not per every attacking action frame
			// if the time of the current playing attacking animation is over the time of the attacking animation's impact frame
			if (anim [attack.name].time > anim [attack.name].length * impactTime
			  && anim[attack.name].time < 0.9 * anim[attack.name].length)
			{
				opponent.GetComponent<Mob>().GetHit(damage);
				impacted = true;
			}
		}
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
