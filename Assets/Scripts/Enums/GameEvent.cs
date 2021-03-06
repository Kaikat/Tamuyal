﻿using UnityEngine;
using System.Collections;

public enum GameEvent {
	Test,
	Spawn,
	Destroy,
	Junk,

	SwitchScreen,

	// For Catch Animal Screen
	GPSInitialized,
	AnimalEncounter,
	AnimalCaught,
    ViewingAnimalsUnderObservation,
    ViewingAnimalInformation,
	QuizTime,


	ObservedAnimalsPreviousScreen,




	AccountCreationSuccess,
	LoginSuccessful,

	WifiAvailable,
	WifiUnavailable,
	ScreenManagerInitialized,
}
