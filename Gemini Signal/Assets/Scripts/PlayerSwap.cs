using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * File Name: PlayerSwap.cs
 * Author: Connor Li
 * Description: Manages the swapping of the player
 * Creation Date: 14/10/2019
 * Last Modified: 14/10/2019
 */

public class PlayerSwap : MonoBehaviour
{
	public float m_timeTillPhased = 1.0f;
	float m_phaseTimer = 0.0f;
	float m_downPhase = 0.9f;

	bool m_isPhased = false;
	bool m_isPhasing = false;

	Material m_shader;

    // Start is called before the first frame update
    void Start()
    {
		m_shader = gameObject.GetComponent<Renderer>().material;  
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetButtonDown("Swap"))
		{
			m_isPhasing = true;
		}

		if (m_isPhasing)
		{
			m_phaseTimer += Time.deltaTime / m_timeTillPhased;

			if(m_isPhased)
			{
				m_shader.SetFloat("Cloak_per", m_phaseTimer);
			}
			else
			{
				m_shader.SetFloat("Cloak_per", m_downPhase - m_phaseTimer);
			}

			if(m_phaseTimer >= m_timeTillPhased || m_phaseTimer <= m_timeTillPhased)
			{
				m_isPhasing = false;
			}
		}
    }
}
