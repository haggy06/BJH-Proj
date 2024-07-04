using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class MedicineQ : ScriptableObject
{
	public List<Sheet1> Sheet1; // Replace 'EntityType' to an actual type that is serializable.
}

[System.Serializable]
public class Sheet1
{
	public string question;
	public bool answer;
	public string wrongComment;
}