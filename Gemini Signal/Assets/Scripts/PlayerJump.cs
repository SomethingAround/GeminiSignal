using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

/*
 * File Name: PlayerJump.cs
 * Author: Connor Li
 * Description: Manages the player's jump
 * Creation Date: 07/10/2019
 * Last Modified: 08/10/2019
 */

public class PlayerJump : MonoBehaviour
{
	float m_jumpTimer = 0.0f;
	public float m_jumpForce = 5.0f;
	public float m_maxJumpTime = 0.1f;
	public float m_jumpAcceleration = 0.5f;
	bool m_inAir = false;

	Vector2 m_jumpVelocity = new Vector2(0.0f, 0.0f);
	Vector2 m_jumpForcev2 = Vector2.zero;

	Rigidbody2D m_rb2d;
    /* 
	 * Start is called before the first frame update
	 */
    void Start()
    {
		m_rb2d = gameObject.GetComponent<Rigidbody2D>();
		m_jumpForcev2 = new Vector2(0.0f, m_jumpForce);
    }

    /*
	 * Update is called once per frame
	 */
    void Update()
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
			m_rb2d.drag = 6;
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
