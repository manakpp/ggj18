﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : Character 
{
	public Animator wifiA;
	public Animator wifiB;

	private List<ShapeType> m_assignedShapes = new List<ShapeType> ();
	private int m_controllerIndex = 0;

	public List<GameObject> m_playerPrefabs = new List<GameObject> ();

	public ShapeType PrimaryShape { get{ return m_assignedShapes [0]; }}

	public int shapeCount1 = 0;
	public int shapeCount2 = 0;

	protected override void Awake()
	{
		base.Awake ();
	}

	public void InitialisePlayer(int controllerIndex, ShapeType type, ShapeType typeB)
	{
		m_assignedShapes.Add(type);
		m_assignedShapes.Add(typeB);
		m_shapes [0] = type;
		m_shapes [1] = typeB;
		m_controllerIndex = controllerIndex;

		m_playerPrefabs [controllerIndex].SetActive (true);
	}

	protected override void Update()
	{
		base.Update ();

		InputDevice device = GameContext.Instance.InputManager.GetDevice(m_controllerIndex);
		if (device == null) 
		{
			device = GameContext.Instance.InputManager.GetKeyboard ();
		}

		if(device.GetButtonUp(MappedButton.Button1))
		{
			if(wifiA != null && wifiA.isInitialized)
				wifiA.SetTrigger("play");
			if(wifiB != null && wifiB.isInitialized)
				wifiB.SetTrigger("play");
			
			shapeCount1++;
		}
		else if(device.GetButtonUp(MappedButton.Button2))
		{
			if(wifiA != null && wifiA.isInitialized)
				wifiA.SetTrigger("play");
			if(wifiB != null && wifiB.isInitialized)
				wifiB.SetTrigger("play");
			
			shapeCount2++;
		}

		float moveX = device.GetAxis (MappedAxis.Horizontal);
		float moveY = device.GetAxis (MappedAxis.Vertical);
		float deadzone = 0.01f;
		if ((moveX > deadzone || moveX < -deadzone) ||
		    (moveY > deadzone || moveY < -deadzone)) 
		{
			float acceleration = GameContext.Instance.Config.Character.PlayerAcceleration;
			float maxVelocity = GameContext.Instance.Config.Character.PlayerMaxVelocity;

			Rigidbody rb = GetComponent<Rigidbody> ();
			rb.velocity += new Vector3 (moveX, moveY, 0.0f) * acceleration * Time.deltaTime;

			if (rb.velocity.magnitude > maxVelocity) 
			{
				rb.velocity = rb.velocity.normalized * maxVelocity;
			}
		} 
		else 
		{
			// Drag
			float deceleration = GameContext.Instance.Config.Character.PlayerDeceleration;
			Rigidbody rb = GetComponent<Rigidbody>();
			rb.velocity = Vector3.MoveTowards (rb.velocity, Vector3.zero, Time.deltaTime * deceleration);
		}
	}

	public void ChangePlayerShapes(ShapeType shape)
	{
		// Only push if it doesn't already exist
		if (!m_shapes.Contains(shape)) 
		{
			m_shapes.RemoveAt(0);
			m_shapes.Add(shape);
		}
	}

	public override void PushShape(ShapeType shape)
	{
	}

	public override void MoveToTargetPosition(Vector3 position)
	{
	}
}
