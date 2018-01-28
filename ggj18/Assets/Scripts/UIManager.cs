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
	public GameObject UIAnimatingPlayer1;
	public GameObject UIAnimatingPlayer2;
	public GameObject levelGameObject;
	public GameObject panelClockGameObject;
    public GameObject player1Panel;
    public GameObject player2Panel;
    public GameObject alienSounds;

	public bool player1wins;
    public GameObject crossFaderObj;
    private CrossFader crossFader;
    private AudioSource audioSource;
    private bool isShowingEndScreen = false;
    private double gameOverStartTime = 0.0;
    private int previousCount = 0;

	void Start () {
        crossFader = crossFaderObj.GetComponent<CrossFader>();
        audioSource = GetComponent<AudioSource>();
        countdownText = countdownTextObject.GetComponent<Text>();
        countdownTextObject.SetActive(false);
        countdownTextGameObject.SetActive(false);
		UIAnimatingPlayer1.SetActive(true);
		UIAnimatingPlayer2.SetActive(true);
        player1Panel.SetActive(false);
        player2Panel.SetActive(false);
        
        StateManager.gameState = (int)StateManager.GameState.START_UI;
        crossFader.CreateFade("Master", 0.0f, 0.001f);
    }
	
	void Update () {
		if(StateManager.gameState == (int)StateManager.GameState.START_UI &&
			Input.GetKeyDown(KeyCode.Space) )
        {
            StartCountdown();
			return;
        }
		if(StateManager.gameState == (int)StateManager.GameState.IN_GAME &&
			Input.GetKeyDown(KeyCode.G))
        {
			GameOver ();
			return;
        }

        if(StateManager.gameState == (int)StateManager.GameState.GAME_OVER && Time.time > gameOverStartTime + 1.0)
        {
            if(Input.anyKeyDown)
            {
                Invoke("loadScene", 0.5f);
                crossFader.CreateFade("Master", -80.0f, 0.5f);
            }
        }

        if(GameContext.Instance.TimeRemaining < 5)
        {
            if(previousCount != (int)GameContext.Instance.TimeRemaining)
            {
                audioSource.Play();
            }
            previousCount = (int)GameContext.Instance.TimeRemaining;
        }
	}

    private void loadScene() { UnityEngine.SceneManagement.SceneManager.LoadScene("Olivier"); }

	public void StartCountdown() {
        countdownTextObject.SetActive(true);
        player1Panel.SetActive(true);
        player2Panel.SetActive(true);
        audioSource.Play();

		float secsPerTick = GameContext.Instance.Config.Scene.SecondsPerTick;
		InvokeRepeating("Countdown", secsPerTick, secsPerTick);
	}

	void Countdown()
	{
        if (countdownFrom == 2)
        {
            //fade in
			float secsPerTick = GameContext.Instance.Config.Scene.SecondsPerTick;
			crossFader.CreateFade("Melody1", 0.0f, secsPerTick);
			crossFader.CreateFade("SFX", -2.5f, secsPerTick);
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
		UIAnimatingPlayer1.SetActive(false);
		UIAnimatingPlayer2.SetActive(false);
		panelClockGameObject.SetActive(true);
        levelGameObject.SetActive(true);
        StateManager.gameState = (int)StateManager.GameState.IN_GAME;
    }

    void displayEndScreen()
    {
        isShowingEndScreen = true;
        planelGameOverGameObject.SetActive(true);
        levelGameObject.SetActive(false);
		panelClockGameObject.SetActive(false);
		crossFader.CreateFade("Melody1", -40.0f, 2.0f);
        crossFader.CreateFade("SFX", -40.0f, 2.0f);
		if (player1wins)
		{
			UIAnimatingPlayer1.SetActive(true);
            alienSounds.GetComponent<AudioSource>().panStereo = -0.7f;
            alienSounds.GetComponent<AudioSource>().Play();
            
        } else
		{
			UIAnimatingPlayer2.SetActive(true);
            alienSounds.GetComponent<AudioSource>().panStereo = 0.7f;
            alienSounds.GetComponent<AudioSource>().Play();
           
        }
    }

	public void GameOver()
	{
		GameContext.Instance.HideCharacters ();
		player1wins = GameContext.Instance.CalculateInfluenceRatio () >= 0.5f;
		StateManager.gameState = (int)StateManager.GameState.GAME_OVER;
		gameOverStartTime = Time.time;
        

        if (!isShowingEndScreen) displayEndScreen();
	}
}
