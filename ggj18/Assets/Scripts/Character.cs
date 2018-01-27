using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour 
{
	public const int MAX_THOUGHTS = 2;

	[SerializeField]
	private ArriveUnit m_arrive;

	// The thing in the first index will be the first to be removed (0 is the end)
	private List<ShapeType> m_shapes = new List<ShapeType>(MAX_THOUGHTS);

	private float m_hearingRadius;
	private float m_talkingRadius;

	public List<ShapeType> Shapes { get { return m_shapes; } }
	public float HearingRadius { get{ return m_hearingRadius; } }
	public float TalkingRadius { get{ return m_talkingRadius; } }

	private void Awake()
	{
		GameConfig config = GameContext.Get<GameConfig>();
		m_hearingRadius = config.Character.HearingRadius;
		m_talkingRadius = config.Character.TalkingRaidus;

		// Populate thoughts with default data
		for(int i = 0; i < MAX_THOUGHTS; ++i)
		{
			m_shapes.Add(ShapeType.None);
		}

		GenerateRandomThoughts();
	}

	private void Start()
	{
		m_arrive.targetPosition = transform.position;
	}

	private void OnDrawGizmos()
	{
		if (!Application.isPlaying)
			return;
		
		GameConfig config = GameContext.Get<GameConfig>();
		if (config.Character.DrawCircles) 
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere (transform.position, m_hearingRadius);
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere (transform.position, m_talkingRadius);
		}
	}

	private void Update()
	{
		// TODO: Replace with sprite UI stuff
		// Update UI
		ShapeType shape = m_shapes[0];
		switch (shape) 
		{
		case ShapeType.Square:
			GetComponentInChildren<SpriteRenderer> ().material.color = new Color (0.75f, 0.40f, 0.40f);
			break;
		case ShapeType.Triangle:
			GetComponentInChildren<SpriteRenderer> ().material.color = Color.green;
			break;
		case ShapeType.Cross:
			GetComponentInChildren<SpriteRenderer> ().material.color = Color.blue;
			break;
		case ShapeType.Circle:
			GetComponentInChildren<SpriteRenderer> ().material.color = Color.red;
			break;

		case ShapeType.None: // Fall
		case ShapeType.MAX: // Fall
		default:
			break;
		}
	}

	public void GenerateRandomThoughts()
	{
		for(int i = 0; i < m_shapes.Count; ++i)
		{
			m_shapes[i] = (ShapeType)Random.Range((int)ShapeType.None + 1, (int)ShapeType.MAX);
		}
	}

	public void PushShape(ShapeType shape)
	{
		// Only push if it doesn't already exist
		if (!m_shapes.Contains(shape)) 
		{
			m_shapes.RemoveAt(0);
			m_shapes.Add(shape);
		}
	}

	public void MoveToTargetPosition(Vector3 position)
	{
		m_arrive.targetPosition = position;
	}
}
