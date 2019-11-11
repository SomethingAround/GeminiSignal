using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

/*
 * File Name: PlayerMovement.cs
 * Author: Connor Li
 * Description: Manages the player's movement
 * Creation Date: 08/10/2019
 * Last Modified: 11/11/2019
 */

public class PlayerMovement : MonoBehaviour
{
	public float m_groundSpeed = 2.0f;
	public float m_airSpeed = 0.1f;
	public float m_maxSpeed = 5.0f;
	public float m_minWallDistance = 0.8f;
	public float m_minimumMaxAirSpeed = 0.1f;

	float m_currentSpeed = 0.0f;
	float m_translation = 0.0f;
	float m_aerialMaxSpeed = 0.0f;
	float m_direction = 0.0f;
	float m_yRayOffset = 0.05f;

	[HideInInspector]
	public Vector3 m_startPosition = Vector3.zero;
	Vector2 m_moveVelocity = Vector2.zero;
	Vector2 m_maxVelocity = Vector2.zero;
	Vector2 m_wallHit = Vector2.zero;

	Vector3 m_rayPosition = Vector3.zero;

	Quaternion m_right = Quaternion.Euler(0, 0, 0);

	Quaternion m_left = Quaternion.Euler(0, 180, 0);

	Rigidbody2D m_rb2d;

	PlayerJump m_playJump;

	CameraMovement m_cameraMovement;

	RaycastHit2D m_rayRH2D;
	Vector2 m_playerDimensions = Vector2.zero;

	/* 
	 * Brief: initialise variables for the player's horizontal movement
	 */
	void Start()
    {
		m_currentSpeed = m_groundSpeed;

		m_startPosition = gameObject.transform.position;

		m_playerDimensions = gameObject.GetComponent<BoxCollider2D>().size;

		m_rayPosition = new Vector3(0.0f, m_yRayOffset, 0.0f);

		m_rb2d = gameObject.GetComponent<Rigidbody2D>();

		m_rb2d.interpolation = RigidbodyInterpolation2D.Interpolate;

		m_playJump = gameObject.GetComponent<PlayerJump>();

		m_cameraMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
	}

	/* 
	 * Brief: manage the horizontal movement of the player
	 */
	void Update()
    {
		// if the player is alive
		if (m_cameraMovement.m_playerAlive)
		{
			m_direction = Input.GetAxis("Horizontal");
			//Sets m_translation to - or + move speed
			m_translation = m_direction * m_currentSpeed;
			//Sets moveVelocity's x to m_translation
			m_moveVelocity.x = m_translation;

			//Sets ray position to the left or right side of player
			if (m_moveVelocity.x > 0)
			{
				m_rayPosition.x = m_playerDimensions.x + m_minWallDistance;
				gameObject.transform.rotation = m_right;
			}
			else if (m_moveVelocity.x < 0)
			{
				m_rayPosition.x = -m_playerDimensions.x - m_minWallDistance;
				gameObject.transform.rotation = m_left;
			}

			//Increases velocity by the move velocity
			m_rb2d.velocity += m_moveVelocity;

			//If jump is pressed set max aerial speed to current x velocity
			if (Input.GetButtonDown("Jump"))
			{
				m_aerialMaxSpeed = Mathf.Abs(m_rb2d.velocity.x);
			}

			//Checks if x velocity is greater than max speed in either direction
			if (m_rb2d.velocity.x > m_maxSpeed || m_rb2d.velocity.x < -m_maxSpeed)
			{
				// set the player's speed to the max speed of the direction it is moving
				m_maxVelocity.Set(m_direction * m_maxSpeed, m_rb2d.velocity.y);
				m_rb2d.velocity = m_maxVelocity;
			}

			//Checks if the player is in the air
			if (m_playJump.m_inAir)
			{
				m_currentSpeed = m_airSpeed;
				//Sets the aerialMaxSpeed to the minimum
				if (m_aerialMaxSpeed < m_minimumMaxAirSpeed)
				{
					m_aerialMaxSpeed = m_minimumMaxAirSpeed;
				}
				//Checks if x velocity is greater than max speed
				if (Mathf.Abs(m_rb2d.velocity.x) > m_aerialMaxSpeed)
				{
					// set the player's speed to the max speed of the direction it is moving
					m_maxVelocity.Set( ((m_rb2d.velocity.x >= 0) ? 1 : -1) * m_aerialMaxSpeed, m_rb2d.velocity.y);
					m_rb2d.velocity = m_maxVelocity;
				}

			}
			//Resets current speed back to ground speed
			else
			{
				m_currentSpeed = m_groundSpeed;
				
				// determine what the max aerial speed would be for if the player were to fall of a platform without jumping
				m_aerialMaxSpeed = Mathf.Abs(m_rb2d.velocity.x);
			}

			// draw a raycast adjacent to the player, in the direction they are moving
			m_rayRH2D = Physics2D.Raycast(gameObject.transform.position + m_rayPosition, gameObject.transform.up, m_playerDimensions.y - m_yRayOffset);

			// if the raycast collides with a platform, the player is hitting a wall
			if (m_rayRH2D.collider != null && m_rayRH2D.collider.gameObject.tag == "Platform")
			{
				// set the player's horizontal velocity to 0
				m_wallHit.y = m_rb2d.velocity.y;
				m_rb2d.velocity = m_wallHit;
			}
		}
		print(m_moveVelocity.x);
		//print(m_direction);
	}
}
