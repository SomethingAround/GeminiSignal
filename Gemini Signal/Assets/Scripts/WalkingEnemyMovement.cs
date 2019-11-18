/*
 * File Name: WalkingEnemyMovement.cs
 * Author: Michael Sweetman
 * Description: manages the movement of walking enemies
 * Creation Date: 08/10/2019
 * Last Modified: 18/11/2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemyMovement : MonoBehaviour
{
	float m_startXPosition;
	float m_targetXPosition;

	Vector3 m_movement = Vector3.zero;
	float m_moveThreshold = 0.1f;
	float m_rotationThreshold = 0.05f;

	bool m_turning = true;

	Animator m_animator;

	public float m_endXPosition = 10;
	public float m_moveSpeed = 3;
	public float m_rotationSpeed = 3;
	public GameObject m_model;
	public GameObject m_fieldOfView;

	/*
	 * Brief: Initialisation for the enemy
	 */
	void Start()
    {
		// determine the enemy's start and target position
		m_startXPosition = gameObject.transform.position.x;
		m_targetXPosition = m_endXPosition;

		// store the enemy's animator
		m_animator = gameObject.GetComponent<Animator>();

		// store the amount the enemy will move in a Vector3
		m_movement.x = m_moveSpeed;
    }

	/*
	* Brief: sets the enemy's position or rotation each frame
	*/
	void Update()
    {
		// if the enemy is turning, rotate towards the target
		if (m_turning)
		{
			//if (m_animator.GetFloat("Blend") < 1.0f)
			//{
			//	m_animator.SetFloat("Blend", m_animator.GetFloat("Blend") + Time.deltaTime);
			//}
			//else
			//{
			//	m_animator.SetFloat("Blend", 1.0f);
			//}

			// if the target is to the right
			if (m_targetXPosition > gameObject.transform.position.x)
			{
				// rotate clockwise
				m_model.transform.Rotate(gameObject.transform.up, -m_rotationSpeed);

				// if the enemy has rotated so they are facing right, stop turning and show the field of view
				if (m_model.transform.right.x > 1.0f - m_rotationThreshold)
				{
					// make the enemy face directly right
					m_model.transform.right = Vector3.right;

					// activate the field of view
					if (m_fieldOfView != null)
					{
						m_fieldOfView.GetComponent<MeshRenderer>().enabled = true;
						m_fieldOfView.GetComponent<Collider2D>().enabled = true;
					}

					// stop turning
					m_turning = false;
					m_animator.SetBool("Turning", m_turning);
				}
			}
			// if the target is to the left
			else
			{
				// rotate anti-clockwise
				m_model.transform.Rotate(gameObject.transform.up, m_rotationSpeed);

				// if the enemy has rotated so they are facing left, stop turning and show the field of view
				if (m_model.transform.right.x < -1.0f + m_rotationThreshold)
				{
					// make the enemy face directly left
					m_model.transform.right = Vector3.left;

					// activate the field of view
					if (m_fieldOfView != null)
					{
						m_fieldOfView.GetComponent<MeshRenderer>().enabled = true;
						m_fieldOfView.GetComponent<Collider2D>().enabled = true;
					}

					// stop turning
					m_turning = false;
					m_animator.SetBool("Turning", m_turning);
				}
			}
		}
		// if the enemy is moving, move towards the target location
		else
		{
			//if (m_animator.GetFloat("Blend") > 0.0f)
			//{
			//	m_animator.SetFloat("Blend", m_animator.GetFloat("Blend") - Time.deltaTime);
			//}
			//else
			//{
			//	m_animator.SetFloat("Blend", 0.0f);
			//}

			// if the target is to the right, move the enemy right
			if (m_targetXPosition > gameObject.transform.position.x)
			{
				gameObject.transform.position += m_movement * Time.deltaTime;
			}
			// if the target is to the left, move the enemy left
			else
			{
				gameObject.transform.position -= m_movement * Time.deltaTime;
			}
		
			// if the enemy has reached its target, change target, hide the field of view and start turning
			if (Mathf.Abs(gameObject.transform.position.x - m_targetXPosition) < m_moveThreshold)
			{
				// determine next target position
				m_targetXPosition = (m_targetXPosition == m_endXPosition) ? m_startXPosition : m_endXPosition;

				// deactivate field of view
				if (m_fieldOfView != null)
				{
					m_fieldOfView.GetComponent<MeshRenderer>().enabled = false;
					m_fieldOfView.GetComponent<Collider2D>().enabled = false;
				}

				// start turning
				m_turning = true;
				m_animator.SetBool("Turning", m_turning);
			}
		}
	}
}
