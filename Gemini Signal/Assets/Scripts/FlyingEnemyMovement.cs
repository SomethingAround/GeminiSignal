/*
 * File Name: FlyingEnemyMovement.cs
 * Author: Michael Sweetman
 * Description: manages the movement of flying enemies
 * Creation Date: 07/10/2019
 * Last Modified: 15/10/2019
 * Note: Known Bug - turns upside down in non rectangular paths
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyMovement : MonoBehaviour
{
	/*
	 * Brief: structure for the waypoints of the flying enemy
	 */
	[System.Serializable]
	public struct Waypoint
	{ 
		public Vector3 m_position;
		public bool m_swapYRotation;

		/*
		 * Brief: Waypoint constructor
		 * Parameter: a_position: the coordinates of the waypoint
		 * Parameter: a_swapYRotation : determines whether the enemy will have to turn around for this waypoint
		 */
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
			// if the enemy is facing the wrong direction, set the target direction to be left or right, facing away from the target
			else
			{
				m_targetDirection = (m_waypoints[m_targetIndex].m_position.x >= gameObject.transform.position.x) ? m_faceLeft : m_faceRight;
			}
			m_facingOpposite = false;
			m_maneuverCompleted = false;
			m_targetDirection.Normalize();
		}
		
		// if the enemy has turned enough to reach its target direction, determine the next target direction or move towards the target
		if (Vector3.Dot(gameObject.transform.right.normalized, m_targetDirection) > m_rotationThreshold)
		{
			gameObject.transform.right = m_targetDirection;

			// if the y rotation needed to be swapped and the maneuver is not yet completed, determine the next direction to reach for the maneuver
			if (m_waypoints[m_targetIndex].m_swapYRotation && !m_maneuverCompleted)
			{
				// if the enemy was previously facing opposite, rotate towards the target
				if (m_facingOpposite)
				{
					// set the target direction to be towards the target
					m_targetDirection = m_waypoints[m_targetIndex].m_position - transform.position;
					m_targetDirection.Normalize();
					m_maneuverCompleted = true;

					// show the field of view
					m_fieldOfView.GetComponent<MeshRenderer>().enabled = true;
					m_fieldOfView.GetComponent<Collider2D>().enabled = true;
				}
				// if the enemy is starting the maneuver, rotate to be facing left or right, towards the target
				else
				{
					// set the target direction to be left or right, towards the target
					m_targetDirection = (m_waypoints[m_targetIndex].m_position.x >= gameObject.transform.position.x) ? m_faceRight : m_faceLeft;
					m_facingOpposite = true;

					// hide the field of view
					m_fieldOfView.GetComponent<MeshRenderer>().enabled = false;
					m_fieldOfView.GetComponent<Collider2D>().enabled = false;
				}
			}
			// if the rotation didn't need to be swapped or if the maneuver is completed
			else
			{
				// move towards the target
				gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, m_waypoints[m_targetIndex].m_position, m_moveSpeed * Time.deltaTime);
			}
		}
		// if the enemy has not yet reached its target direction, rotate towards the target
		else
		{
			// if the enemy is facing in the opposite direction, rotate around the y axis
			if (m_facingOpposite && !m_maneuverCompleted)
			{
				gameObject.transform.Rotate(gameObject.transform.up, ((m_waypoints[m_targetIndex].m_position.x >= gameObject.transform.position.x) ? -1 : 1) * m_rotationSpeed);
			}
			// otherwise, rotate around the z axis
			else
			{
				gameObject.transform.right = Vector3.RotateTowards(gameObject.transform.right, m_targetDirection, m_rotationSpeed * Time.deltaTime, 0.0f);
			}
		}
	}
}
