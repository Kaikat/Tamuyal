﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MajorsButton : MonoBehaviour 
{
	public void Click()
	{
		EventManager.TriggerEvent (GameEvent.SwitchScreen, ScreenType.MajorsJournal);
	}
}