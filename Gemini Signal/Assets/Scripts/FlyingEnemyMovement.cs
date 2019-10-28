/*
 * File Name: FlyingEnemyMovement.cs
 * Author: Michael Sweetman
 * Description: manages the movement of flying enemies
 * Creation Date: 07/10/2019
 * Last Modified: 28/10/2019
 * 
 * ROTATION THRESHOLD SHOULD BE DETERMINED BY ROTATION SPEED
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyMovement : MonoBehaviour
{
	[System.Serializable]
	public struct Movement
	{
		public Vector3 m_offset;
		public bool m_swapYawRotation;

		public Movement(Vector3 a_offset, bool a_swapYawRotation)
		{
			m_offset = a_offset;
			m_swapYawRotation = a_swapYawRotation;
		}
	}

	Vector3 m_targetDirection;
	int m_targetIndex = 0;
	Vector3 m_targetPosition;

	Transform m_fieldOfView;

	float m_rotationThreshold = 0.999f;
	bool m_rotatingYaw = false;
	bool m_maneuverCompleted = true;

	public float m_moveSpeed;
	public float m_rotationSpeed;
	public List<Movement> m_movements;

	void Start()
	{
		m_targetPosition = transform.position + m_movements[m_targetIndex].m_offset;
		m_targetDirection = m_targetPosition - transform.position;

		if (transform.childCount > 0)
		{
			m_fieldOfView = transform.GetChild(0);
		}
	}

	void Update()
	{
		if (m_movements.Count > 0)
		{
			if (transform.position == m_targetPosition)
			{
				if (m_movements[m_targetIndex].m_swapYawRotation && !m_maneuverCompleted)
				{
					m_targetDirection = (m_targetPosition.x >= transform.position.x) ? Vector3.left : Vector3.right;
				}
				else if (!m_movements[m_targetIndex].m_swapYawRotation)
				{
					if (m_targetIndex == m_movements.Count - 1)
					{
						m_targetIndex = 0;
					}
					else
					{
						++m_targetIndex;
					}

					m_targetPosition = transform.position + m_movements[m_targetIndex].m_offset;
					m_targetDirection = m_targetPosition - transform.position;
					m_targetDirection.Normalize();

					m_rotatingYaw = false;
					m_maneuverCompleted = false;
				}
			}


			if (Vector3.Dot(transform.right.normalized, m_targetDirection) > m_rotationThreshold)
			{
				transform.right = m_targetDirection;

				if (m_movements[m_targetIndex].m_swapYawRotation && !m_maneuverCompleted)
				{
					if (m_rotatingYaw)
					{
						m_targetDirection = m_targetPosition - transform.position;
						m_targetDirection.Normalize();
						m_maneuverCompleted = true;

						m_fieldOfView.GetComponent<MeshRenderer>().enabled = true;
						m_fieldOfView.GetComponent<Collider2D>().enabled = true;
					}
					else
					{
						m_targetDirection = (m_targetPosition.x >= transform.position.x) ? Vector3.right : Vector3.left;
						m_rotatingYaw = true;

						m_fieldOfView.GetComponent<MeshRenderer>().enabled = false;
						m_fieldOfView.GetComponent<Collider2D>().enabled = false;
					}
				}
				else
				{
					transform.position = Vector3.MoveTowards(transform.position, m_targetPosition, m_moveSpeed * Time.deltaTime);
				}
			}
			else
			{
				if (m_rotatingYaw && !m_maneuverCompleted)
				{
					transform.Rotate(transform.up, ((m_targetPosition.x >= transform.position.x) ? -1 : 1) * m_rotationSpeed);
				}
				else
				{
					transform.right = Vector3.RotateTowards(transform.right.normalized, m_targetDirection, m_rotationSpeed * Time.deltaTime, 0.0f);
				}
			}
		}
	}
}
