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

    public GameObject crossFaderObj;
    private CrossFader crossFader;

	void Start () {
        crossFader = crossFaderObj.GetComponent<CrossFader>();
        StateManager.gameState = (int)StateManager.GameState.START_UI;
	}
	
	void Update () {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCountdown();
        }
	}


	public void StartCountdown() { //Play
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
        if(countdownFrom == 1)
        {
            //fade in
            crossFader.CreateFade("Melody1", 0.0f, 1.0f);
            crossFader.CreateFade("SFX", -2.5f, 1.0f);
        }
		if (--countdownFrom == 0) {
			Invoke("ClearMainMenu",1f);
			CancelInvoke("Countdown");
            StateManager.gameState = (int)StateManager.GameState.IN_GAME;
        }	
		countdownText.text = countdownFrom.ToString();
	} 
}
