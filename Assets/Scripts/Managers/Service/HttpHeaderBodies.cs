﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HttpHeaderBodies
{
	public class AccountDetails
	{
		public string username;
		public string name;
		public string password;
		public string email;
		public string gender;
		public string birthdate;

		public AccountDetails(string account_username, string account_name, string account_password, string account_email, string account_gender, string account_birthdate)
		{
			username = account_username;
			name = account_name;
			password = account_password;
			email = account_email;
			gender = account_gender;
			birthdate = account_birthdate;
		}
	}

	public class LoginDetails
	{
		public string username;
		public string password;

		public LoginDetails(string account_username, string account_password)
		{
			username = account_username;
			password = account_password;
		}
	}

	public class AnimalEncounterData
	{
		public string session_key;
		public string encounter_type;
		public string species;
		public int encounter_id;
		public string nickname;
		public float age;
		public float height;
		public float weight;
		public float health1;
		public float health2;
		public float health3;

		public AnimalEncounterData()
		{
			age = -1.0f;
			height =  1.0f;
			weight = -1.0f;
			health1 = -1.0f;
			health2 = -1.0f;
			health3 = -1.0f;
		}
	}

	[System.Serializable]
	public class InterestData
	{
		public string interest;
		public int value;

		public InterestData(string interestData, int valueData)
		{
			interest = interestData;
			value = valueData;
		}
	}

	[System.Serializable]
	public class SurveyData
	{
		public string session_key;
		public List<InterestData> interests;
	}
}
