using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * File Name: Enemy Detection
 * Author: Steven Pham
 * Description: Has the enemy detect the player if they are in the opposite Dimension
 * Creation Date: 7/10/2019
 * Last Modified: 21/10/2019
 */

public class EnemyDetection : MonoBehaviour
{
    public bool m_isPhased;
    CameraMovement m_cameraMovement;
    GameObject m_player;
    private void Start()
    {
        //Finds the Camera 
        m_cameraMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
        //Finds the Player
        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    /*
     * Breif: will detect if a collision has accured
     * Parameter: a_collision: to determine the collision that will be encountered
     * 
     */
    private void OnTriggerStay2D(Collider2D a_collision)
    {
        //Finds the player and determine if the enemy sees the player that are in the opposite dimension
        if (a_collision.gameObject.tag == "Player" && a_collision.gameObject.GetComponent<PlayerSwap>().m_isPhasing != m_isPhased)
        {
            m_player.transform.position = m_player.GetComponent<PlayerMovement>().m_startPosition;
            m_cameraMovement.m_playerAlive = false;
        }
    }
}
