﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JsonResponse  
{
	[System.Serializable]
	public class BasicResponse //JsonResponse
	{
		public string id;
		public string message;
		public bool error;
	}

	[System.Serializable]
	public class BasicIntResponse
	{
		public int count;
	}

	[System.Serializable]
	public class PlayerDataResponse
	{
		public string name;
		public string avatar;
		public int currency;
		public bool survey;
	}

	[System.Serializable]
	public class EchoResponse
	{
		public string[] ResultingData;
	}

	[System.Serializable]
	public class DataAnimal
	{
		public string species;
		public string name;
		public string nahuatl_name;
		public string spanish_name;
		public string description;
		public string habitat_level;
		public float min_size;
		public float max_size;
		public float min_age;
		public float max_age;
		public float min_weight;
		public float max_weight;
		public string colorkey_map_file;
	}

	[System.Serializable]
	public class ListResponse //DictResponse
	{
		public List<DataAnimal> AnimalData;
	}

	[System.Serializable]
	public class DiscoveredSpeciesData
	{
		public string animal_species;
		public string discovered_date;
	}

	[System.Serializable]
	public class DiscoveredListResponse
	{
		public bool empty;
		public List<DiscoveredSpeciesData> DiscoveredSpeciesData;
	}

	[System.Serializable]
	public class OwnedAnimalResponse
	{		
		public bool empty;
		public List<OwnedAnimalData> OwnedAnimalData;
	}

	//TODO: Update these last 2
	[System.Serializable]
	public class OwnedAnimalData
	{
		public int animal_id;
		public string animal_species;
		public string nickname;
		public float height;
		public float age;
		public float weight;
		public float health_1;
		public float health_2;
		public float health_3;
	}
		
	[System.Serializable]
	public class GennedAnimalData
	{
		public string animal_species;
		public int animal_id;
		public float health_1;
		public float health_2;
		public float health_3;
		public float size;
		public float weight;
		public float age;
	}

	[System.Serializable]
	public class EncounteredAnimalResponse
	{
		public bool empty;
		public List<EncounterData> EncounterData;
	}

	[System.Serializable]
	public class EncounterData
	{
		public int animal_id;
		public string species;
		public float health_1;
		public float health_2;
		public float health_3;
		public string encounter_date;
	}

	[System.Serializable]
	public class JournalResponse
	{
		public bool empty;
		public List<JournalEntryData> JournalEntryData;
	}

	[System.Serializable]
	public class JournalEntryData
	{
		public int animal_id;
		public string species;
		public string encounter_type;
		public string caught_date;
		public float caught_health_1;
		public float caught_health_2;
		public float caught_health_3;
		public string released_date;
		public float released_health_1;
		public float released_health_2;
		public float released_health_3;
	}

	[System.Serializable]
	public class LocationData
	{
		public int location_id;
		public string location_name;
		public double x_coordinate;
		public double y_coordinate;
		public string description;
		public string species;
	}

	[System.Serializable]
	public class LocationResponse
	{
		public bool empty;
		public List<LocationData> LocationData;
	}

	[System.Serializable]
	public class MajorEntryData
	{
		public string major;
		public string major_name;
		public string description;
	}

	[System.Serializable]
	public class MajorEntriesResponse
	{
		public bool empty;
		public List<MajorEntryData> MajorEntries;
	}

	//ODD ONE OUT - MOVE ME
	public class LoginResponse
	{
		public bool error;
		public string session_key;

		public LoginResponse(bool hasError, string key)
		{
			error = hasError;
			session_key = key;
		}
	}
}