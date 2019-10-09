/*
 * File Name: FlyingEnemyMovement.cs
 * Author: Michael Sweetman
 * Description: manages the movement of flying enemies
 * Creation Date: 07/10/2019
 * Last Modified: 09/10/2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyMovement : MonoBehaviour
{
	Vector3 m_startPosition;
	Vector3 m_targetDirection;
	int m_targetIndex;
	float m_rotationThreshold = 0.999f;
	int m_maneuverIndex;
	bool m_maneuvering;
	Transform m_fieldOfView;

	public float m_moveSpeed;
	public float m_rotationSpeed;
	public List<Vector3> m_positions;
	public int[] m_maneuverPoints;

	/*
	 * Brief: Initialisation for the enemy
	 */
	void Start()
    {
		// add the enemy's starting position to the start of the positions list
		m_startPosition = gameObject.transform.position;
		m_positions.Insert(0, m_startPosition);
		
		// determine the first target direction
		m_targetDirection = m_positions[m_targetIndex] - transform.position;
		m_targetDirection.Normalize();

		m_fieldOfView = gameObject.transform.GetChild(0);
	}

	/*
	 * Brief: sets the enemy's position or rotation each frame
	 */
	void Update()
	{
		// if the enemy has reached its target position, determine the direction to get to the next target
		if (m_positions[m_targetIndex] == gameObject.transform.position)
		{
			// move to the next target in the list, or to the start of the list if at the end
			if (m_targetIndex == m_positions.Count - 1)
			{
				m_targetIndex = 0;
			}
			else
			{
				++m_targetIndex;
			}
			/// determine the direction to the new target
			m_maneuverIndex = 0;
			m_maneuvering = false;
			m_targetDirection = m_positions[m_targetIndex] - transform.position;
			for (int i = 0; i < m_maneuverPoints.Length; ++i)
			{
				if (m_maneuverPoints[i] == m_targetIndex)
				{
					m_maneuvering = true;
					m_targetDirection.Set((m_positions[m_targetIndex].x > gameObject.transform.position.x) ? -1.0f : 1.0f, 0.0f, 0.0f);
					break;
				}
			}
			m_targetDirection.Normalize();
		}
		
		/// if the enemy has turned enough to reach its target direction, move towards the target, otherwise continue turning
		if (Vector3.Dot(gameObject.transform.right.normalized, m_targetDirection) > m_rotationThreshold)
		{
			// if the enemy is maneuvering, determine the next step that needs to be performed, or stop maneuvering if it is complete
			if (m_maneuvering)
			{
				++m_maneuverIndex;
				switch (m_maneuverIndex)
				{
					case 1:
						m_targetDirection.Set((m_positions[m_targetIndex].x > gameObject.transform.position.x) ? 1.0f : -1.0f, 0.0f, 0.0f);
						m_fieldOfView.GetComponent<MeshRenderer>().enabled = false;
						m_fieldOfView.GetComponent<Collider2D>().enabled = false;
						break;
					case 2:
						m_targetDirection = m_positions[m_targetIndex] - transform.position;
						m_fieldOfView.GetComponent<MeshRenderer>().enabled = false;
						m_fieldOfView.GetComponent<Collider2D>().enabled = false;
						break;
					case 3:
						m_maneuvering = false;
						break;
				}
			}
			// if the enmy is not maneuvering, move towards the target
			else
			{
				gameObject.transform.right = m_targetDirection;
				gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, m_positions[m_targetIndex], m_moveSpeed * Time.deltaTime);
			}
		}
		// if the enemy has not yet reached its target direction, rotate towards the target
		else
		{
			// if the enemy is at the second step in the maneuver, rotate around the up vector instead of the right vector
			if (m_maneuverIndex == 1)
			{
				gameObject.transform.up = Vector3.RotateTowards(gameObject.transform.up, m_targetDirection, m_rotationSpeed * Time.deltaTime, 0.0f);
			}
			else
			{
				gameObject.transform.right = Vector3.RotateTowards(gameObject.transform.right, m_targetDirection, m_rotationSpeed * Time.deltaTime, 0.0f);
			}
		}
	}
}
