using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour 
{
	public Text m_timerText;

	void Update ()
	{
		if (StateManager.gameState == (int)StateManager.GameState.IN_GAME)
			m_timerText.text = Mathf.CeilToInt (GameContext.Instance.TimeRemaining).ToString();
		else if (StateManager.gameState == (int)StateManager.GameState.START_UI)
			m_timerText.text = Mathf.CeilToInt (GameContext.Instance.Config.Scene.TimeLimit).ToString();
		else
			m_timerText.text = "0";
	}
}
