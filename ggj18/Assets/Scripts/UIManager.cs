using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public int countdownFrom;
	public Text countdownText;
	public GameObject panelMainMenuGameObject;
	public GameObject planelGameOverGameObject;
	public GameObject countdownTextGameObject;
	public GameObject mainmenuAnimationsGameObject;
	public GameObject levelGameObject;
	

	// Use this for initialization
	void Start () {
		Play() ;
	}
	
	// Update is called once per frame
	void Update () {

	}

	//display Player 1 UI
	//display Player 2 UI
	//assign concepts
	//animate player 1
	//animate player 2


	void Play() { //Play
		InvokeRepeating("Countdown", 1.0f, 1.0f);

	}

	void ClearMainMenu()
	{
			//move off screen player 1 animation
			//move on screen player 2 animation
			countdownTextGameObject.SetActive(false);
			mainmenuAnimationsGameObject.SetActive(false);
			levelGameObject.SetActive(true);
			//add player 1 to scene
			//add player 2 to scene
		}

	void Countdown() //Countdown until 0
	{
		if (--countdownFrom == 0) {
			Invoke("ClearMainMenu",1f);
			CancelInvoke("Countdown");
		}	
		countdownText.text = countdownFrom.ToString();
	} 
}


