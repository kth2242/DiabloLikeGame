using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour {
	
	private int interval = 120; // in terms of frame
	private int count;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (count >= interval)
		{
			// save
			SavePosition();
			count = 0;
		}
		++count;
	}

	void SavePosition()
	{
		PlayerPrefs.SetFloat("x", transform.parent.position.x);
		PlayerPrefs.SetFloat("y", transform.parent.position.y);
		PlayerPrefs.SetFloat("z", transform.parent.position.z);
	}

	public static Vector3 ReadPlayerPosition()
	{
		Vector3 position = new Vector3 ();
		position.x = PlayerPrefs.GetFloat ("x");
		position.y = PlayerPrefs.GetFloat ("y");
		position.z = PlayerPrefs.GetFloat ("z");

		return position;
	}

	public static void SaveMobHealth(int id, int health)
	{
		PlayerPrefs.SetInt ("MobHealth" + id, health);
	}

	public static int ReadMobHealth(int id)
	{
		if (PlayerPrefs.HasKey ("MobHealth" + id))
			return PlayerPrefs.GetInt ("MobHealth" + id);
		else
			return -1;
	}
}
