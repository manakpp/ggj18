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

	public GamepadInputDevice GetDevice(int index)
	{
		var devices = MappedInput.inputDevices;

		// I think there is an issue with IDs when rebinded, do this incase of weird windows stuff
		// If there are more than 2, first in list goes to player 1, second to player 2.
		for (int i = 0; i < devices.Count; ++i) 
		{
			GamepadInputDevice device = devices [i] as GamepadInputDevice;
			if (device != null) 
			{
				if (index == 0)
					return device;
				else
					index--;
			}
		}

		return null;
	}

	public KeyboardInputDevice GetKeyboard()
	{
		KeyboardInputDevice keyboard = null;
		var devices = MappedInput.inputDevices;
		for (int i = 0; i < devices.Count; ++i) 
		{
			if (devices [i] is KeyboardInputDevice) 
			{
				keyboard = devices [i] as KeyboardInputDevice;
				return keyboard;
			}
		}

		return null;
	}
}
