using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulseLighting : MonoBehaviour {

    private Metronome m_metronome;
    private Image m_image;

    public List<Color> m_lightColors = new List<Color>();

    public float m_maxAlpha = 0.3f;
    private bool m_lightsOn = false;
    private float t;

	// Use this for initialization
	void Start () {
        m_image = GetComponent<Image>();
        m_metronome = FindObjectOfType<Metronome>();
        m_metronome.TickEvent += MetronomeTick;
	}

    void MetronomeTick()
    {
        m_image.color = m_lightColors[Random.Range(0, m_lightColors.Count)];
        m_lightsOn = true;
        t = 0;
    }

    // Update is called once per frame
    void Update()
    {        
        t += Time.deltaTime * 5.0f;

        if (m_lightsOn)
        {
            Color c = m_image.color;
            c.a = m_maxAlpha; // Mathf.Lerp(0, m_maxAlpha, t);
            m_image.color = c;

            //if (c.a > m_maxAlpha - 0.01f)
            //{
                m_lightsOn = false;
                t = 0;
            //}
        }
        else if (m_image.color.a > 0.01f)
        {
            Color c = m_image.color;
            c.a = Mathf.Lerp(m_maxAlpha, 0, t);
            m_image.color = c;
        }
	}
}
