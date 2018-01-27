using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour 
{
	private void Update()
	{
		GamepadDevice device = GameContext.Instance.InputManager.GetDevice(0);
		if (device == null)
			return;

		if(device.GetButtonUp(GamepadButton.Action1))
		{
			// Change thought
		}
		else if(device.GetButtonUp(GamepadButton.Action2))
		{
			// Change thought
		}
		else if(device.GetButtonUp(GamepadButton.Action3))
		{
			// Change thought
		}
		else if(device.GetButtonUp(GamepadButton.Action4))
		{
			// Change thought
		}

		float moveX = device.GetAxis (GamepadAxis.LeftStickX);
		float moveY = device.GetAxis (GamepadAxis.LeftStickY);
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
}
