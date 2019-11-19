using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

/*
 * File Name: PlayerJump.cs
 * Author: Connor Li
 * Description: Manages the player's jump
 * Creation Date: 07/10/2019
 * Last Modified: 18/11/2019
 */

public class PlayerJump : MonoBehaviour
{
	public float m_jumpForce = 5.0f;
	public float m_maxJumpTime = 0.1f;
	public float m_jumpAcceleration = 0.5f;
	public float m_airDrag = 0.0f;
	public float m_groundDrag = 6.0f;
	public float m_maxLandDrag = 40.0f;
	public float m_recoveryTime = 0.5f;
	float m_landDrag = 0.0f;
	float m_recoveryTimer = 0.0f;
	float m_jumpTimer = 0.0f;
    float m_rayOffset = 0.05f;
	float m_maxHorizontalSpeed = 5.0f;

    [HideInInspector]
	public bool m_inAir = false;

	float m_jumpVelocity = 0.0f;
	Vector2 m_jumpForcev2 = Vector2.zero;
    Vector3 m_rayPosition = Vector3.zero;

    Rigidbody2D m_rb2d;

	RaycastHit2D m_rayH2D;
	Vector2 m_playerDimensions = Vector2.zero;

	CameraMovement m_cameraMovement;
	
	/* 
	 * Brief: initialise variables for the player's jump
	 */
	void Start()
    {
		m_rb2d = gameObject.GetComponent<Rigidbody2D>();

		m_jumpForcev2 = new Vector2(0.0f, m_jumpForce);

		m_playerDimensions = gameObject.GetComponent<BoxCollider2D>().size;

        m_rayPosition = new Vector3(-(m_playerDimensions.x / 2) + m_rayOffset, -m_rayOffset, 0.0f);

		m_rb2d.drag = m_groundDrag;

		m_cameraMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();

		m_maxHorizontalSpeed = gameObject.GetComponent<PlayerMovement>().m_maxSpeed;
	}

    /*
	 * Brief: manage the jump of the player
	 */
    void Update()
    {
		// if the player is alive
		if (m_cameraMovement.m_playerAlive)
		{
			if (m_rb2d.drag > m_groundDrag)
			{
				m_recoveryTimer += Time.deltaTime;
				m_rb2d.drag = Mathf.Lerp(m_landDrag, m_groundDrag, m_recoveryTimer / m_recoveryTime);
			}

			// If the jump button goes down and the player is not in the air, jump
			if ((Input.GetButtonDown("Jump") || XCI.GetButtonDown(XboxButton.A)) && m_jumpTimer == 0.0f && !m_inAir)
			{
				m_rb2d.AddForce(m_jumpForcev2, ForceMode2D.Impulse);
				m_jumpTimer += Time.deltaTime;
			}
			// If the jump key or button is let go, stop jumping
			else if (Input.GetButtonUp("Jump") || XCI.GetButtonUp(XboxButton.A))
			{
				//Checks if the player is in the air
				if (m_inAir)
				{
					m_jumpTimer = m_maxJumpTime;
				}
				//Checks if the player is no longer in the air
				else
				{
					m_jumpTimer = 0.0f;
				}
			}
			//If jump key or button is held maintain velocity and increase timer
			else if ((Input.GetButton("Jump") || XCI.GetButton(XboxButton.A)) && m_jumpTimer < m_maxJumpTime)
			{
				//Checks if player is in the air
				if (m_inAir)
				{
					m_rb2d.velocity.Set(m_rb2d.velocity.x, m_jumpVelocity);

				}
				m_jumpTimer += Time.deltaTime;
			}

			//Store previous jump velocity
			m_jumpVelocity = m_rb2d.velocity.y;
			
			if (gameObject.transform.rotation.eulerAngles.y > 90.0f)
			{
				m_rayPosition.x = (m_playerDimensions.x / 2) - m_rayOffset;
			}
			else
			{
				m_rayPosition.x = -(m_playerDimensions.x / 2) + m_rayOffset;
			}

			// draw a raycast below the player
			m_rayH2D = Physics2D.Raycast(gameObject.transform.position + m_rayPosition, gameObject.transform.right, m_playerDimensions.x - m_rayOffset);

			//Debug.DrawRay(gameObject.transform.position + m_rayPosition, gameObject.transform.right);

			// if the raycast collides with a platform, the player is touching the ground
			if (m_rayH2D.collider != null && m_rayH2D.collider.gameObject.tag == "Platform")
			{
				m_inAir = false;
				m_jumpTimer = 0.0f;

				if (m_rb2d.drag == m_airDrag)
				{
					m_landDrag = Mathf.Lerp(m_maxLandDrag, m_groundDrag, (Mathf.Abs(m_rb2d.velocity.x) / m_maxHorizontalSpeed));
					m_rb2d.drag = m_landDrag;
					m_recoveryTimer = 0.0f;
				}
			}
			// if the raycast does not collide with a platform, the player is in the air
			else
			{
				m_inAir = true;
				m_rb2d.drag = m_airDrag;
			}
		}
		print(m_rb2d.velocity.x);
    }
}
