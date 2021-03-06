using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player {
	
	public string SessionKey;
	public string Username { set; get; } //TODO: Make private again
	public string Name { private set; get; }
	public Avatar Avatar { private set; get; }
	public int Currency { private set; get; }
	public bool Survey { get; set; } //TODO: Make private

	public int AnimalsDiscovered { private set; get; }
	public int AnimalsCaught { private set; get; }
	public int AnimalsReleased { private set; get; }
	public int AnimalsNursing { private set; get; }

	private Dictionary<AnimalSpecies, List<Animal>> Animals;
	private Dictionary<AnimalSpecies, List<Animal>> ReleasedAnimals;
	private List<DiscoveredAnimal> DiscoveredAnimals;
	private Dictionary<string, MajorLocationData> Recommendations;

	public void Print()
	{
		string fstring = "SessionKey: " + SessionKey;
		fstring += "\nUsername : " + Username;
		fstring += "\nName : " + Name;
		fstring += "\nAvatar : " + Avatar;
		fstring += "\nCurrency : " + Currency.ToString ();
		fstring += "\nNumDiscovered : " + AnimalsDiscovered.ToString ();
		fstring += "\nNumCaught : " + AnimalsCaught.ToString ();
		fstring += "\nNumReleased : " + AnimalsReleased.ToString ();
		fstring += "\nNumNursing : " + AnimalsNursing.ToString ();
		Debug.LogWarning (fstring);
	}

	//TODO: ONLY USED BY OLD STUFF. Should be using the new constructor.
	public Player(string username, string name, Avatar avatar, int currency, int discovered, int caught, int released,
		Dictionary<AnimalSpecies, List<Animal>> ownedAnimals, Dictionary<AnimalSpecies, List<Animal>> releasedAnimals,
		List<DiscoveredAnimal> discoveredAnimals)
	{
		Username = username;
		Name = name;
		Avatar = avatar;
		Currency = currency;
		Survey = false;

		Animals = ownedAnimals;
		ReleasedAnimals = releasedAnimals;
		DiscoveredAnimals = discoveredAnimals;

		AnimalsDiscovered = discovered;
		AnimalsCaught = caught;
		AnimalsReleased = released;
		AnimalsNursing = Animals.Count;
	}

	//doesn't get called?
	/*public Player(string name, Avatar avatar, int currency, int discovered, int caught, int released,
		Dictionary<AnimalSpecies, List<Animal>> ownedAnimals, Dictionary<AnimalSpecies, List<Animal>> releasedAnimals,
		List<DiscoveredAnimal> discoveredAnimals)
	{
		Name = name;
		Avatar = avatar;
		Currency = currency;

		Animals = ownedAnimals;
		ReleasedAnimals = releasedAnimals;
		DiscoveredAnimals = discoveredAnimals;

		AnimalsDiscovered = discovered;
		AnimalsCaught = caught;
		AnimalsReleased = released;
		AnimalsNursing = Animals.Count;
	}*/

	public Player(string name, Avatar avatar, int currency, bool survey,
		Dictionary<AnimalSpecies, List<Animal>> ownedAnimals, Dictionary<AnimalSpecies, List<Animal>> releasedAnimals,
		List<DiscoveredAnimal> discoveredAnimals, Dictionary<string, MajorLocationData> recommendations)
	{
		Name = name;
		Avatar = avatar;
		Currency = currency;
		Survey = survey;

		if (ownedAnimals.ContainsKey(AnimalSpecies.Horse))
		{
			ownedAnimals.Remove (AnimalSpecies.Horse);
		}
		if (ownedAnimals.ContainsKey (AnimalSpecies.Butterfly))
		{
			ownedAnimals.Remove (AnimalSpecies.Butterfly);
		}
		if (ownedAnimals.ContainsKey (AnimalSpecies.Tiger))
		{
			ownedAnimals.Remove (AnimalSpecies.Tiger);
		}

		Animals = ownedAnimals;

		if (releasedAnimals.ContainsKey(AnimalSpecies.Horse))
		{
			releasedAnimals.Remove (AnimalSpecies.Horse);
		}
		if (releasedAnimals.ContainsKey (AnimalSpecies.Butterfly))
		{
			releasedAnimals.Remove (AnimalSpecies.Butterfly);
		}
		if (releasedAnimals.ContainsKey (AnimalSpecies.Tiger))
		{
			releasedAnimals.Remove (AnimalSpecies.Tiger);
		}

		ReleasedAnimals = releasedAnimals;

		for (int i = discoveredAnimals.Count - 1; i >= 0; i--)
		{
			if (discoveredAnimals[i].Species == AnimalSpecies.Horse || discoveredAnimals[i].Species == AnimalSpecies.Butterfly ||
				discoveredAnimals[i].Species == AnimalSpecies.Tiger)
			{
				discoveredAnimals.RemoveAt (i);
			}
		}

		DiscoveredAnimals = discoveredAnimals;
		AnimalsDiscovered = discoveredAnimals.Count;

		int ownedAnimalCount = 0;
		List<AnimalSpecies> ownedKeyList = new List<AnimalSpecies> (ownedAnimals.Keys);
		foreach (AnimalSpecies species in ownedKeyList)
		{
			ownedAnimalCount += ownedAnimals [species].Count;
		}

		int releasedAnimalCount = 0;
		List<AnimalSpecies> releasedKeyList = new List<AnimalSpecies> (releasedAnimals.Keys);
		foreach (AnimalSpecies species in releasedKeyList)
		{
			releasedAnimalCount += releasedAnimals [species].Count;
		}

		AnimalsCaught = ownedAnimalCount + releasedAnimalCount;
		AnimalsReleased = releasedAnimalCount;

		AnimalsNursing = ownedAnimalCount;
		Recommendations = recommendations;
	}

	public List<AnimalLocation> GetDiscoveredPlaces()
	{
		List<AnimalLocation> visitedPlaces = new List<AnimalLocation> ();
		List<AnimalLocation> allPlaces = Service.Request.PlacesToVisit ();

		foreach (AnimalLocation place in allPlaces)
		{
			if (HasDiscoveredAnimal (place.Animal))
			{
				visitedPlaces.Add (place);
			}
		}

		return visitedPlaces;
	}

	public List<AnimalLocation> GetUndiscoveredPlaces()
	{
		List<AnimalLocation> undiscoveredPlaces = new List<AnimalLocation> ();
		List<AnimalLocation> allPlaces = Service.Request.PlacesToVisit ();

		foreach (AnimalLocation place in allPlaces)
		{
			if (!HasDiscoveredAnimal (place.Animal))
			{
				undiscoveredPlaces.Add (place);
			}
		}

		return undiscoveredPlaces;
	}

	public void SetAvatar(Avatar avatar)
	{
		Avatar = avatar;
	}

	public void SetRecommendations(Dictionary<string, MajorLocationData> recommendations)
	{
		Recommendations = recommendations;
	}

	public Dictionary<string, MajorLocationData>  GetRecommendations()
	{
		return Recommendations;
	}

	public int GetRecommendationIndex(string location)
	{
		return Recommendations [location].Index;
	}

	public void Destroy()
	{
	}

	public Dictionary<AnimalSpecies, List<Animal>> GetAnimals() 
	{
		return Animals;
	}

	public Dictionary<AnimalSpecies, List<Animal>> GetReleasedAnimals()
	{
		return ReleasedAnimals;
	}

	public bool HasDiscoveredAnimal(AnimalSpecies species)
	{
		for (int i = 0; i < DiscoveredAnimals.Count; i++)
		{
			if (DiscoveredAnimals [i].Species == species)
			{
				return true;
			}
		}

		return false;
	}

	public bool isAnimalOwned(AnimalSpecies species)
	{
		return Animals.ContainsKey (species);
	}

	public bool hasReleasedAnimal(AnimalSpecies species)
	{
		return ReleasedAnimals.ContainsKey (species);
	}

	public void AddDiscoveredAnimal(AnimalSpecies species, string discovered_date)
	{
		if (!HasDiscoveredAnimal(species))
		{
			DiscoveredAnimals.Add (new DiscoveredAnimal(species, discovered_date));
			AnimalsDiscovered++;
		}
	}

	public void AddOwnedAnimal(Animal animal)
	{
		AnimalSpecies species = animal.Species;
		if (Animals.ContainsKey (species)) 
		{				
			Animals [species].Add (animal);
		} 
		else 
		{
			Animals.Add (species, new List<Animal>());
			Animals[species].Add(animal);
			AnimalsDiscovered++;
		}
		AnimalsCaught++;
		AnimalsNursing++;
	}

	public void RemoveOwnedAnimal(Animal animal)
	{
		AnimalSpecies species = animal.Species;
		if (Animals.ContainsKey (species))
		{
			List<Animal> animalsOfSpecies = Animals [species];
			for (int i = animalsOfSpecies.Count - 1; i >= 0; i--)
			{
				Animal animalToRemove = animalsOfSpecies [i];
				if (animalToRemove.AnimalID == animal.AnimalID)
				{
					animalsOfSpecies.Remove (animalToRemove);
				}
			}
		}
		AnimalsNursing--;
	}

	public void AddReleasedAnimal(Animal animal)
	{
		AnimalSpecies species = animal.Species;
		if (ReleasedAnimals.ContainsKey (species))
		{				
			ReleasedAnimals [species].Add (animal);
		}
		else
		{
			ReleasedAnimals.Add (species, new List<Animal> ());
			ReleasedAnimals [species].Add (animal);
		}
		AnimalsReleased++;
	}

	public Animal GetAnimalBySpeciesAndID(AnimalSpecies species, int animalID)
	{
		if (Animals.ContainsKey(species)) 
		{
			List<Animal> animals = Animals [species];
			foreach (Animal animal in animals) 
			{
				if (animal.AnimalID == animalID) 
				{
					return animal;
				}
			}
		}

		if (ReleasedAnimals.ContainsKey(species)) 
		{
			List<Animal> releasedAnimals = ReleasedAnimals [species];
			foreach (Animal animal in releasedAnimals) 
			{
				if (animal.AnimalID == animalID) 
				{
					return animal;
				}
			}
		}

		return null;
	}
}



/*private void SpawnFunction()
	{
		Debug.Log ("Spawning");
		tigerAsset = (GameObject)GameObject.Instantiate (AssetManager.GetAnimalPrefab (AnimalSpecies.Tiger));


		//tigerAsset = (GameObject)GameObject.Instantiate (AssetManager.GetAnimalPrefab (AnimalSpecies.Tiger), new Vector3(0.0f, 0.0f, -5.0f), Quaternion.identity);
		//Animal animal = new Animal ("tiger1", AnimalSpecies.Tiger, "Tigecito", AnimalEncounterType.Caught, HabitatLevelType.Middle);
		//tigerAsset = (GameObject)GameObject.Instantiate(Resources.Load("AnimalPrefabs/Tiger"));//, new Vector3 (0f, 0f, 0f), Quaternion.identity);
	}*/
