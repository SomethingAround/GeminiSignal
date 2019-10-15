/*
 * File Name: Goal.cs
 * Author: Michael Sweetman
 * Description: activates the victory screen and disables player movement
 * Creation Date: 15/10/2019
 * Last Modified: 15/10/2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
	GameObject m_player;
	GameObject m_victoryScreen;
	GameObject m_swapBar;

	/*
	 * Brief: initialisation for the Goal
	 */
	private void Start()
	{
		// store the player and the UI elements
		m_player = GameObject.FindGameObjectWithTag("Player");
		m_victoryScreen = GameObject.FindGameObjectWithTag("VictoryScreen");
		m_swapBar = GameObject.FindGameObjectWithTag("SwapBar");
	}

	/*
	 * Brief: activates victory screen and disables player movement when the player collides with this object
	 */
	private void OnCollisionEnter2D(Collision collision)
	{
		// if the collision object is the player
		if (collision.gameObject == m_player)
		{
			// disable the player control scripts
			m_player.GetComponent<PlayerJump>().enabled = false;
			m_player.GetComponent<PlayerMovement>().enabled = false;
			m_player.GetComponent<PlayerSwap>().enabled = false;

			// hide the swap bar and show the victory screen
			m_swapBar.SetActive(false);
			m_victoryScreen.SetActive(true);

		}
			print("collided");
	}
}
