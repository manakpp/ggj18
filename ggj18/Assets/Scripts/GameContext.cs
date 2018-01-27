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

	#region context
	private Dictionary<System.Type, MonoBehaviour> m_contextObjects = new Dictionary<System.Type, MonoBehaviour>();

	public static T Get<T>() where T: MonoBehaviour
	{
		if (Instance.m_contextObjects.ContainsKey (typeof(T))) 
		{
			return (T)Instance.m_contextObjects[typeof(T)];
		}
		return null;
	}

	public static void Add(MonoBehaviour behaviour)
	{
		Instance.m_contextObjects[behaviour.GetType()] = behaviour;
	}

	public static void Remove(MonoBehaviour behaviour)
	{
		if (Instance.m_contextObjects.ContainsKey (behaviour.GetType ())) 
		{
			Instance.m_contextObjects.Remove (behaviour.GetType());
		}
	}
	#endregion

	[SerializeField]
	private Character m_characterPrefab;

	private List<Character> m_characters = new List<Character>();

	private float m_timeElapsed = 0.0f;
	private float m_timeBetweenConversing = 1.0f;

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

		int numCharacters = 20;
		for(int i = 0; i < numCharacters; ++i)
		{
			Character character = GameObject.Instantiate<Character>(m_characterPrefab);
			m_characters.Add(character);

			float x = Random.Range(-5.0f, 5.0f);
			float y = Random.Range(-5.0f, 5.0f);
			character.transform.position = new Vector3(x, y, y);

			character.transform.SetParent(charParent.transform, false);
		}
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
}
