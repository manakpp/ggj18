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

	[SerializeField]
	private ZOrderUpdater m_zOrderUpdater;

	private Metronome m_metronome;

	private List<Character> m_characters = new List<Character>();
	private List<PlayerCharacterController> m_playerCharacters = new List<PlayerCharacterController>();

	private float m_timeElapsed = 0.0f;
	private float m_timeBetweenConversing = 1.0f;

	private float m_mingleTimeElapsed = 4.5f;
	private float m_timeBetweenMingling = 5.0f;

	private float m_timeSinceLevelStarted = 0.0f;

	private bool m_worldCreated = false;

	public GameConfig Config {get{return m_config; }}
	public InputManager InputManager {get{return m_inputManager; }}
	public float TimeLimit { get{ return m_config.Scene.TimeLimit; }}
	public float TimeRemaining { get{ return Mathf.Max(0.0f, m_config.Scene.TimeLimit - m_timeSinceLevelStarted); }}
	public float TimeRemainingRatio { get { return TimeRemaining / TimeLimit; }}
	public List<PlayerCharacterController> Players{ get{ return m_playerCharacters; }}

	private void Awake()
	{
		if (s_context != null && s_isInitialised)
		{
			GameObject.Destroy(gameObject);
			return;
		}
		s_context = this;
			
		m_inputManager = GameObject.Instantiate<InputManager> (m_inputManager);
		m_inputManager.transform.SetParent (this.transform);

		m_metronome = FindObjectOfType<Metronome> ();
		m_metronome.TickEvent += MetronomeTick;

		CreatePlayers ();
	}

	private void CreateWorld()
	{
		m_zOrderUpdater = GameObject.Instantiate<ZOrderUpdater>(m_zOrderUpdater);
		m_zOrderUpdater.transform.SetParent (this.transform);

		// Generate characters
		GameObject charParent = new GameObject("Characters");
		charParent.AddComponent<ZOrderUpdater>();

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

		for(int i = 0; i < m_playerCharacters.Count; ++i)
			m_playerCharacters[i].gameObject.SetActive (true);
	}

	public void CreatePlayers()
	{
		List<ShapeType> randomShapes = new List<ShapeType> ();
		for (int i = 0; i < (int)ShapeType.MAX; ++i) {
			ShapeType type = (ShapeType)i;
			if (type != ShapeType.None && type != ShapeType.MAX) {
				randomShapes.Add (type);
			}
		}

		GameObject charParent = new GameObject("PCharacters");

		int numCharacters = 2;
		for(int i = 0; i < numCharacters; ++i)
		{
			PlayerCharacterController character = GameObject.Instantiate<PlayerCharacterController>(m_playerPrefab);
			m_characters.Add(character);
			m_playerCharacters.Add (character);

			float x = Random.Range(-5.0f, 5.0f);
			float y = Random.Range(-5.0f, 5.0f);
			character.transform.position = new Vector3(x, y, 0.0f);

			character.transform.SetParent(charParent.transform, false);

			int randIndex = Random.Range (0, randomShapes.Count);
			ShapeType randShapeA = randomShapes [randIndex];
			randomShapes.RemoveAt (randIndex);
			randIndex = Random.Range (0, randomShapes.Count);
			ShapeType randShapeB = randomShapes [randIndex];
			randomShapes.RemoveAt (randIndex);

			character.InitialisePlayer (i, randShapeA, randShapeB);

			character.gameObject.SetActive (false);

		}
	}

	private void Start()
	{
		m_timeBetweenConversing = Config.Character.TimeBetweenConversing;
	}

	private void OnDestroy()
	{
		m_metronome.TickEvent -= MetronomeTick;
	}

	private void Update()
	{
		if (StateManager.gameState != (int)StateManager.GameState.IN_GAME)
			return;

		if (!m_worldCreated) 
		{
			CreateWorld ();
			m_worldCreated = true;
			return;
		}

		m_mingleTimeElapsed += Time.deltaTime;
		if (m_mingleTimeElapsed >= m_timeBetweenMingling) 
		{
			m_mingleTimeElapsed = 0.0f;

			Mingle();
		}

		m_timeSinceLevelStarted += Time.deltaTime;
		if (m_timeSinceLevelStarted >= Config.Scene.TimeLimit) 
		{
            // GameOver
			FindObjectOfType<UIManager>().GameOver();

			for (int i = 0; i < m_characters.Count; ++i) {
				m_characters [i].gameObject.SetActive (false);
			}
        }
	}

	private void MetronomeTick()
	{
		if (StateManager.gameState != (int)StateManager.GameState.IN_GAME)
			return;
		
		Converse();
	}

	private void Converse()
	{
		for(int i = 0; i < m_characters.Count; ++i)
		{
			var character = m_characters [i];
			float rollToChanceIdea = Random.Range (0.0f, 100.0f);
			if (rollToChanceIdea < Config.Character.ChanceToChangeIdea) 
			{
				continue;
			}

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

		for (int i = 0; i < m_playerCharacters.Count; ++i) 
		{
			m_playerCharacters [i].shapeCount1 = 1;
			m_playerCharacters [i].shapeCount2 = 1;
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

			if (character is PlayerCharacterController) 
			{
				PlayerCharacterController pc = character as PlayerCharacterController;
				var shapes = character.Shapes;

				shapeCount [(int)shapes [0]] += pc.shapeCount1;
				shapeCount [(int)shapes [1]] += pc.shapeCount2;
			}
			else
			{
				var shapes = character.Shapes;
				for (int j = 0; j < shapes.Count; ++j) 
				{
					shapeCount [(int)shapes [j]]++;
				}
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

	public float CalculateInfluenceRatio()
	{
		ShapeType a = m_playerCharacters [0].PrimaryShape;
		ShapeType b = m_playerCharacters [1].PrimaryShape;

		List<int> sumShapes = new List<int> ();
		for (int i = 0; i < (int)ShapeType.MAX; ++i) {
			sumShapes.Add (0);
		}

		for (int i = 0; i < m_characters.Count; ++i) {
			sumShapes [(int)m_characters [i].Shapes [0]]++;
			sumShapes [(int)m_characters [i].Shapes [1]]++;
		}

		int sum = sumShapes [(int)a] + sumShapes [(int)b];
		float playerRatio = (float)sumShapes [(int)a] / (float)sum;

		return playerRatio;
	}

	public List<ShapeType> GetTopShapes()
	{
		return null;
	}
}
