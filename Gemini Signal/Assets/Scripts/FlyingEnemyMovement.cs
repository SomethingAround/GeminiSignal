/*
 * File Name: FlyingEnemyMovement.cs
 * Author: Michael Sweetman
 * Description: manages the movement of flying enemies
 * Creation Date: 07/10/2019
 * Last Modified: 08/10/2019
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

	public float m_moveSpeed;
	public float m_rotationSpeed;
	public List<Vector3> m_positions;

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
    }

	/*
	 * Brief: sets the enemy's position each frame
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
			// determine the direction to the new target
			m_targetDirection = m_positions[m_targetIndex] - transform.position;
			m_targetDirection.Normalize();
		}

		// if the enemy has turned enough to reach its target direction, move towards the target, otherwise continue turning
		if (Vector3.Dot(gameObject.transform.right.normalized, m_targetDirection) > m_rotationThreshold)
		{
			gameObject.transform.right = m_targetDirection;
			gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, m_positions[m_targetIndex], m_moveSpeed * Time.deltaTime);
		}
		else
		{
			gameObject.transform.right = Vector3.RotateTowards(gameObject.transform.right, m_targetDirection, m_rotationSpeed * Time.deltaTime, 0.0f);
		}
	}
}
