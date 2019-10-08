/*
 * File Name: WalkingEnemyMovement.cs
 * Author: Michael Sweetman
 * Description: manages the movement of walking enemies
 * Creation Date: 08/10/2019
 * Last Modified: 08/10/2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemyMovement : MonoBehaviour
{
	float m_startXPosition;
	float m_targetPosition;
	
	float m_rotationThreshold = 0.999f;
	float m_positionThreshold = 0.1f;
	
	bool m_turning = false;
	Transform m_fieldOfView;

	public float m_endXPosition;
	public float m_moveSpeed;
	public float m_rotationSpeed;

	/*
	 * Brief: Initialisation for the enemy
	 */
	void Start()
    {
		m_startXPosition = gameObject.transform.position.x;
		m_targetPosition = m_endXPosition;
		m_fieldOfView = gameObject.transform.GetChild(0);
    }

	/*
	* Brief: sets the enemy's position each frame
	*/
	void Update()
    {
		// if the enemy is turning, rotate towards the target
		if (m_turning)
		{
			// if the target is to the right
			if (m_targetPosition > gameObject.transform.position.x)
			{
				// rotate anti-clockwise
				gameObject.transform.Rotate(gameObject.transform.up, m_rotationSpeed);

				// if the enemy has rotated so they are facing right, stop turning and show the field of view
				if (gameObject.transform.rotation.eulerAngles.y > 170.0f)
				{
					gameObject.transform.rotation.SetEulerAngles(0.0f, 180.0f, 0.0f);
					m_turning = false;
					m_fieldOfView.GetComponent<MeshRenderer>().enabled = true;
					m_fieldOfView.GetComponent<Collider2D>().enabled = true;
				}
			}
			else
			{
				// rotate clockwise
				gameObject.transform.Rotate(gameObject.transform.up, -m_rotationSpeed);

				// if the enemy has rotated so they are facing left, stop turning and show the field of view
				if (gameObject.transform.rotation.eulerAngles.y < 10.0f)
				{
					gameObject.transform.rotation.SetEulerAngles(0.0f, 0.0f, 0.0f);
					m_turning = false;
					m_fieldOfView.GetComponent<MeshRenderer>().enabled = true;
					m_fieldOfView.GetComponent<Collider2D>().enabled = true;
				}
			}
		}
		// if the enemy is moving, move towards the target location
		else
		{
			// if the target is to the right, move the enemy right
			if (m_targetPosition > gameObject.transform.position.x)
			{
				gameObject.transform.position += new Vector3(m_moveSpeed, 0.0f, 0.0f) * Time.deltaTime;
			}
			// if the target is to the left, move the enemy left
			else
			{
				gameObject.transform.position += new Vector3(-m_moveSpeed, 0.0f, 0.0f) * Time.deltaTime;
			}

			// if the enemy has reached its target, change target, hide the field of view and start turning
			if (gameObject.transform.position.x - m_targetPosition < m_positionThreshold && gameObject.transform.position.x - m_targetPosition > -m_positionThreshold)
			{
				m_targetPosition = (m_targetPosition == m_endXPosition) ? m_startXPosition : m_endXPosition;
				m_turning = true;
				m_fieldOfView.GetComponent<MeshRenderer>().enabled = false;
				m_fieldOfView.GetComponent<Collider2D>().enabled = false;
			}
		}
	}
}
