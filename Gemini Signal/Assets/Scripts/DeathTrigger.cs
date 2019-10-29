using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
	CameraMovement m_cameraMovement;

	GameObject m_player;
	/*
	 * File Name: DeathTrigger.cs
	 * Author: Connor Li
	 * Description: Checks if the player has hit the death trigger
	 * Creation Date: 15/10/2019
	 * Last Modified: 15/10/2019
	 */
	void Start()
	{
		m_cameraMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();

		m_player = GameObject.FindGameObjectWithTag("Player");
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			m_cameraMovement.m_playerAlive = false;
		}
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			m_player.SetActive(false);

			m_player.transform.position = m_player.GetComponent<PlayerMovement>().m_startPosition;
			m_player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}
	}
}
