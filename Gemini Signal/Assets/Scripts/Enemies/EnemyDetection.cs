using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * File Name: Enemy Detection
 * Author: Steven Pham
 * Description: Has the enemy detect the player if they are in the opposite Dimension
 * Creation Date: 7/10/2019
 * Last Modified: 25/11/2019
 */

public class EnemyDetection : MonoBehaviour
{
    public bool m_isPhased;
    CameraMovement m_cameraMovement;
    GameObject m_player;
	PlayerSwap m_playerSwap;

	bool m_killedPlayer = false;
	float respawnTimer = 0.0f;
	public float m_respawnTimer = 1.0f;

	/*
	 * Brief: initialisation for enemy detection
	 */
    private void Start()
    {
        //Finds the Camera 
        m_cameraMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
        //Finds the Player
        m_player = GameObject.FindGameObjectWithTag("Player");
		// Finds the Player Swap script
		m_playerSwap = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSwap>();
    }

	/*
	 * manages the timer that determines when the player respawns after being killed by an enemy
	 */
	private void Update()
	{
		// if the player has been killed
		if (m_killedPlayer)
		{
			// increase the timer
			respawnTimer += Time.deltaTime;

			// if enough time has passed, respawn the player
			if (respawnTimer >= m_respawnTimer)
			{
				// return the player to its start position, with 0 velocity
				m_player.transform.position = m_player.GetComponent<PlayerMovement>().m_startPosition;
				m_player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

				// reset enemy detection
				m_killedPlayer = false;
				respawnTimer = 0.0f;
			}
		}
		
	}

	/*
     * Brief: will detect if a collision has accured
     * Parameter: a_collision: to determine the collision that will be encountered
     */
	private void OnTriggerStay2D(Collider2D a_collision)
    {
        //Finds the player and determine if the enemy sees the player that are in the opposite dimension
        if (a_collision.gameObject.tag == "Player" && m_playerSwap.m_isPhased != m_isPhased && !m_killedPlayer)
        {
			m_cameraMovement.m_playerAlive = false;
			m_killedPlayer = true;
		}
    }
}
