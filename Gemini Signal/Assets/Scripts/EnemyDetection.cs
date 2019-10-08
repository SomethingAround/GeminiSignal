﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * File Name: Enemy Detection
 * Author: Steven Pham
 * Description: Has the enemy detect the player if they are in the opposite Dimension
 * Creation Date: 7/10/2019
 * Last Modified: 8/10/2019
 */

public class EnemyDetection : MonoBehaviour
{
    /*
     * Breif: will detect if a collision has accured
     * Parameter: a_collision: to determine the collision that will be encountered
     * 
     */
    private void OnTriggerStay2D(Collider2D a_collision)
    {
        if ((a_collision.gameObject.tag == "Fov" && a_collision.gameObject.GetComponentInParent<PlayerMove>() != null && a_collision.gameObject.GetComponentInParent<PlayerMove>().m_isBlue != m_isBlue) ||
            (a_collision.gameObject.GetComponent<PlayerMove>() != null && a_collision.gameObject.GetComponentInParent<PlayerSwap>().m_isBlue != m_isBlue))
        {
            gameObject.GetComponent<PlayerMove>()->ReturnToStart();
        }
    }
}