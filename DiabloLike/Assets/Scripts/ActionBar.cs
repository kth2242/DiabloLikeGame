using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBar : MonoBehaviour {

	public Texture2D actionBar;
	public Rect position;
	public SkillSlot[] skills;

	public float skillX;
	public float skillY;
	public float skillWidth;
	public float skillHeight;
	public float skillDistance;

	// Use this for initialization
	void Start () 
	{
		Init ();	
	}
	
	// Update is called once per frame
	void Update ()
	{
		UpdateSkillSlots ();		
	}

	void Init()
	{
		SpecialAttack[] attacks = GameObject.FindGameObjectWithTag ("Player").GetComponents<SpecialAttack>();

		skills = new SkillSlot[attacks.Length];

		for (int i = 0; i < attacks.Length; ++i) 
		{
			skills[i] = new SkillSlot();
			skills[i].skill = attacks[i];
		}

		skills[0].key = KeyCode.Q;
		skills[1].key = KeyCode.W;
		//skills[2].key = KeyCode.E;
		//skills[3].key = KeyCode.R;
	}

	void UpdateSkillSlots()
	{
		SpecialAttack[] attacks = GameObject.FindGameObjectWithTag ("Player").GetComponents<SpecialAttack>();

		for (int i = 0; i < attacks.Length; ++i) 
		{
			skills[i].skill = attacks[i];
			skills [i].position.Set (skillX + i * (skillWidth + skillDistance),	skillY, skillWidth, skillHeight);
		}
	}

	void OnGUI()
	{
		DrawActionBar ();
		DrawSkillSlot ();
	}

	void DrawActionBar()
	{
		GUI.DrawTexture (GetScreenRect(position), actionBar);
	}

	void DrawSkillSlot()
	{
		for (int i = 0; i < skills.Length; ++i)
		{
			GUI.DrawTexture(GetScreenRect(skills[i].position), skills[i].skill.skillPicture);
		}
	}

	Rect GetScreenRect(Rect pos)
	{
		return new Rect (Screen.width * pos.x, Screen.height * pos.y, Screen.width * pos.width, Screen.height * pos.height);
	}
}
