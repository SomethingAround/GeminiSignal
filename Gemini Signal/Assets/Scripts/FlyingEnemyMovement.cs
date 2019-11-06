/*
 * File Name: FlyingEnemyMovement.cs
 * Author: Michael Sweetman
 * Description: manages the movement of flying enemies
 * Creation Date: 07/10/2019
 * Last Modified: 06/11/2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyMovement : MonoBehaviour
{
	enum MoveStep
	{
		rotatingTowardsOpposite,
		rotatingYaw,
		rotatingTowardsTarget,
		moving
	}
	MoveStep m_currentMoveStep;

	int m_targetIndex = 0;
	Vector3 m_targetPosition;
	Vector3 m_targetDirection;

	float m_rotationThreshold = 0.999f;
	float m_pitchTimer = 0.0f;
	Quaternion m_originRotation;

	Transform m_fieldOfView;

	/*
	 * Brief: a structure that stores whether the enemy's yaw needs to be rotated and the amount that the enemy is to move
	 */
	[System.Serializable]
	public struct Movement
	{
		public bool m_swapYawRotation;
		public Vector3 m_offset;
	}

	public float m_moveSpeed = 3;
	public float m_pitchRotationSpeed = 3;
	public float m_yawRotationSpeed = 6;
	public List<Movement> m_movements;


	void Start()
	{
		// determine the first target position
		m_targetPosition = transform.position + m_movements[m_targetIndex].m_offset;

		// store the start rotation as the origin rotation
		m_originRotation = transform.rotation;

		// if the yaw rotation is to be swapped, set the enemy to start rotating so it is facing horizontally away from the target
		if (m_movements[m_targetIndex].m_swapYawRotation)
		{
			m_targetDirection = (m_targetPosition.x >= transform.position.x) ? Vector3.left : Vector3.right;
			m_currentMoveStep = MoveStep.rotatingTowardsOpposite;
		}
		// if the yaw rotation is not to be swapped, set the enemy to start rotating so it is facing its target position
		else
		{
			m_targetDirection = m_targetPosition - transform.position;
			m_targetDirection.Normalize();
			m_currentMoveStep = MoveStep.rotatingTowardsTarget;
		}

		// if the enemy has a child, store it as its field of view
		if (transform.childCount > 0)
		{
			m_fieldOfView = transform.GetChild(0);
		}
	}

	void Update()
	{
		// if there are movements to be performed
		if (m_movements.Count > 0)
		{
			switch (m_currentMoveStep)
			{
				// if the enemy is currently rotating to face horizontally away from its target
				case MoveStep.rotatingTowardsOpposite:
					// rotate towards the target direction
					m_pitchTimer += Time.deltaTime * m_pitchRotationSpeed;
					transform.rotation = Quaternion.Lerp(m_originRotation, Quaternion.LookRotation(m_targetDirection, Vector3.up) * Quaternion.Euler(0, -90, 0), m_pitchTimer);

					// if the enemy has rotated enough to reach its target direction
					if (Vector3.Dot(transform.right, m_targetDirection) > m_rotationThreshold)
					{
						// set the rotation so the enemy exactly faces the target direction
						transform.rotation = Quaternion.LookRotation(m_targetDirection, Vector3.up) * Quaternion.Euler(0, -90, 0);
						// set the target direction to be facing horizontally towards its target
						m_targetDirection = (m_targetPosition.x >= transform.position.x) ? Vector3.right : Vector3.left;
						++m_currentMoveStep;

						// if the enemy has a field of view, disable it
						if (m_fieldOfView != null)
						{
							m_fieldOfView.GetComponent<MeshRenderer>().enabled = false;
							m_fieldOfView.GetComponent<Collider2D>().enabled = false;
						}
					}
					break;
				
				// if the enemy is currently rotating to face horizontally towards its target
				case MoveStep.rotatingYaw:
					// rotate the up vector so the right vector is facing the target direction
					transform.Rotate(transform.up, ((m_targetPosition.x >= transform.position.x) ? -1 : 1) * m_yawRotationSpeed);

					// if the enemy has rotated enough to reach its target direction
					if (Vector3.Dot(transform.right, m_targetDirection) > m_rotationThreshold)
					{
						// set the right vector to exactly face the target direction
						transform.right = m_targetDirection;
						// set the target direction to be facing the target position
						m_targetDirection = m_targetPosition - transform.position;
						m_targetDirection.Normalize();
						++m_currentMoveStep;

						// store the current rotation as the origin rotation
						m_originRotation = transform.rotation;
						// reset the timer
						m_pitchTimer = 0.0f;

						// if the enemy has a field of view, enable it
						if (m_fieldOfView != null)
						{
							m_fieldOfView.GetComponent<MeshRenderer>().enabled = true;
							m_fieldOfView.GetComponent<Collider2D>().enabled = true;
						}
					}
					break;

				// if the enemy is currently rotating to face towards its target
				case MoveStep.rotatingTowardsTarget:
					// rotate towards the target direction
					m_pitchTimer += Time.deltaTime * m_pitchRotationSpeed;
					Vector3 yAxis = Vector3.Cross(m_targetDirection, ((m_targetDirection.x >= 0.0f) ? -1 : 1) * Vector3.forward);
					transform.rotation = Quaternion.Lerp(m_originRotation, Quaternion.LookRotation(m_targetDirection, yAxis) * Quaternion.Euler(0, -90, 0), m_pitchTimer);

					// if the enemy has rotated enough to reach its target direction
					if (Vector3.Dot(transform.right, m_targetDirection) > m_rotationThreshold)
					{
						// set the rotation so the enemy exactly faces the target direction
						transform.rotation = Quaternion.LookRotation(m_targetDirection, yAxis) * Quaternion.Euler(0, -90, 0);
						++m_currentMoveStep;
					}
					break;

				// if the enemy is currently moving towards its target
				case MoveStep.moving:
					// move the enemy closer to the target position
					transform.position = Vector3.MoveTowards(transform.position, m_targetPosition, m_moveSpeed * Time.deltaTime);

					// if the enemy has reached its target position
					if (transform.position == m_targetPosition)
					{
						// switch to the next movement in the m_movements list
						if (m_targetIndex == m_movements.Count - 1)
						{
							m_targetIndex = 0;
						}
						else
						{
							++m_targetIndex;
						}

						// determine the target position
						m_targetPosition = transform.position + m_movements[m_targetIndex].m_offset;

						// if the yaw rotation is to be swapped, set the enemy to start rotating so it is facing horizontally away from the target
						if (m_movements[m_targetIndex].m_swapYawRotation)
						{
							m_targetDirection = (m_targetPosition.x >= transform.position.x) ? Vector3.left : Vector3.right;
							m_currentMoveStep = MoveStep.rotatingTowardsOpposite;
						}
						// if the yaw rotation is not to be swapped, set the enemy to start rotating so it is facing its target position
						else
						{
							m_targetDirection = m_targetPosition - transform.position;
							m_targetDirection.Normalize();
							m_currentMoveStep = MoveStep.rotatingTowardsTarget;
						}

						// store the current rotation as the origin rotation
						m_originRotation = transform.rotation;
						// reset the timer
						m_pitchTimer = 0.0f;
					}
					break;
			}
		}
		print(m_targetIndex);
	}
}
