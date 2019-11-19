using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * File Name: SwapBar
 * Author: Steven Pham, Michael Sweetman
 * Description: to display the time they have till they are forced to switch
 * Creation Date: 8/10/2019
 * Last Modified: 04/11/2019
 */

public class SwapBar : MonoBehaviour
{
    PlayerSwap m_playerSwap;
	float m_maxPhase = 100.0f;
	float m_phaseLevel = 50.0f;
    bool m_playerPhased = false;
	Vector3 m_scale = Vector3.zero;

	public float m_rate = 25.0f;

	/*
	 * Brief: Initialisation for the swap bar
	 */
	void Start()
    {
		// store player's swap script and scale of the bar
		m_playerSwap = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSwap>();
		m_scale = gameObject.GetComponent<RectTransform>().localScale;
	}

	/*
	 * Brief: determines and displays player phase level and determines whether the player should be forced to swap each frame
	 */
	void Update()
    {
		// get the player's current phase state
		m_playerPhased = m_playerSwap.m_isPhased;

		// if the player is phased and the phase level hasn't reached its minimum, decrease the phase level
		if (m_playerPhased && m_phaseLevel > 0.0f)
        {
            m_phaseLevel -= m_rate * Time.deltaTime;
        }
		// if the player is not phased and the phase level hasn't reached its maximum, increase the phase level
        else if (!m_playerPhased && m_phaseLevel < m_maxPhase)
        {
            m_phaseLevel += m_rate * Time.deltaTime;
        }

		// if the player is phased and the phase level is depleted, or if the player is not phased and the phase level has maxed out, force the player to swap states
        if (m_phaseLevel <= 0.0f && m_playerPhased || m_phaseLevel >= m_maxPhase && !m_playerPhased)
        {
			m_playerSwap.m_isPhasing = true;
			m_playerSwap.m_isPhased = !m_playerSwap.m_isPhased;
		}

		// determine the y scale of the bar using the phase level
		gameObject.GetComponent<RectTransform>().localScale = new Vector3(m_scale.x, (m_phaseLevel / m_maxPhase) * m_scale.y, m_scale.z);
	}
}
