using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour {

	public Fighter player;
	public KeyCode key;
	public float damagePercentage = 1.5f;
	public int stunTime = 4;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey(key))
		{
			player.isSpecialAttackOn = true;
			player.ResetAttackUpdate ();
		}

		player.AttackUpdate (stunTime, damagePercentage, key);
	}
}
