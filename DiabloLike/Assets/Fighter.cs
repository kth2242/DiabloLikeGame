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

	private bool inAction = false;
	private int ballCount = 0;
	public float desireSkillAngle = 40f;

	// Use this for initialization
	void Start () 
	{
		health = maxHealth;
		anim = GetComponent<Animation>();
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey(KeyCode.Space))
			inAction = true;

		if (inAction) 
		{
			if (!AttackUpdate (KeyCode.Space, null, 0, true))
				inAction = false;
		}

		if (IsDead () && !isDieAnimEnded)
			Die ();
	}

	public bool AttackUpdate(KeyCode key, GameObject ball, int ballNum, bool opponentBased)
	{
		if (opponentBased)
		{
			if (Input.GetKey (key) && opponent != null
			   && !IsDead () && InRange ()) 
			{
				anim.CrossFade (attack.name);
				isAttacking = true;	
				transform.LookAt (opponent.transform.position);
			}
		}
		else
		{
			if (Input.GetKey (key) && !IsDead ()) 
			{
				anim.CrossFade (attack.name);
				isAttacking = true;	
				transform.LookAt (ClickToMove.cursorPos);
			}
		}

		// if attacking animation is almost ended
		if (anim[attack.name].time > 0.9 * anim[attack.name].length)
		{
			isAttacking = false;
			impacted = false;
			ballCount = 0;
			return false;
		}
		Impact(ball, ballNum, opponentBased);
		return true;
	}

	public void ResetAttackUpdate()
	{
		isAttacking = false;
		impacted = false;
		anim.Stop (attack.name);
	}

	void Impact(GameObject ball, int ballNum, bool opponentBased)
	{
		if ((!opponentBased || opponent != null) && anim.IsPlaying(attack.name) && impacted == false)
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
				if(opponentBased)
				{
					opponent.GetComponent<Mob>().GetHit(damage);
					impacted = true;
				}

				// send out spheres (projectile)
				if (ball != null)
				{
					Quaternion rot = transform.rotation;
					rot.x = 0;
					rot.z = 0;

					if (ballNum == 1 && ballCount < ballNum)
					{
						Instantiate (ball, new Vector3 (transform.position.x, transform.position.y + 0.9f, transform.position.z), rot);
						++ballCount;
					}
					else if(ballNum > 1)
					{
						float diff = desireSkillAngle / (ballNum - 1);

						while (ballCount < ballNum)
						{
							GameObject newBall = Instantiate (ball, new Vector3 (transform.position.x, transform.position.y + 0.9f, transform.position.z), rot);
							newBall.transform.Rotate (Vector3.up * (-(desireSkillAngle / 2) + diff * ballCount));
							++ballCount;
						}
					}
				}
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
