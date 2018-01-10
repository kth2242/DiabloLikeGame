using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	public Texture2D frame;
	public Rect framePos;
	public Texture2D healthBar;
	public Rect healthBarPos;
	public float horizontalDistance;
	public float verticalDistance;
	public float widthScale;
	public float heightScale;
	public Fighter player;
	public Mob target;
	public float healthPercentage = 0f;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (player.opponent != null)
		{
			target = player.opponent.GetComponent<Mob> ();
			healthPercentage = (float)target.health / target.maxHealth;
		}
	}

	void OnGUI()
	{
		if (player.opponent != null && player.countDown > 0)
		{
			DrawFrame ();
			DrawBar ();
		}
	}

	void DrawFrame()
	{
		framePos.x = (Screen.width - framePos.width) / 2;
		// to make the same ratio depending on the screen resolution
		float width = 0.364583f;  // framePos.width  / Screen.currentResolution.width;
		float height = 0.092592f; // framePos.height / Screen.currentResolution.height;
		framePos.width = Screen.width * width;
		framePos.height = Screen.height * height;
		GUI.DrawTexture (framePos, frame);
	}

	void DrawBar()
	{
		healthBarPos.x = framePos.x + framePos.width * horizontalDistance;
		healthBarPos.y = framePos.y + framePos.height * verticalDistance;
		healthBarPos.width = framePos.width * widthScale * healthPercentage;
		healthBarPos.height = framePos.height * heightScale;
		GUI.DrawTexture (healthBarPos, healthBar);
	}
}
