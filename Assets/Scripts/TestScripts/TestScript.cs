using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {

	/* Beginning of placeholder stuff
	public Text tempInformationFootnote;
	public Slider maxVeloSlider;
	public Slider accSlider;
	public Slider obsDiffSlider;
	public Slider eneDiffSlider;
	public Dropdown cameraFormat;
	public Dropdown levelType;
	public Dropdown spellFormat;
	public GameObject cameraOrthoGO;
	public GameObject cameraPersGO;
	public GameObject cameraDynaGO;
	public GameObject spellLocatorFree;
	public GameObject spellLocatorBound;
	*/

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	void TotalShitPlaceholder (){
		/*
		// Temporary code while testing\
		// Changes up the camera depending on what has been selected
		String tempCamera = "";
		if (cameraFormat.value == 0) {
			cameraOrthoGO.SetActive (true);
			tempCamera = "Orthographic";
		}
		else if (cameraFormat.value == 1) {
			cameraOrthoGO.SetActive (false);
			cameraPersGO.SetActive (true);
			tempCamera = "Perspective";
		}
		else if (cameraFormat.value == 2) {
			cameraOrthoGO.SetActive (false);
			cameraDynaGO.SetActive (true);
			tempCamera = "Perspective Dynamic";
		}
		// Deals with the level type and the prefabs used in it
		String tempLevelType = "Dynamic";
		if (levelType.value == 0) {
			terrainPrefabList.RemoveAt (3);
			terrainPrefabList.RemoveAt (2);
			tempLevelType = "Flat";
		}
		if (levelType.value == 1) {
			terrainPrefabList.RemoveAt (3);
			tempLevelType = "Downhill";
		}
		// Handles the spell activation
		String tempSpell = "";
		if (spellFormat.value == 0){
			spellLocatorFree.SetActive (true);
			tempSpell = "Free";
		}
		if (spellFormat.value == 1) {
			spellLocatorBound.SetActive (true);
			tempSpell = "Bound";
		}


		// Handling of all the sliders
		acceleration = Mathf.RoundToInt (accSlider.value);
		maxVelocity = Mathf.RoundToInt (maxVeloSlider.value);
		obstacleDifficulty = Mathf.RoundToInt (obsDiffSlider.value);
		enemyDifficulty = Mathf.RoundToInt (eneDiffSlider.value);



		tempInformationFootnote.text = "ObstDiff: " +obstacleDifficulty + "  EnemyDiff: " 
			+ enemyDifficulty + "  MaxVelocity: " + maxVelocity + "  Acceleration: " + acceleration
			+ "  Level Format: " + tempLevelType + "  Camera: " + tempCamera + "  Spell: " + tempSpell;
		// End of test code
		*/

		// Have to make the text variable public, get it as a gameObject, then set it active.
	}


}
