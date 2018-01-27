using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour 
{
	private GamepadInput m_gamepadInput;

	public GamepadInput Input { get { return m_gamepadInput; } }

	private void Awake()
	{
		m_gamepadInput = GetComponent<GamepadInput>();
	}

	public GamepadDevice GetDevice(int index)
	{
		if (m_gamepadInput.gamepads == null ||
		   index <= m_gamepadInput.gamepads.Count)
			return null;

		return m_gamepadInput.gamepads [index];
	}
}
