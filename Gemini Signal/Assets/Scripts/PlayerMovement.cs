using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

/*
 * File Name: PlayerMovement.cs
 * Author: Connor Li
 * Description: Manages the player's movement
 * Creation Date: 08/10/2019
 * Last Modified: 14/10/2019
 */

public class PlayerMovement : MonoBehaviour
{
	public float m_groundSpeed = 2.0f;
	public float m_airSpeed = 0.1f;
	public float m_maxSpeed = 5.0f;
	public float m_minWallDistance = 0.8f;

	float m_currentSpeed = 0.0f;
	float m_translation = 0.0f;
	float m_aerialMaxSpeed = 0.0f;
	float m_direction = 0.0f;
	float m_yRayOffset = 0.03f;

	Vector2 m_startPosition = Vector2.zero;
	Vector2 m_moveVelocity = Vector2.zero;
	Vector2 m_maxVelocity = Vector2.zero;
	Vector2 m_wallHit = Vector2.zero;

	Vector3 m_rayPosition = Vector3.zero;

	Rigidbody2D m_rb2d;

	PlayerJump m_playJump;

	RaycastHit2D m_rayRH2D;

	/* 
	 * Start is called before the first frame update
	 */
	void Start()
    {
		m_currentSpeed = m_groundSpeed;

		m_startPosition = gameObject.transform.position;

		m_rayPosition = new Vector3(m_minWallDistance, -((gameObject.transform.localScale.y / 2) - m_yRayOffset), 0.0f);

		m_rb2d = gameObject.GetComponent<Rigidbody2D>();

		m_rb2d.interpolation = RigidbodyInterpolation2D.Interpolate;

		m_playJump = gameObject.GetComponent<PlayerJump>();

		m_rb2d.drag = 6.0f;
	}

	/*
	 * Update is called once per frame
	 */
	void Update()
    {
		m_direction = Input.GetAxis("Horizontal");
		//Sets m_translation to - or + move speed
		m_translation = m_direction * m_currentSpeed;
		//Sets moveVelocities x to m_translation
		m_moveVelocity.x = m_translation;
		//Sets ray position to the left or right side of player
		if (m_moveVelocity.x > 0)
		{
			m_rayPosition.x = m_minWallDistance;
		}
		else if (m_moveVelocity.x < 0)
		{
			m_rayPosition.x = -m_minWallDistance;
		}
		//Increases velocity by the 
		m_rb2d.velocity += m_moveVelocity;

		//If jump is pressed set max aerial speed to current x velocity
		if(Input.GetButtonDown("Jump"))
		{
			m_aerialMaxSpeed = m_rb2d.velocity.x;
		}

		//Checks if x velocity is greater than max speed in either direction
		if (m_rb2d.velocity.x > m_maxSpeed || m_rb2d.velocity.x < -m_maxSpeed)
		{
			m_maxVelocity.Set(m_direction * m_maxSpeed, m_rb2d.velocity.y);
			m_rb2d.velocity = m_maxVelocity;
		}

		//Checks if the play is in the air
		if (m_playJump.m_inAir)
		{
			m_currentSpeed = m_airSpeed;
			//Checks if the player is moving right 
			if (m_aerialMaxSpeed >= 0)
			{
				//Checks if x velocity is greater than max speed
				if (m_rb2d.velocity.x > m_aerialMaxSpeed)
				{
					m_maxVelocity.Set(m_aerialMaxSpeed, m_rb2d.velocity.y);
					m_rb2d.velocity = m_maxVelocity;
				}
			}
			//Checks if the player is moving left 
			else if (m_aerialMaxSpeed <= 0)
			{
				//Checks if x velocity is less than max speed
				if (m_rb2d.velocity.x < m_aerialMaxSpeed) 
				{
					m_maxVelocity.Set(m_aerialMaxSpeed, m_rb2d.velocity.y);
					m_rb2d.velocity = m_maxVelocity;
				}
			}
		}
		//Resets current speed back to ground speed
		else
		{
			m_currentSpeed = m_groundSpeed;
		}

		m_rayRH2D = Physics2D.Raycast(gameObject.transform.position + m_rayPosition, gameObject.transform.up, gameObject.transform.localScale.y - (m_yRayOffset * 2));
		if (m_rayRH2D.collider != null && m_rayRH2D.collider.gameObject.tag == "Platform")
		{
			m_wallHit.y = m_rb2d.velocity.y;
			m_rb2d.velocity = m_wallHit;
		}
		Debug.DrawRay(gameObject.transform.position + m_rayPosition, gameObject.transform.up, Color.magenta);
		print(m_rb2d.velocity);
	}
}
