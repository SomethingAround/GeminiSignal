/*
 * File Name: WalkingEnemyMovement.cs
 * Author: Michael Sweetman
 * Description: manages the movement of walking enemies
 * Creation Date: 08/10/2019
 * Last Modified: 4/11/2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemyMovement : MonoBehaviour
{
	float m_startXPosition;
	float m_targetXPosition;
	
	float m_moveThreshold = 0.1f;
	float m_rotationThreshold = 0.05f;

	Vector3 m_faceLeft = new Vector3(-1.0f, 0.0f, 0.0f);
	Vector3 m_faceRight = new Vector3(1.0f, 0.0f, 0.0f);

	bool m_turning = true;
	Transform m_fieldOfView;

	public float m_endXPosition;
	public float m_moveSpeed;
	public float m_rotationSpeed;

	/*
	 * Brief: Initialisation for the enemy
	 */
	void Start()
    {
		// determine the enemy's start and target position
		m_startXPosition = gameObject.transform.position.x;
		m_targetXPosition = m_endXPosition;

		// if the enemy has a child object, store it as the field of view so it can be hidden when turning
		if (gameObject.transform.childCount >= 0)
		{
			m_fieldOfView = gameObject.transform.GetChild(0);
		}
    }

	/*
	* Brief: sets the enemy's position or rotation each frame
	*/
	void Update()
    {
		// if the enemy is turning, rotate towards the target
		if (m_turning)
		{
			// if the target is to the right
			if (m_targetXPosition > gameObject.transform.position.x)
			{
				// rotate clockwise
				gameObject.transform.Rotate(gameObject.transform.up, -m_rotationSpeed);

				// if the enemy has rotated so they are facing right, stop turning and show the field of view
				if (gameObject.transform.right.x > 1.0f - m_rotationThreshold)
				{
					gameObject.transform.right = m_faceRight;
					
					m_turning = false;

					if (m_fieldOfView != null)
					{
						m_fieldOfView.GetComponent<MeshRenderer>().enabled = true;
						m_fieldOfView.GetComponent<Collider2D>().enabled = true;
					}
				}
			}
			// if the target is to the left
			else
			{
				// rotate anti-clockwise
				gameObject.transform.Rotate(gameObject.transform.up, m_rotationSpeed);

				// if the enemy has rotated so they are facing left, stop turning and show the field of view
				if (gameObject.transform.right.x < -1.0f + m_rotationThreshold)
				{
					gameObject.transform.right = m_faceLeft;
					m_turning = false;

					if (m_fieldOfView != null)
					{
						m_fieldOfView.GetComponent<MeshRenderer>().enabled = true;
						m_fieldOfView.GetComponent<Collider2D>().enabled = true;
					}
				}
			}
		}
		// if the enemy is moving, move towards the target location
		else
		{
			// if the target is to the right, move the enemy right
			if (m_targetXPosition > gameObject.transform.position.x)
			{
				gameObject.transform.position += new Vector3(m_moveSpeed, 0.0f, 0.0f) * Time.deltaTime;
			}
			// if the target is to the left, move the enemy left
			else
			{
				gameObject.transform.position += new Vector3(-m_moveSpeed, 0.0f, 0.0f) * Time.deltaTime;
			}
		
			// if the enemy has reached its target, change target, hide the field of view and start turning
			if (Mathf.Abs(gameObject.transform.position.x - m_targetXPosition) < m_moveThreshold)
			{
				m_targetXPosition = (m_targetXPosition == m_endXPosition) ? m_startXPosition : m_endXPosition;
				m_turning = true;

				if (m_fieldOfView != null)
				{
					m_fieldOfView.GetComponent<MeshRenderer>().enabled = false;
					m_fieldOfView.GetComponent<Collider2D>().enabled = false;
				}
			}
		}
	}
}
