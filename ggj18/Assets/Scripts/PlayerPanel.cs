using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerPanel : MonoBehaviour 
{
	public int playerIndex = 0;
	public Image imageA;
	public Image imageB;

	bool isInitialised;

	void Initialise()
	{
		isInitialised = true;

		var pc = GameContext.Instance.Players [playerIndex];
		imageA.sprite = pc.m_shapeSpriteDict[pc.Shapes[0]];
		imageB.sprite = pc.m_shapeSpriteDict[pc.Shapes[1]];
	}

	void Update ()
	{
		if (!isInitialised)
			Initialise ();
		

	}
}
