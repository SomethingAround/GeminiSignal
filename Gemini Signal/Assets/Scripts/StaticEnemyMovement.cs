/*
 * File Name: StaticEnemyMovement.cs
 * Author: Michael Sweetman
 * Description: manages the rotation of static enemies
 * Creation Date: 09/10/2019
 * Last Modified: 04/11/2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemyMovement : MonoBehaviour
{
	Vector3 m_startRotation;
	Vector3 m_targetRotation;
	Vector3 m_endRotation;
	float m_rotationThreshold = 0.99999f;

	public float m_endZRotation;
	public float m_rotationSpeed;

    /*
	 * Brief: determines the start and end rotations that the enemy wil cycle between
	 */
    void Start()
    {
		// store the start rotation
		m_startRotation = gameObject.transform.right;

		// determine and store the end rotation
		gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, m_endZRotation);
		m_endRotation = gameObject.transform.right;

		// initialise start conditions
		m_targetRotation = m_endRotation;
		gameObject.transform.right = m_startRotation;
    }

   /*
    * Brief: sets the enemy's rotation each frame
	*/
    void Update()
    {
		// rotate towards the target rotation
		gameObject.transform.right = Vector3.RotateTowards(gameObject.transform.right, m_targetRotation, m_rotationSpeed * Time.deltaTime, 0.0f);

		// if the enemy has reached its target rotation, swap its target
		if (Vector3.Dot(gameObject.transform.right.normalized, m_targetRotation.normalized) > m_rotationThreshold)
		{
			m_targetRotation = (m_targetRotation == m_startRotation) ? m_endRotation : m_startRotation;
		}
	}
}
