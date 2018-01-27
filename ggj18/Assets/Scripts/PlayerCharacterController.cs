using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : Character 
{
	private ShapeType m_shapeType = ShapeType.None;
	private int m_controllerIndex = 0;

	protected override void Awake()
	{
		base.Awake ();
	}

	public void InitialiseController(int controllerIndex)
	{
		m_controllerIndex = controllerIndex;
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
			m_shapeType = ShapeType.Cross;
			PushShape (m_shapeType);
		}
		else if(device.GetButtonUp(MappedButton.Button2))
		{
			m_shapeType = ShapeType.Circle;
			PushShape (m_shapeType);
		}
		else if(device.GetButtonUp(MappedButton.Button3))
		{
			m_shapeType = ShapeType.Square;
			PushShape (m_shapeType);
		}
		else if(device.GetButtonUp(MappedButton.Button4))
		{
			m_shapeType = ShapeType.Triangle;
			PushShape (m_shapeType);
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

	public override void MoveToTargetPosition(Vector3 position)
	{
	}
}
