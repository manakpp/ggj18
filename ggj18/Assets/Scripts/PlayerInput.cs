using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour 
{
	private PlayerCharacterController m_controller;
	private int m_playerIndex = 0;

	public void InitialisePlayer(PlayerCharacterController controller, int index)
	{
		m_controller = controller;
		m_playerIndex = 0;
	}

	private void Update()
	{
		GamepadDevice device = GameContext.Get<InputManager> ().GetDevice(m_playerIndex);
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
		if (moveX > deadzone || moveX < deadzone) 
		{
			// Move horizontal
		}
		if (moveY > deadzone || moveY < deadzone) 
		{
			// Move vertical
		}
	}
}
