using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZOrderUpdater : MonoBehaviour {

	public List<Transform> Objects = new List<Transform>();

	void Update()
	{
		
		Objects.Sort(delegate(Transform x, Transform y)
		{
				return y.position.y.CompareTo(x.position.y);

				/*
				if (x.position.y > y.position.y)
					return x;
				else
					return y;
				*/
		});	

		//float z = 0;
		for (int i = 0; i < Objects.Count; ++i)
		{
			//o.position = new Vector3(o.position.x, o.position.y, z);
			//z -= 0.01;

			SpriteRenderer[] spriteRenderers = Objects[i].GetComponentsInChildren<SpriteRenderer>();

			foreach (var sr in spriteRenderers)
			{
				sr.sortingOrder = i;
			}

			//Objects[i].GetComponent<SpriteRenderer>().sortingOrder = i;
		}
	}
}
