using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : MonoBehaviour 
{
	[System.Serializable]
	public class SceneConfig
	{
		public int NumCharacters = 20;
	}

	[System.Serializable]
	public class CharacterConfig
	{
		public bool DrawCircles = true;
		public float TimeBetweenConversing = 1.0f;
		public float HearingRadius = 5.0f;
		public float TalkingRaidus = 5.0f;
	}

	public SceneConfig Scene;
	public CharacterConfig Character;
}
