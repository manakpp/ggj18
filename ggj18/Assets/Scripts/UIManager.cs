using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public int countdownFrom;
	public Text countdownText;
	

	// Use this for initialization
	void Start () {
		InvokeRepeating("Countdown", 1.0f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
		//countdownText.text = countdownFrom.ToString();
		//countdownFrom--;
	}

	//display Player 1 UI
	//display Player 2 UI
	//assign concepts
	//animate player 1
	//animate player 2


	void Play() {
		InvokeRepeating("Countdown", 1.0f, 1.0f);
	}

	void Countdown()
	{
			if (--countdownFrom == 0) CancelInvoke("Countdown");
			countdownText.text = countdownFrom.ToString();

			//countdownFrom--;
			//countdownText.text = countdownFrom.ToString();
		} 
	}
