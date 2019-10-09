using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * File Name: SwapBar
 * Author: Steven Pham
 * Description: to display the time they have till they are forced to switch
 * Creation Date: 8/10/2019
 * Last Modified: 8/10/2019
 */

public class SwapBar : MonoBehaviour
{
    private float m_blueLevel = 2.0f;
    GameObject m_player;
    private float m_rate = 1.0f;
    private bool m_playerBlue = true;

    /*
     * Start is called before the first frame update
    */
    void Start()
    {
        m_player = GameObject.Find("Player");
    }

    /*
     * Update is called once per frame
    */
    void Update()
    {
        //m_playerBlue = m_player.GetComponent<PlayerSwap>().m_isBlue;
        if (m_playerBlue && m_blueLevel > 0.0f)
        {
            m_blueLevel -= m_rate * Time.deltaTime;
        }
        else if (!m_playerBlue && m_blueLevel < 4.0f)
        {
            m_blueLevel += m_rate * Time.deltaTime;
        }

        if (m_blueLevel <= 0.0f && m_playerBlue || m_blueLevel >= 4.0f && !m_playerBlue)
        {
            //m_player.GetComponent<PlayerSwap>().m_isblue = !m_player.GetComponent<PlayerSwap>().m_isBlue;
            //m_player.GetComponent<PlayerSwap>().m_cloakTime = 0.0f;
        }

    }


}
