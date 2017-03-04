﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScreenManager : MonoBehaviour {

	public ScreenType ActiveScreen;
	public List<TaggedShowHide> Screens;

	private Dictionary<ScreenType, TaggedShowHide> screenMap;

	void Start ()
	{
		Screen.orientation = ScreenOrientation.Landscape;

		screenMap = new Dictionary<ScreenType, TaggedShowHide> ();

		foreach (TaggedShowHide screen in Screens)
		{
			screenMap [screen.Tag] = screen;
		}
		screenMap [ActiveScreen].Show ();

		EventManager.RegisterEvent <ScreenType> (GameEvent.Login, ShowScreen);
		EventManager.RegisterEvent (GameEvent.Home, () => ShowScreen(ScreenType.Home));
		EventManager.RegisterEvent <ScreenType> (GameEvent.CreateNewAccount, ShowScreen);
		EventManager.RegisterEvent <ScreenType> (GameEvent.IDCard, ShowScreen);
		EventManager.RegisterEvent <ScreenType> (GameEvent.CatchAnimal, ShowScreen);
		//Journal
		EventManager.RegisterEvent <ScreenType> (GameEvent.Journal, ShowScreen);
		EventManager.RegisterEvent <ScreenType> (GameEvent.Caught, ShowScreen);

	}

	public void ShowScreen(ScreenType screen)
	{
		Debug.LogWarning (screen.ToString ());
		//Hide previous screen and show active screen
		screenMap [ActiveScreen].Hide ();
		ActiveScreen = screen;
		screenMap [ActiveScreen].Show ();
	}

	public void Destroy()
	{
		EventManager.UnregisterEvent <ScreenType> (GameEvent.Login, ShowScreen);
		EventManager.UnregisterEvent <ScreenType> (GameEvent.Home, ShowScreen);
		EventManager.UnregisterEvent <ScreenType> (GameEvent.CreateNewAccount, ShowScreen);
		EventManager.UnregisterEvent <ScreenType> (GameEvent.IDCard, ShowScreen);
		EventManager.UnregisterEvent <ScreenType> (GameEvent.CatchAnimal, ShowScreen);
		EventManager.UnregisterEvent <ScreenType> (GameEvent.Journal, ShowScreen);
		EventManager.UnregisterEvent <ScreenType> (GameEvent.Caught, ShowScreen);
	}
}












































/*public void ChangeScreen(GameEvent gameEvent)
	{
		switch (gameEvent)
		{
			case GameEvent.Home:
			break;

			case GameEvent.CreateNewAccount:
			break;

			default:
				break;
		}
	}*/

//EventManager.RegisterEvent(GameEvent.Home, ()=>ShowScreen("Home"));
//EventManager.RegisterEvent(GameEvent.Home, ShowScreen);
/*EventManager.RegisterEvent(GameEvent.Junk, Junk);
		EventManager.RegisterEvent(GameEvent.LoginSuccessful, Destroy);
		EventManager.RegisterEvent(GameEvent.Home, Destroy);*/