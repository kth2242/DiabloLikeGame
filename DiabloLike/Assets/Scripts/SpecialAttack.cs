using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour {

	public Texture2D skillPicture;
	public Fighter player;
	public KeyCode key;
	private bool inAction = false;
	public GameObject ball;
	public int ballNum = 1;
	public bool isActivated = true;

	// Update is called once per frame
	void Update () 
	{
		if (isActivated && Input.GetKey(key) && !inAction)
		{
			player.ResetAttackUpdate ();
			inAction = true;
		}

		if (inAction) 
		{
			if (!player.AttackUpdate (key, ball, ballNum, false))
				inAction = false;
		}
	}
}
