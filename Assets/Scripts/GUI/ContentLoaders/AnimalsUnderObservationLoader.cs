﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalsUnderObservationLoader : MonoBehaviour, IShowHideListener 
{
	//public Camera BackgroundGUICamera;
	public GameObject AnimalGrid;
    public GameObject UIObjectScreen;

    private AnimalSpecies animalSpecies;
    private bool isAnimalSpeciesSet = false;

    private List<GameObject> rows = new List<GameObject> ();
	private List<GameObject> animalModels = new List<GameObject>();

	private const string ANIMAL_UNDER_OBS_PREFAB = "RowFab";
	private const string PREFAB_FOLDER = "UIPrefabs";
	private const int NUM_COLUMNS = 4;

	Dictionary<AnimalSpecies, Vector3> AnimalScales;
	Dictionary<AnimalSpecies, Vector3> AnimalRotations;

    void Awake()
    {
        EventManager.RegisterEvent<AnimalSpecies>(GameEvent.ViewingAnimalsUnderObservation, SetAnimalSpecies);
        UIObjectScreen.GetComponent<TaggedShowHide>().listener = this;
		SetAnimalScales ();
		SetAnimalRotations ();
    }

    void Destroy()
    {
        EventManager.UnregisterEvent<AnimalSpecies>(GameEvent.ViewingAnimalsUnderObservation, SetAnimalSpecies);
    }

    private void SetAnimalSpecies(AnimalSpecies species)
    {
        animalSpecies = species;
		isAnimalSpeciesSet = true;
    }

    public void OnShow()
    {
		if (!isAnimalSpeciesSet) 
		{
			return;
		}

		//TODO: This is messy and was for testing purposes
		int totalAnimals = 0;
        Player player = Service.Request.Player();
		if (player.isAnimalOwned(animalSpecies))
		{
			GameObject parentlessPrefab = AssetManager.LoadPrefab(PREFAB_FOLDER, ANIMAL_UNDER_OBS_PREFAB) as GameObject;
			List<Animal> ownedAnimals = player.GetAnimals()[animalSpecies];
			totalAnimals = ownedAnimals.Count;

			for (int i = 0; i < (ownedAnimals.Count / NUM_COLUMNS) + 1; i++) 
			{
				GameObject row = Instantiate(parentlessPrefab);
				row.transform.SetParent (AnimalGrid.transform);
				row.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
				row.transform.localPosition = new Vector3 (0.0f, 0.0f, 0.0f);
				rows.Add (row);
			}
            
            for (int i = 0; i < ownedAnimals.Count || i < rows.Count * NUM_COLUMNS; i++)
            {
                int row = i / NUM_COLUMNS;
                int col = i % NUM_COLUMNS;
                if (i < ownedAnimals.Count)
                {
                    RawImage animalCard = rows[row].gameObject.GetComponent<RowInAnimalGrid>().AnimalCards[col];
                    GameObject animalModel = AssetManager.GetAnimalClone(animalSpecies);
                    Vector3 animalCardPosition = animalCard.transform.position;
                    animalModel.transform.position = new Vector3(animalCardPosition.x, animalCardPosition.y - 20.0f, animalCardPosition.z);
					animalModel.transform.localScale = AnimalScales [animalSpecies];
					animalModel.transform.localRotation = Quaternion.Euler(
						AnimalRotations [animalSpecies].x, AnimalRotations[animalSpecies].y, AnimalRotations[animalSpecies].z);
                    animalModels.Add(animalModel);

                    int _i = i;
                    animalCard.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        Dictionary<string, object> eventDict = new Dictionary<string, object>()
                        {
                            { AnimalInformationController.ANIMAL, ownedAnimals[_i] },
                            { AnimalInformationController.CALLING_SCREEN, ScreenType.AnimalUnderObs }
                        };
                        EventManager.TriggerEvent(GameEvent.SwitchScreen, ScreenType.Caught);
                        EventManager.TriggerEvent(GameEvent.ViewingAnimalInformation, eventDict);
                    });
                }
                else
                {
                    rows[row].gameObject.GetComponent<RowInAnimalGrid>().AnimalCards[col].enabled = false;
                }
            }
		}

		//TODO: Add the released animals to the cards
		//remember some owned animals were already added in front of some cards
		//One idea would be to make a list of both owned and released animals with a bool per animal saying if it is currently owned.
		//Then the loop would only need to be done once instead of twice
		/*if (Service.Request.Player().hasReleasedAnimal(animalSpecies))
		{
			List<Animal> releasedAnimals = Service.Request.Player().GetReleasedAnimals()[animalSpecies];
			totalAnimals += releasedAnimals.Count;
			foreach(Animal animal in releasedAnimals)
			{
				GameObject animalObject = AssetManager.GetAnimalClone(animalSpecies);
				//animalObject.transform.parent = ContentPanel.transform;
				//animalPrefabs.Add(animalObject);
			}
		}    */  
    }

	public void OnHide()
    {
		foreach (GameObject row in rows)
        {
			GameObject.Destroy(row);
        }
		rows.Clear();

		foreach (GameObject animal in animalModels) 
		{
			GameObject.Destroy (animal);
		}
		animalModels.Clear ();
        isAnimalSpeciesSet = false;
    }

	public void SetAnimalScales()
	{
		AnimalScales = new Dictionary<AnimalSpecies, Vector3> ();
		AnimalScales.Add (AnimalSpecies.Acorn, new Vector3 (3.0f, 3.0f, 3.0f));
		AnimalScales.Add (AnimalSpecies.Bat, new Vector3 (3.0f, 3.0f, 3.0f));
		AnimalScales.Add (AnimalSpecies.Butterfly, new Vector3 (20.0f, 20.0f, 20.0f));
		AnimalScales.Add (AnimalSpecies.Coyote, new Vector3 (15.0f, 15.0f, 15.0f));
		AnimalScales.Add(AnimalSpecies.Datura, new Vector3 (20.0f, 20.0f, 20.0f));
		AnimalScales.Add (AnimalSpecies.Death, new Vector3 (10.0f, 10.0f, 10.0f));
		AnimalScales.Add (AnimalSpecies.Deer, new Vector3 (17.0f, 17.0f, 17.0f));
		AnimalScales.Add (AnimalSpecies.Dolphin, new Vector3 (10.0f, 10.0f, 10.0f));
		AnimalScales.Add (AnimalSpecies.Dragonfly, new Vector3 (80.0f, 80.0f, 80.0f));
		AnimalScales.Add (AnimalSpecies.Earth, new Vector3 (10.0f, 10.0f, 10.0f));
		AnimalScales.Add (AnimalSpecies.Heron, new Vector3 (15.0f, 15.0f, 15.0f));
		AnimalScales.Add (AnimalSpecies.Lizard, new Vector3 (45.0f, 45.0f, 45.0f));
		AnimalScales.Add (AnimalSpecies.Mountainlion, new Vector3 (10.0f, 10.0f, 10.0f));
		AnimalScales.Add (AnimalSpecies.Rabbit, new Vector3 (20.0f, 20.0f, 20.0f));
		AnimalScales.Add (AnimalSpecies.Rain, new Vector3 (3.0f, 3.0f, 3.0f));
		AnimalScales.Add (AnimalSpecies.Rattlesnake, new Vector3 (10.0f, 10.0f, 10.0f));
		AnimalScales.Add (AnimalSpecies.Redtailedhawk, new Vector3 (20.0f, 20.0f, 20.0f));
		AnimalScales.Add (AnimalSpecies.Shark, new Vector3 (20.0f, 20.0f, 20.0f));
		AnimalScales.Add (AnimalSpecies.Squirrel, new Vector3 (20.0f, 20.0f, 20.0f));
		AnimalScales.Add (AnimalSpecies.Water, new Vector3 (3.0f, 3.0f, 3.0f));
		AnimalScales.Add (AnimalSpecies.Wind, new Vector3 (10.0f, 10.0f, 10.0f));
	}

	public void SetAnimalRotations()
	{
		AnimalRotations = new Dictionary<AnimalSpecies, Vector3> ();
		AnimalRotations.Add (AnimalSpecies.Acorn, new Vector3 (0.0f, 150.0f, 0.0f));
		AnimalRotations.Add (AnimalSpecies.Bat, new Vector3 (0.0f, 150.0f, 0.0f));
		AnimalRotations.Add (AnimalSpecies.Butterfly, new Vector3 (0.0f, 0.0f, 0.0f));
		AnimalRotations.Add (AnimalSpecies.Coyote, new Vector3 (13.0f, 180.0f, 0.0f));
		AnimalRotations.Add (AnimalSpecies.Datura, new Vector3 (0.0f, 0.0f, 0.0f));
		AnimalRotations.Add (AnimalSpecies.Death, new Vector3 (12.0f, 180.0f, 0.0f));
		AnimalRotations.Add (AnimalSpecies.Deer, new Vector3 (12.0f, 180.0f, 0.0f));
		AnimalRotations.Add (AnimalSpecies.Dolphin, new Vector3 (12.0f, 180.0f, 0.0f));
		AnimalRotations.Add (AnimalSpecies.Dragonfly, new Vector3 (12.0f, 180.0f, 0.0f));
		AnimalRotations.Add (AnimalSpecies.Earth, new Vector3 (12.0f, 180.0f, 0.0f));
		AnimalRotations.Add (AnimalSpecies.Heron, new Vector3 (0.0f, 200.0f, 0.0f));
		AnimalRotations.Add (AnimalSpecies.Lizard, new Vector3 (15.0f, 180.0f, 0.0f));
		AnimalRotations.Add (AnimalSpecies.Mountainlion, new Vector3 (12.0f, 180.0f, 0.0f));
		AnimalRotations.Add (AnimalSpecies.Rabbit, new Vector3 (12.0f, 180.0f, 0.0f));
		AnimalRotations.Add (AnimalSpecies.Rain, new Vector3 (0.0f, 150.0f, 0.0f));
		AnimalRotations.Add (AnimalSpecies.Rattlesnake, new Vector3 (12.0f, 180.0f, 0.0f));
		AnimalRotations.Add (AnimalSpecies.Redtailedhawk, new Vector3 (12.0f, 180.0f, 0.0f));
		AnimalRotations.Add (AnimalSpecies.Shark, new Vector3 (17.0f, 180.0f, 0.0f));
		AnimalRotations.Add (AnimalSpecies.Squirrel, new Vector3 (0.0f, 270.0f, 0.0f));
		AnimalRotations.Add (AnimalSpecies.Water, new Vector3 (0.0f, 150.0f, 0.0f));
		AnimalRotations.Add (AnimalSpecies.Wind, new Vector3 (12.0f, 180.0f, 0.0f));
	}
}
