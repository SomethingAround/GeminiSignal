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
	[HideInInspector]
	public float m_phaseTimer = 0.0f;
	float m_downPhase = 0.9f;

	[HideInInspector]
	public bool m_isPhased = false;
	[HideInInspector]
	public bool m_isPhasing = false;

	Material m_shader;

	/* 
	 * Brief: gets the player's shader
	 */
	void Start()
    {
		m_shader = gameObject.GetComponent<Renderer>().material;  
    }

	/* 
	 * Brief: manage the swapping of the player
	 */
	void Update()
    {
		// if the swap button is pressed, start phasing
		if (Input.GetButtonDown("Swap"))
		{
			m_isPhasing = true;
		}

		// if the player is phasing, update the shader
		if (m_isPhasing)
		{
			// increate the timer
			m_phaseTimer += Time.deltaTime / m_timeTillPhased;

			// if the player is phased
			if(m_isPhased)
			{
				// decrease the value of the shader
				m_shader.SetFloat("_Cloak_per", m_downPhase - m_phaseTimer);
			}
			// if the player is not phased
			else
			{
				// increase the value of the shader
				m_shader.SetFloat("_Cloak_per", m_phaseTimer);
			}

			// if the phase timer has run out
			if(m_phaseTimer >= m_timeTillPhased)
			{
				// stop phasing
				m_isPhasing = false;
				// swap the current phase state of the player
				m_isPhased = !m_isPhased;
				// reset the timer
				m_phaseTimer = 0.0f;
			}
		}
    }
}
