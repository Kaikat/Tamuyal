﻿using UnityEngine;
using UnityEngine.UI;

public class CaughtUIObject : MonoBehaviour {
	
	public RawImage AnimalImage;
	public Text AnimalNameTitle;
	public Text AnimalDescription;

	public Text HealthFactor1;
	public Text HealthFactor2;
	public Text HealthFactor3;

	void Awake()
	{
		Event.Request.RegisterEvent <Animal> (GameEvent.AnimalCaught, SetTextFields);
	}

	void SetTextFields(Animal animal)
	{
		string animalName = Service.Request.AnimalEnglishName (animal.Species);
		animalName += "\n" + Service.Request.AnimalSpanishName (animal.Species);
		animalName += "\n" + Service.Request.AnimalNahuatlName (animal.Species);

		AnimalImage.texture = Resources.Load<Texture> (UIConstants.ANIMAL_IMAGE_PATH + animal.Species.ToString());
		AnimalNameTitle.text = animalName;
		AnimalDescription.text = Service.Request.AnimalDescription (animal.Species);

		HealthFactor1.text = animal.Stats.Health1.ToString ();
		HealthFactor2.text = animal.Stats.Health2.ToString ();
		HealthFactor3.text = animal.Stats.Health3.ToString ();
	}

	void Destroy()
	{
		Event.Request.UnregisterEvent <Animal> (GameEvent.AnimalCaught, SetTextFields);
	}
}
