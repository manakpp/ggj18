using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public int countdownFrom;
	public GameObject countdownTextObject;
    private Text countdownText;
	public GameObject panelMainMenuGameObject;
	public GameObject planelGameOverGameObject;
	public GameObject countdownTextGameObject;
	public GameObject mainmenuAnimationsGameObject;
	public GameObject levelGameObject;

    public GameObject crossFaderObj;
    private CrossFader crossFader;
    private AudioSource audioSource;
    private bool isShowingEndScreen = false;
    private double gameOverStartTime = 0.0;

	void Start () {
        crossFader = crossFaderObj.GetComponent<CrossFader>();
        audioSource = GetComponent<AudioSource>();
        countdownText = countdownTextObject.GetComponent<Text>();
        countdownTextObject.SetActive(false);
        countdownTextGameObject.SetActive(false);
        mainmenuAnimationsGameObject.SetActive(true);
        StateManager.gameState = (int)StateManager.GameState.START_UI;
	}
	
	void Update () {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCountdown();
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            StateManager.gameState = (int)StateManager.GameState.GAME_OVER;
            gameOverStartTime = Time.time;
            if (!isShowingEndScreen) displayEndScreen();
        }

        if(StateManager.gameState == (int)StateManager.GameState.GAME_OVER && Time.time > gameOverStartTime + 1.0)
        {
            if(Input.anyKeyDown)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Chris");
            }
        }
	}

	public void StartCountdown() {
        countdownTextObject.SetActive(true);
        audioSource.Play();
        InvokeRepeating("Countdown", 1.0f, 1.0f);
	}

	void Countdown()
	{
        if (countdownFrom == 2)
        {
            //fade in
            crossFader.CreateFade("Melody1", 0.0f, 1.0f);
            crossFader.CreateFade("SFX", -2.5f, 1.0f);
        }
		if (--countdownFrom == 0) {
            CancelInvoke("Countdown");
            startPlaying();
        }
        else audioSource.Play();
        countdownText.text = countdownFrom.ToString();
	} 

    void startPlaying()
    {
        panelMainMenuGameObject.SetActive(false);
        mainmenuAnimationsGameObject.SetActive(false);
        levelGameObject.SetActive(true);
        StateManager.gameState = (int)StateManager.GameState.IN_GAME;
    }

    void displayEndScreen()
    {
        isShowingEndScreen = true;
        planelGameOverGameObject.SetActive(true);
        levelGameObject.SetActive(false);
        crossFader.CreateFade("Melody1", -40.0f, 2.0f);
        crossFader.CreateFade("SFX", -40.0f, 2.0f);
    }
}
