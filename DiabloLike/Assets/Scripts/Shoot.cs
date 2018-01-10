using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

	public Pool pool;
		
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Keypad0))
			pool.Activate (0, new Vector3(10.45f, 1f, 23.39f), Quaternion.identity);
		else if(Input.GetKeyDown (KeyCode.Keypad1))
			pool.Activate (1, new Vector3(15.52f, 0.5f, 24f), Quaternion.identity);
		else if(Input.GetKeyDown (KeyCode.Keypad2))
			pool.Activate (2, new Vector3(20.86f, 0.5f, 24f), Quaternion.identity);
	}
}
