using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

/*
 * File Name: PlayerJump.cs
 * Author: Connor Li
 * Description: Manages the player's jump
 * Creation Date: 07/10/2019
 * Last Modified: 15/10/2019
 */

public class PlayerJump : MonoBehaviour
{
	public float m_jumpForce = 5.0f;
	public float m_maxJumpTime = 0.1f;
	public float m_jumpAcceleration = 0.5f;
	public float m_groundDrag = 6.0f;
	float m_jumpTimer = 0.0f;
    float m_yRayOffset = 0.05f;
    float m_rayLength = 0.9f;

    [HideInInspector]
	public bool m_inAir = false;
	bool m_isOnGround = true;

	Vector2 m_jumpVelocity = new Vector2(0.0f, 0.0f);
	Vector2 m_jumpForcev2 = Vector2.zero;
    Vector3 m_rayPosition = Vector3.zero;

    Rigidbody2D m_rb2d;

	RaycastHit2D m_rayH2D;

	CameraMovement m_cameraMovement;
	/* 
	 * Start is called before the first frame update
	 */
	void Start()
    {
		m_rb2d = gameObject.GetComponent<Rigidbody2D>();

		m_jumpForcev2 = new Vector2(0.0f, m_jumpForce);

        m_rayPosition = new Vector3(-(gameObject.transform.localScale.x / 2) + m_yRayOffset, -((gameObject.transform.localScale.y / 2) + m_yRayOffset), 0.0f);

		m_rb2d.drag = m_groundDrag;

		m_cameraMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
	}

    /*
	 * Update is called once per frame
	 */
    void Update()
    {
		if (m_cameraMovement.m_playerAlive)
		{
			//If the jump key or button goes down jump
			if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space)) && m_jumpTimer == 0.0f && !m_inAir)
			{
				m_rb2d.AddForce(m_jumpForcev2, ForceMode2D.Impulse);
				m_jumpTimer += Time.deltaTime;
			}
			//If the jump key or button is let go set timer to max
			else if ((Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.Space)))
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
			//If jump key or button is held hold velocity and increase timer
			else if ((Input.GetButton("Jump") || Input.GetKey(KeyCode.Space)) && m_jumpTimer < m_maxJumpTime)
			{
				//Checks if player is in the air
				if (m_inAir)
				{
					m_rb2d.velocity = m_jumpVelocity;

				}
				m_jumpTimer += Time.deltaTime;
			}
			//Store previous jump velocity
			m_jumpVelocity = m_rb2d.velocity;

			m_rayH2D = Physics2D.Raycast(gameObject.transform.position + m_rayPosition, gameObject.transform.right, m_rayLength - (m_yRayOffset * 2));
			if (m_rayH2D.collider != null && m_rayH2D.collider.gameObject.tag == "Platform")
			{
				m_isOnGround = true;
			}
			else
			{
				m_isOnGround = false;
			}

			if (m_isOnGround)
			{
				m_inAir = false;
				m_rb2d.drag = m_groundDrag;
			}
			else
			{
				m_inAir = true;
				m_rb2d.drag = 0.0f;
			}

			Debug.DrawRay(gameObject.transform.position + m_rayPosition, gameObject.transform.right, Color.magenta);
			//print(m_isOnGround + "Ground");
			print(m_inAir + "Air");
		}
    }

	/*
	 * Checks if the player has entered a collider
	 */
	private void OnCollisionEnter2D(Collision2D collision)
	{
		//Checks if the collision is with a Floor
		if (collision.gameObject.tag == "Platform")
		{
			m_inAir = false;
			m_rb2d.drag = m_groundDrag;
			m_jumpTimer = 0.0f;
		}
	}

	/*
	 * Checks if the player has left a collider
	 */
	private void OnCollisionExit2D(Collision2D collision)
	{
		//Checks if the collision is with a Floor
		if (collision.gameObject.tag == "Platform")
		{
			m_inAir = true;
			m_rb2d.drag = 0;
		}
	}
}
