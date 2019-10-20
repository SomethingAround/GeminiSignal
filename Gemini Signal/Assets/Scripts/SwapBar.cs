using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * File Name: SwapBar
 * Author: Steven Pham
 * Description: to display the time they have till they are forced to switch
 * Creation Date: 8/10/2019
 * Last Modified: 15/10/2019
 */

public class SwapBar : MonoBehaviour
{
    public float m_phaseLevel = 2.0f;
    GameObject m_player;
    public float m_rate = 1.0f;
    bool m_playerPhased = false;

    /*
     * Start is called before the first frame update
    */
    void Start()
    {
		m_player = GameObject.FindGameObjectWithTag("Player");
    }

    /*
     * Update is called once per frame
    */
    void Update()
    {
		m_playerPhased = m_player.GetComponent<PlayerSwap>().m_isPhased;

		//This is how the swapbar will be working and how it will function
		if (m_playerPhased && m_phaseLevel > 0.0f)
        {
            m_phaseLevel -= m_rate * Time.deltaTime;
        }
        else if (!m_playerPhased && m_phaseLevel < 4.0f)
        {
            m_phaseLevel += m_rate * Time.deltaTime;
        }

        if (m_phaseLevel <= 0.0f && m_playerPhased || m_phaseLevel >= 4.0f && !m_playerPhased)
        {
			m_player.GetComponent<PlayerSwap>().m_isPhasing = true;
        }

		gameObject.GetComponent<RectTransform>().localPosition = new Vector3(505.4953f, (m_phaseLevel * 25.0f) + 54.0f, 0.0f);
		gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.1752778f, m_phaseLevel / 2.0f, 0.5842592f);
	}
}
