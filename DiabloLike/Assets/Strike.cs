using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strike : MonoBehaviour {

	public float speed = 1.5f;
	public int damage = 100;
	public int stunTime = 4;
	public float life = 2f;
	public GameObject particleEffect;
	private int ERROR_CODE = 79531246;

	// Update is called once per frame
	void Update ()
	{
		ReachUpdate ();

		Vector3 enemyPos = EnemyInRange ();

		if (enemyPos != new Vector3 (ERROR_CODE, ERROR_CODE, ERROR_CODE))
			transform.position = Vector3.MoveTowards (transform.position, enemyPos, speed * Time.deltaTime);
		else
			transform.Translate (Vector3.forward * speed * Time.deltaTime);			
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy") 
		{
			other.GetComponent<Mob> ().GetHit(damage);
			other.GetComponent<Mob>().GetStun(stunTime);
			GetComponent<SphereCollider> ().enabled = false;
			GetComponent<Renderer> ().enabled = false;

			// play particle effect
			if (particleEffect != null)
			{
				life = 5f; // reset life time
				Instantiate (particleEffect, new Vector3 (other.transform.position.x, other.transform.position.y + 0.9f, other.transform.position.z), Quaternion.identity);
			}
		}
	}

	void ReachUpdate()
	{
		life -= Time.deltaTime;

		if (life <= 0f && gameObject != null)
		{
			//if (particleEffect != null && !particleEffect.GetComponent<ParticleSystem>().IsAlive())
				//Destroy (particleEffect);
				//Destroy (particleEffect, particleEffect.GetComponent<ParticleSystem> ().main.duration);
			
			Destroy (this.gameObject);
		}
	}

	Vector3 EnemyInRange()
	{
		GameObject[] opponents = GameObject.FindGameObjectsWithTag ("Enemy");

		foreach (GameObject enemy in opponents) 
		{
			if (Vector3.Distance (transform.position, enemy.transform.position) <= 1.5)
				return enemy.transform.position;
		}
		return new Vector3(ERROR_CODE, ERROR_CODE, ERROR_CODE);
	}
}
