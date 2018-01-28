using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour 
{
	public Image imageA;
	public Image imageB;
	public Image imageC;

	private bool m_initialised;

	public void Update()
	{
		if (!m_initialised)
			Initialise ();
		
	}

	private void Initialise()
	{
		var players = GameContext.Instance.Players;
		var characters = GameContext.Instance.Characters;

		ShapeType a = players [0].PrimaryShape;
		ShapeType b = players [1].PrimaryShape;

		List<int> sumShapes = new List<int> ();
		for (int i = 0; i < (int)ShapeType.MAX; ++i) {
			sumShapes.Add (0);
		}

		for (int i = 0; i < characters.Count; ++i) {
			sumShapes [(int)characters [i].Shapes [0]]++;
			sumShapes [(int)characters [i].Shapes [1]]++;
		}
			
		ShapeType shapeA = GetPopularShape(0, sumShapes);
		imageA.sprite = players[0].m_shapeSpriteDict[shapeA];
		ShapeType shapeB = GetPopularShape(1, sumShapes);
		imageB.sprite = players[0].m_shapeSpriteDict[shapeB];
		ShapeType shapeC = GetPopularShape(2, sumShapes);
		imageC.sprite = players[0].m_shapeSpriteDict[shapeC];
	}

	ShapeType GetPopularShape(int index, List<int> shapeCounts)
	{
		ShapeType shape = ShapeType.None;

		for(int j = 0; j <index +1; ++j)
		{
			int highestValue = 0;
			for (int i = 0; i < shapeCounts.Count; ++i) 
			{
				if (shapeCounts [i] > highestValue) 
				{
					highestValue = shapeCounts [i];
					shape = (ShapeType)i;
				}
			}

			shapeCounts [(int)shape] = 0; // so it wont be selected again
		}

		UnityEngine.Debug.LogError (shape);
		return shape;
	}
}
