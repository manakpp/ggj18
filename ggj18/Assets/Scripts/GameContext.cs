using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContext : MonoBehaviour 
{
	#region singletone
	private static GameContext s_context;
	private static bool s_isInitialised;
	public static GameContext Instance 
	{
		get 
		{
			if (s_context == null) 
			{
				s_context = FindObjectOfType<GameContext>();
			}
			return s_context;
		}
	}
	#endregion

	[SerializeField]
	private PlayerCharacterController m_playerPrefab;

	[SerializeField]
	private Character m_characterPrefab;

	[SerializeField]
	private GameConfig m_config;

	[SerializeField]
	private InputManager m_inputManager;

	private List<Character> m_characters = new List<Character>();

	private float m_timeElapsed = 0.0f;
	private float m_timeBetweenConversing = 1.0f;

	private float m_mingleTimeElapsed = 4.5f;
	private float m_timeBetweenMingling = 5.0f;

	public GameConfig Config {get{return m_config; }}
	public InputManager InputManager {get{return m_inputManager; }}

	private void Awake()
	{
		if (s_context != null && s_isInitialised)
		{
			GameObject.Destroy(gameObject);
			return;
		}
		s_context = this;

		// Generate characters
		GameObject charParent = new GameObject("Characters");

		int numCharacters = Config.Scene.NumCharacters;
		for(int i = 0; i < numCharacters; ++i)
		{
			Character character = GameObject.Instantiate<Character>(m_characterPrefab);
			m_characters.Add(character);

			float x = Random.Range(-Config.Scene.BoundsX, Config.Scene.BoundsX);
			float y = Random.Range(-Config.Scene.BoundsY, Config.Scene.BoundsY);
			character.transform.position = new Vector3(x, y, 0.0f);

			character.transform.SetParent(charParent.transform, false);
		}

		numCharacters = 2;
		for(int i = 0; i < numCharacters; ++i)
		{
			PlayerCharacterController character = GameObject.Instantiate<PlayerCharacterController>(m_playerPrefab);
			m_characters.Add(character);

			float x = Random.Range(-5.0f, 5.0f);
			float y = Random.Range(-5.0f, 5.0f);
			character.transform.position = new Vector3(x, y, 0.0f);

			character.transform.SetParent(charParent.transform, false);
		}

		m_inputManager = GameObject.Instantiate<InputManager> (m_inputManager);
		m_inputManager.transform.SetParent (this.transform);


	}

	private void Start()
	{
		m_timeBetweenConversing = Config.Character.TimeBetweenConversing;
	}

	private void Update()
	{
		// Iterate through all characters and tick
		m_timeElapsed += Time.deltaTime;
		if(m_timeElapsed >= m_timeBetweenConversing)
		{
			m_timeElapsed = 0.0f;

			Converse();
		}

		m_mingleTimeElapsed += Time.deltaTime;
		if (m_mingleTimeElapsed >= m_timeBetweenMingling) 
		{
			m_mingleTimeElapsed = 0.0f;

			Mingle();
		}
	}

	private void Converse()
	{
		for(int i = 0; i < m_characters.Count; ++i)
		{
			var character = m_characters [i];

			// Get characters in area
			var othersAroundMe = GetOthersAroundCharacter(character);

			// For each group evaluate the a the outcoming shape
			var chosenShape = EvaluateShapeFromGroup(othersAroundMe);
			if (chosenShape == ShapeType.None ||
			   chosenShape == ShapeType.MAX) 
			{
				// Can't choose these types
				continue;
			}

			// Push shape
			character.PushShape(chosenShape);
		}
	}

	private List<Character> GetOthersAroundCharacter(Character character)
	{
		List<Character> nearbyCharacters = new List<Character>();

		Vector3 position = character.transform.position;

		for (int i = 0; i < m_characters.Count; ++i) 
		{
			Character other = m_characters[i];
			if (character == other)
				continue; // Don't care about self

			Vector3 otherPosition = other.transform.position;
			float distance = Vector3.Distance(position, otherPosition);
			if (distance < character.HearingRadius) // TODO: Overlap circles
			{
				nearbyCharacters.Add(other);
			}
		}

		return nearbyCharacters;
	}

	private ShapeType EvaluateShapeFromGroup(List<Character> characters)
	{
		int[] shapeCount = new int[(int)ShapeType.MAX];

		for (int i = 0; i < characters.Count; ++i) 
		{
			Character character = characters[i];
			var shapes = character.Shapes;
			for (int j = 0; j < shapes.Count; ++j) 
			{
				shapeCount[(int)shapes[j]]++;
			}
		}

		// Weighted random
		int totalWeight = 0;
		for (int i = 0; i < shapeCount.Length; ++i) 
		{
			totalWeight += shapeCount[i];
		}

		int randValue = Random.Range (0, totalWeight + 1); // Because exclusive 
		int total = 0;
		for (int i = 0; i < shapeCount.Length; i++) 
		{
			total += shapeCount[i];
			if (total >= randValue) 
			{
				ShapeType shape = (ShapeType)i;
				return shape;
			}
		}

		return ShapeType.None;
	}

	private void Mingle()
	{
		int groups = Random.Range (5, 10);

		Shuffle<Character>(m_characters);

		List<Vector3> groupPositions = new List<Vector3>();
		for (int i = 0; i < groups; ++i) 
		{
			float x = Random.Range(-Config.Scene.BoundsX, Config.Scene.BoundsX);
			float y = Random.Range(-Config.Scene.BoundsY, Config.Scene.BoundsY);
			groupPositions.Add(new Vector3(x, y, y));
		}

		// Assign characters to groups
		int movedCount = 0;
		for (int i = 0; i < m_characters.Count; ++i) 
		{
			float scalingChance = m_characters [i].MissedMoveCount * 10.0f;
			float chance = Config.Character.ChanceToLeaveConversation * 100 + scalingChance;
			float roll = Random.Range(0.0f, 100.0f);
			if (roll >= chance) 
			{
				m_characters [i].MoveToTargetPosition (groupPositions [Random.Range (0, groups)]);
				++movedCount;
				m_characters [i].MissedMoveCount = 0;
			}

			if (movedCount >= Config.Character.MaxCharactersMovingBetweenConversations)
				break;
		}
	}

	public static void Shuffle<T>(IList<T> list) 
	{  
		System.Random rnd = new System.Random();
		int n = list.Count;  
		while (n > 1) 
		{  
			n--;  
			int k = rnd.Next(n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}
}
