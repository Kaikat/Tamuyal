﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GetName : MonoBehaviour
{
	Player player;
	Text t;

	// Use this for initialization
	void Start ()
	{
		player = StartGame.CurrentPlayer;
		t = GetComponent<Text> ();
		//t.text = player.GetName ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}

