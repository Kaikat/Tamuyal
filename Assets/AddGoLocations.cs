﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoShared;

public class AddGoLocations : MonoBehaviour 
{
	public GoMap.GOMap goMap;
	public GameObject prefab;

	private List<AnimalLocation> locations;
	private bool stationsLoaded = false;

	void Awake()
	{
		Event.Request.RegisterEvent <ScreenType> (GameEvent.SwitchScreen, LoadStations);
	}

	void Start () 
	{
		locations = Service.Request.PlacesToVisit ();
		if (locations == null)
		{
			Event.Request.RegisterEvent (GameEvent.WifiAvailable, Init);
		}
	}

	void Init()
	{
		locations = Service.Request.PlacesToVisit ();
		if (locations != null)
		{
			Event.Request.UnregisterEvent (GameEvent.WifiAvailable, Init);
		}
	}

	public void LoadStations(ScreenType screen)
	{
		if (stationsLoaded)
		{
			return;
		}
			
		if (screen == ScreenType.GoMapHome) 
		{
			if (!Service.Request.Player ().Survey)
			{
				return;
			}

			LoadProperGameVersion ();
			stationsLoaded = true;
		}
	}

	void Destroy()
	{
		Event.Request.UnregisterEvent<ScreenType> (GameEvent.SwitchScreen, LoadStations);
	}

	private void LoadProperGameVersion()
	{
		GameVersion version = Service.Request.Player ().Username.GetGameVersion ();
		switch (version)
		{
			case GameVersion.TrackVisits:
				LoadUncoloredVersion ();
				break;
			case GameVersion.ColorCodedMajors:
				LoadMajorColorCodesVersion ();
				break;
			default:
				LoadFullVersion ();
				break;
		}
	}

	private void LoadFullVersion()
	{
		Dictionary<string, MajorLocationData>  playersRecommendations = Service.Request.Player().GetRecommendations();

		foreach(AnimalLocation location in locations)
		{
			Coordinates coordinates = new Coordinates (location.Location.Coordinate.x, location.Location.Coordinate.y, 0.0f);
			GameObject go = GameObject.Instantiate (prefab);
			Vector3 coordinate = coordinates.convertCoordinateToVector();
			go.transform.localPosition = new Vector3 (coordinate.x, coordinate.y + 5.0f, coordinate.z);
			go.transform.parent = transform;
			go.name = location.Location.LocationName;

			//Top 7 recommendations will be color coded
			//top 4 = gold, next 3 = blue, rest = green
			//Already visited locations should be what color? gray? Or should it be a different asset?
			//Making the asset not appear would make it hard if someone wanted to catch the same animal again.
			if (Service.Request.Player ().isAnimalOwned (location.Animal) ||
				Service.Request.Player ().hasReleasedAnimal (location.Animal)) 
			{
				go.GetComponentInChildren<BannerColor> ().SetBannerData (Resources.Load<Texture> ("gray"), location.Animal);
			} 
			else if(playersRecommendations.ContainsKey(location.Location.LocationName))
			{
				if (playersRecommendations [location.Location.LocationName].Index < UIConstants.Recommended) 
				{
					go.GetComponentInChildren<BannerColor> ().SetBannerData (Resources.Load<Texture> ("yellow"), location.Animal);
				}
				else if (playersRecommendations[location.Location.LocationName].Index >= UIConstants.Recommended && 
					playersRecommendations[location.Location.LocationName].Index < UIConstants.SomewhatRecommended)
				{
					go.GetComponentInChildren<BannerColor> ().SetBannerData (Resources.Load<Texture> ("blue"), location.Animal);
				}
				else 
				{
					go.GetComponentInChildren<BannerColor> ().SetBannerData (Resources.Load<Texture> ("green"), location.Animal);
				}
			}
			else
			{
				Debug.LogWarning ("ERROR KEY *" + location.Location.LocationName + "* was not in dictionary");
			}
		}
	}

	private void LoadMajorColorCodesVersion()
	{
		Dictionary<string, List<Major>> majorLocations = Service.Request.GetMajorsAtLocation ();
		if (majorLocations == null)
		{
			return;
		}

		foreach (AnimalLocation location in locations)
		{
			Coordinates coordinates = new Coordinates (location.Location.Coordinate.x, location.Location.Coordinate.y, 0.0f);
			GameObject go = GameObject.Instantiate (prefab);
			Vector3 coordinate = coordinates.convertCoordinateToVector();
			go.transform.localPosition = new Vector3 (coordinate.x, coordinate.y + 5.0f, coordinate.z);
			go.transform.parent = transform;
			go.name = location.Location.LocationName;

			//NOTE: This assumes that every location only as majors of the same type, for example,
			//		2+ STEM majors at one spot, not a combination of major types
			if (Service.Request.Player ().isAnimalOwned (location.Animal) ||
			    Service.Request.Player ().hasReleasedAnimal (location.Animal))
			{
				go.GetComponentInChildren<BannerColor> ().SetBannerData (Resources.Load<Texture> ("gray"), location.Animal);
			} 
			else if (GameConstants.STEM.Contains(majorLocations[location.Location.LocationName][0]))
			{
				go.GetComponentInChildren<BannerColor> ().SetBannerData (Resources.Load<Texture> ("yellow"), location.Animal);
			}
			else if (GameConstants.SocialSciences.Contains(majorLocations[location.Location.LocationName][0]))
			{
				go.GetComponentInChildren<BannerColor>().SetBannerData (Resources.Load<Texture>("blue"), location.Animal);
			}
			else
			{
				go.GetComponentInChildren<BannerColor>().SetBannerData (Resources.Load<Texture>("green"), location.Animal);
			}
		}
	}

	private void LoadUncoloredVersion()
	{
		foreach (AnimalLocation location in locations)
		{
			Coordinates coordinates = new Coordinates (location.Location.Coordinate.x, location.Location.Coordinate.y, 0.0f);
			GameObject go = GameObject.Instantiate (prefab);
			Vector3 coordinate = coordinates.convertCoordinateToVector();
			go.transform.localPosition = new Vector3 (coordinate.x, coordinate.y + 5.0f, coordinate.z);
			go.transform.parent = transform;
			go.name = location.Location.LocationName;

			//NOTE: This is for visited/not visited so it will go off of whether or not an animal has been
			//		discovered or not. The other options go off whether or not the animal is owned/released
			if (Service.Request.Player().HasDiscoveredAnimal(location.Animal))
			{
				go.GetComponentInChildren<BannerColor> ().SetBannerData (Resources.Load<Texture> ("gray"), location.Animal);
			} 
			else
			{
				go.GetComponentInChildren<BannerColor>().SetBannerData(Resources.Load<Texture>("yellow"), location.Animal);
			}
		}
	}
}
