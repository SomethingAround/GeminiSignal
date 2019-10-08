using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

/*
 * File Name: PlayerMovement.cs
 * Author: Connor Li
 * Description: Manages the player's movement
 * Creation Date: 08/10/2019
 * Last Modified: 08/10/2019
 */

public class PlayerMovement : MonoBehaviour
{
	public float m_moveSpeed = 2.0f;
	public float m_maxSpeed = 5.0f;
	float m_translation = 0.0f;

	Vector2 m_startPosition = Vector2.zero;
	Vector2 m_moveVelocity = Vector2.zero;

	Rigidbody2D m_rb2d;
    // Start is called before the first frame update
    void Start()
    {	
		m_startPosition = gameObject.transform.position;
		m_rb2d = gameObject.GetComponent<Rigidbody2D>();
		m_rb2d.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    // Update is called once per frame
    void Update()
    {
		//Sets m_translation to - or + move speed
		m_translation = Input.GetAxis("Horizontal") * m_moveSpeed;
		//Sets moveVelocities x to m_translation
		m_moveVelocity.x = m_translation;
		//Increases velocity by the 
		m_rb2d.velocity += m_moveVelocity;
		//Clamps the velocities magnitude to the max speed the player can go
		m_rb2d.velocity = Vector2.ClampMagnitude(m_rb2d.velocity, m_maxSpeed);
	}
}
