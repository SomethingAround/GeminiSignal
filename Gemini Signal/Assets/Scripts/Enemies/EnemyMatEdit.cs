/*
 * File Name: EnemyMatEdit.cs
 * Author: Michael Sweetman
 * Description: manages the alpha of the enemy materials
 * Creation Date: 20/11/2019
 * Last Modified: 20/11/2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMatEdit : MonoBehaviour
{
	public Material[] m_redEnemyMaterials;
	public Material[] m_blueEnemyMaterials;
	public float m_safeEnemyOpacity = 0.7f;
	public float m_dangerousEnemyOpacity = 1.0f;

	/*
	 * Brief: switches the alpha of the textures of enemies of the same state according to the phase state of the player
	 * Parameter: a_playerPhased: the player's current phase state
	 */
	public void EditEnemyAlpha(bool a_playerPhased)
	{
		// set the alpha of all red enemy textures
		foreach (Material a_material in m_redEnemyMaterials)
		{
			Color newAlpha = a_material.color;
			newAlpha.a = (a_playerPhased) ? m_dangerousEnemyOpacity : m_safeEnemyOpacity;
			a_material.color = newAlpha;
		}

		// set the alpha of all blue enemy textures
		foreach (Material a_material in m_blueEnemyMaterials)
		{
			Color newAlpha = a_material.color;
			newAlpha.a = (a_playerPhased) ? m_safeEnemyOpacity : m_dangerousEnemyOpacity;
			a_material.color = newAlpha;
		}
	}
}
