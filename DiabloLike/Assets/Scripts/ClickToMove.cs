using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove : MonoBehaviour {

	public float speed = 10f;
	private CharacterController controller;
	private Vector3 position;
	public AnimationClip run;
	public AnimationClip idle;
	private Animation anim;

	public static Vector3 cursorPos;

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();
		position = transform.position;
		anim = GetComponent<Animation> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		CursorUpdate ();

		if (!Fighter.isAttacking && !Fighter.isDead)
		{
			if (Input.GetKey(KeyCode.Mouse0)) //GetMouseButtonDown (0))
			{
				// Locate where the player clicked on the terrain
				LocatePosition ();
			}

			// Move the player to the position
			MoveToPosition ();
		}
	}

	void LocatePosition()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if(Physics.Raycast(ray, out hit, 1000f))
		{
			if (hit.collider.tag != "Player" && hit.collider.tag != "Enemy")
				position = hit.point;
		}
	}

	void CursorUpdate()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if(Physics.Raycast(ray, out hit, 1000f))
		{
			cursorPos = hit.point;
		}
	}

	void MoveToPosition()
	{
		// Game object is moving
		if (Vector3.Distance (transform.position, position) > 1)
		{
			Quaternion newRotation = Quaternion.LookRotation (position - transform.position);

			transform.rotation = Quaternion.Slerp (transform.rotation, newRotation, Time.deltaTime * 10);
			controller.SimpleMove (transform.forward * speed);

			anim.CrossFade(run.name);
		}
		// Game object is not moving
		else
		{
			anim.CrossFade(idle.name);
		}
	}
}
