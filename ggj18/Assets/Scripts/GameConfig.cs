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
		public float ChanceToLeaveConversation = 0.3f;
		public int MaxCharactersMovingBetweenConversations = 10;

		public float PlayerMaxVelocity = 10.0f;
		public float PlayerAcceleration = 10.0f;
		public float PlayerDeceleration = 10.0f;
	}

	public SceneConfig Scene;
	public CharacterConfig Character;
}
