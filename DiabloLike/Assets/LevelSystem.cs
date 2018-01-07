using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour {

	public int level = 1;
	public int exp;
	public Fighter player;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (exp >= level * 100) 
		{
			LevelUp ();	
		}
	}

	void LevelUp()
	{
		exp -= level * 100;
		++level;
		LevelEffect ();
	}

	void LevelEffect()
	{
		player.maxHealth += 100;
		player.health = player.maxHealth;
		player.damage += (int)Mathf.Pow(level-1, 2) * 50;
	}
}
