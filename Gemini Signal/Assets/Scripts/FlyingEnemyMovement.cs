/*
 * File Name: FlyingEnemyMovement.cs
 * Author: Michael Sweetman
 * Description: manages the movement of flying enemies
 * Creation Date: 07/10/2019
 * Last Modified: 14/10/2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyMovement : MonoBehaviour
{
	[System.Serializable]
	public struct Waypoint
	{ 
		public Vector3 m_position;
		public bool m_swapYRotation;

		public Waypoint(Vector3 a_position, bool a_swapYRotation)
		{
			m_position = a_position;
			m_swapYRotation = a_swapYRotation;
		}
	}

	Vector3 m_targetDirection;
	int m_targetIndex = 1;
	float m_rotationThreshold = 0.999f;
	Transform m_fieldOfView;
	bool m_facingOpposite = false;
	bool m_maneuverCompleted = true;

	Vector3 m_faceLeft = new Vector3(-1.0f, 0.0f, 0.0f);
	Vector3 m_faceRight = new Vector3(1.0f, 0.0f, 0.0f);

	public bool m_swapYAtStart;
	public float m_moveSpeed;
	public float m_rotationSpeed;
	public List<Waypoint> m_waypoints;

	/*
	 * Brief: Initialisation for the enemy
	 */
	void Start()
    {
		// add the enemy's starting position to the start of the positions list
		m_waypoints.Insert(0, new Waypoint(gameObject.transform.position, m_swapYAtStart));
		
		// determine the first target direction
		m_targetDirection = m_waypoints[m_targetIndex].m_position - transform.position;
		m_targetDirection.Normalize();

		// if the enemy has a child object, store it as the field of view so it can be hidden when turning on the Y axis
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
		// if the enemy has reached its target position, determine the direction to get to the next target
		if (m_waypoints[m_targetIndex].m_position == gameObject.transform.position)
		{
			// move to the next target in the list, or to the start of the list if at the end
			if (m_targetIndex == m_waypoints.Count - 1)
			{
				m_targetIndex = 0;
			}
			else
			{
				++m_targetIndex;
			}
			
			// if the enemy is facing in the direction it should, set the target direction to be the direction to the next target
			if (!m_waypoints[m_targetIndex].m_swapYRotation)
			{
				m_targetDirection = m_waypoints[m_targetIndex].m_position - transform.position;
			}
			/// if the enemy is facing the wrong direction, set the target direction to be left or right, depending on where the target is relative to the enemy horizontally
			else
			{
				m_targetDirection = (m_waypoints[m_targetIndex].m_position.x >= gameObject.transform.position.x) ? m_faceLeft : m_faceRight;
			}
			m_facingOpposite = false;
			m_maneuverCompleted = false;
			m_targetDirection.Normalize();
		}
		
		/// if the enemy has turned enough to reach its target direction, move towards the target
		if (Vector3.Dot(gameObject.transform.right.normalized, m_targetDirection) > m_rotationThreshold)
		{
			gameObject.transform.right = m_targetDirection;

			// if the y rotation needed to be swapped
			if (m_waypoints[m_targetIndex].m_swapYRotation && !m_maneuverCompleted)
			{
				if (m_facingOpposite)
				{
					m_targetDirection = m_waypoints[m_targetIndex].m_position - transform.position;
					m_targetDirection.Normalize();
					m_maneuverCompleted = true;
					m_fieldOfView.GetComponent<MeshRenderer>().enabled = true;
					m_fieldOfView.GetComponent<Collider2D>().enabled = true;
				}
				else
				{
					m_targetDirection = (m_waypoints[m_targetIndex].m_position.x >= gameObject.transform.position.x) ? m_faceRight : m_faceLeft;
					m_facingOpposite = true;
					m_fieldOfView.GetComponent<MeshRenderer>().enabled = false;
					m_fieldOfView.GetComponent<Collider2D>().enabled = false;
				}
			}
			else
			{
				gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, m_waypoints[m_targetIndex].m_position, m_moveSpeed * Time.deltaTime);
			}
		}
		// if the enemy has not yet reached its target direction, rotate towards the target
		else
		{
			if (!m_waypoints[m_targetIndex].m_swapYRotation || m_maneuverCompleted)
			{
				gameObject.transform.right = Vector3.RotateTowards(gameObject.transform.right, m_targetDirection, m_rotationSpeed * Time.deltaTime, 0.0f);
			}
			else if (m_facingOpposite)
			{
				gameObject.transform.Rotate(gameObject.transform.up, ((m_waypoints[m_targetIndex].m_position.x >= gameObject.transform.position.x) ? -1 : 1) * m_rotationSpeed);
			}
			else
			{
				gameObject.transform.right = Vector3.RotateTowards(gameObject.transform.right, m_targetDirection, m_rotationSpeed * Time.deltaTime, 0.0f);
			}
		}
	}
}
