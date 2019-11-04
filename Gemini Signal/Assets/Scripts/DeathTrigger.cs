using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * File Name: DeathTrigger.cs
 * Author: Connor Li
 * Description: Checks if the player has hit the death trigger
 * Creation Date: 15/10/2019
 * Last Modified: 28/10/2019
 */

public class DeathTrigger : MonoBehaviour
{
	CameraMovement m_cameraMovement;
	GameObject m_player;

	/*
	 * Brief: initialisation for the death trigger
	 */
	void Start()
	{
		m_cameraMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
		m_player = GameObject.FindGameObjectWithTag("Player");
	}

	/*
	 * Brief: response when player enters death trigger
	 */
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// if the collision object is the player, set the player to not be alive
		if(collision.gameObject.tag == "Player")
		{
			m_cameraMovement.m_playerAlive = false;
		}
	}

	/*
	 * Brief: response when player exits death trigger
	 */
	void OnTriggerExit2D(Collider2D collision)
	{
		// if the collision object is the player, make the player inactive and move it to its start position
		if (collision.gameObject.tag == "Player")
		{
			// make the player inactive
			m_player.SetActive(false);

			// set the player's position to its start position
			m_player.transform.position = m_player.GetComponent<PlayerMovement>().m_startPosition;
			// remove the player's velocity
			m_player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}
	}
}
