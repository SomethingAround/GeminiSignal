﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

/*
 * File Name: PlayerSwap.cs
 * Author: Connor Li
 * Description: Manages the swapping of the player
 * Creation Date: 14/10/2019
 * Last Modified: 20/11/2019
 */

public class PlayerSwap : MonoBehaviour
{
	public float m_timeTillPhased = 1.0f;
	[HideInInspector]
	public float m_phaseTimer = 0.0f;

	[HideInInspector]
	public bool m_isPhased = false;
	[HideInInspector]
	public bool m_isPhasing = false;

	Material m_shader;

	public GameObject m_enemyMaterialManager;
	EnemyMatEdit m_enemyMaterialEditor;

	/* 
	 * Brief: gets the player's shader
	 */
	void Start()
    {
		m_shader = gameObject.GetComponent<Renderer>().materials[1];

		// store the enemy material editor
		m_enemyMaterialEditor = m_enemyMaterialManager.GetComponent<EnemyMatEdit>();
		m_enemyMaterialEditor.EditEnemyAlpha(m_isPhased);
	}

	/* 
	 * Brief: manage the swapping of the player
	 */
	void Update()
    {
		// if the swap button is pressed, start phasing
		if ((Input.GetButtonDown("Swap") || XCI.GetButtonDown(XboxButton.X)) && !m_isPhasing)
		{
			// set m_isPhasing to true so the phase animation begins
			m_isPhasing = true;
			// swap the current phase state of the player
			m_isPhased = !m_isPhased;

			// edit the alpha of the enemy
			m_enemyMaterialEditor.EditEnemyAlpha(m_isPhased);
		}

		// if the player is phasing, update the shader
		if (m_isPhasing)
		{
			// increase the timer
			m_phaseTimer += Time.deltaTime;

			// if the player is phased
			if(m_isPhased)
			{
				// increase the value of the shader
				m_shader.SetFloat("_Cloak_per", m_phaseTimer);
			}
			// if the player is not phased
			else
			{
				// decrease the value of the shader
				m_shader.SetFloat("_Cloak_per", 1.0f - m_phaseTimer);
			}

			// if the phase timer has run out, stop phasing
			if(m_phaseTimer >= m_timeTillPhased)
			{
				// stop phasing
				m_isPhasing = false;
				// set value of shader to intended value
				m_shader.SetFloat("_Cloak_per", (m_isPhased) ? 1 : 0);
				// reset the timer
				m_phaseTimer = 0.0f;
			}
		}
    }
}
