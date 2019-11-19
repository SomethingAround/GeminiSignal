/*
 * File Name: VictoryScreen.cs
 * Author: Michael Sweetman
 * Description: manages the events triggered by the buttons in the victory screen
 * Creation Date: 15/10/2019
 * Last Modified: 15/10/2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class VictoryScreen : MonoBehaviour
{
	/*
	 * Brief: Loads the main menu scene
	 */
	public void BackToMainMenu()
	{
		// load the main menu scene
		SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
	}

	/*
	 * Brief: Quits the game
	 */
	public void QuitButton()
	{
		// quit the application
		Application.Quit();
	}
}