using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour {

	public GameObject[] objects;
	public int[] objectNum;
	public List<GameObject>[] pool;

	// Use this for initialization
	void Start () {
		Make ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Make()
	{
		GameObject cloneObject;
		pool = new List<GameObject>[objects.Length];

		for (int i = 0; i < objects.Length; ++i)
		{
			pool [i] = new List<GameObject> ();
			for (int j = 0; j < objectNum [i]; ++j) 
			{
				cloneObject = Instantiate (objects [i]);
				cloneObject.transform.parent = this.transform;
				pool [i].Add (cloneObject);
			}
		}
	}

	public GameObject Activate(int index)
	{
		for (int i = 0; i < pool[index].Count; ++i) 
		{
			if (!pool[index][i].activeSelf)
			{
				pool[index][i].SetActive (true);
				return pool[index][i];
			}
		}
		pool[index].Add(Instantiate(objects[index]));
		pool[index][pool[index].Count-1].transform.parent = this.transform;
		return pool[index][pool[index].Count-1];
	}

	public GameObject Activate(int index, Vector3 pos, Quaternion rot)
	{
		for (int i = 0; i < pool[index].Count; ++i) 
		{
			if (!pool[index][i].activeSelf)
			{
				pool[index][i].SetActive (true);
				pool[index][i].transform.position = pos;
				pool[index][i].transform.rotation = rot;
				return pool[index][i];
			}
		}
		pool[index].Add(Instantiate(objects[index]));
		pool[index][pool[index].Count-1].transform.position = pos;
		pool[index][pool[index].Count-1].transform.rotation = rot;
		pool[index][pool[index].Count-1].transform.parent = this.transform;
		return pool[index][pool[index].Count-1];
	}

	public void DeActivate(GameObject deActivateObject)
	{
		deActivateObject.SetActive (false);
	}
}
