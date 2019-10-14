/*
 * File Name: CameraMovement.cs
 * Author: Michael Sweetman
 * Description: manages how the camera moves, based on the position of the player
 * Creation Date: 07/10/2019
 * Last Modified: 08/10/2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	GameObject m_player;
	Vector3 m_playerPosition;

	Vector3 m_cameraStartPosition;
	Vector3 m_cameraPosition;
	
	Vector3 m_newPosition;
	Vector3 m_velocity = Vector3.zero;

	float m_playerDeathWaitTimer = 0.0f;
	public float m_playerDeathWaitTime;

	[HideInInspector]
	public bool m_playerAlive;
	public float m_moveDuration;
	public float m_yOffsetFromPlayer;

    /*
	 * Brief: Initialisation for the camera
	 */
    void Start()
    {
		// store the player
		m_player = GameObject.FindGameObjectWithTag("Player");
		m_playerAlive = true;

		// determine where the camera should move to when the player is respawning
		m_playerPosition = m_player.transform.position;
		m_cameraStartPosition.Set(m_playerPosition.x,m_playerPosition.y + m_yOffsetFromPlayer,gameObject.transform.position.z);
		
		// set the camera's position
		gameObject.transform.position = m_cameraStartPosition;
    }

    /*
	 * Brief: sets the camera's position each frame
	 */
    void Update()
    {
		// if the player is alive, move towards the player
		if (m_playerAlive)
		{
			// reset the death wait timer
			m_playerDeathWaitTimer = 0.0f;

			// get the player's position
			m_playerPosition = m_player.transform.position;

			// determine where the camera should move to smoothly move to the player
			m_newPosition = Vector3.SmoothDamp(gameObject.transform.position, m_playerPosition, ref m_velocity, m_moveDuration);

			// set the camera's x position so it smoothly follows the player
			m_cameraPosition.Set(m_newPosition.x, m_playerPosition.y + m_yOffsetFromPlayer, gameObject.transform.position.z);
			gameObject.transform.position = m_cameraPosition;
		}
		// if the player is not alive, wait and then move towards the start position
		else
		{
			// increase the timer
			m_playerDeathWaitTimer += Time.deltaTime;

			// if the camera has waited enough, move towards the start position
			if (m_playerDeathWaitTimer >= m_playerDeathWaitTime)
			{
				// set the camera's x and y position so it moves towards the start position
				m_newPosition = Vector3.SmoothDamp(gameObject.transform.position, m_cameraStartPosition, ref m_velocity, m_moveDuration);
				m_cameraPosition.Set(m_newPosition.x, m_newPosition.y, gameObject.transform.position.z);
				gameObject.transform.position = m_cameraPosition;
			}
		}
    }
}
