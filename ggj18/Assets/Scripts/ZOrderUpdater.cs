using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZOrderUpdater : MonoBehaviour {

	void Update()
	{		
        GameObject[] objBases = GameObject.FindGameObjectsWithTag("ObjBasePos"); // Todo: put in reload function if there are performance issues.

        List<GameObject> m_objBasePos = new List<GameObject>(objBases.Length);
        m_objBasePos.AddRange(objBases);

        if (m_objBasePos == null)
            return;

        m_objBasePos.Sort(delegate(GameObject a, GameObject b)
		{
            return b.transform.position.y.CompareTo(a.transform.position.y);
		});

        for (int i = 0; i < m_objBasePos.Count; ++i)
		{
            SpriteRenderer[] spriteRenderers = m_objBasePos[i].transform.parent.GetComponentsInChildren<SpriteRenderer>();

			foreach (var sr in spriteRenderers)
			{
				sr.sortingOrder = i;
			}
		}
	}
}
