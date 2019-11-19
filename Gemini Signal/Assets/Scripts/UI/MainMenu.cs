/*
 * File Name: MainMenu.cs
 * Author: Michael Sweetman
 * Description: manages the events triggered by the buttons in the main menu
 * Creation Date: 15/10/2019
 * Last Modified: 15/10/2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
	/*
	 * Brief: Loads the game level scene
	 */
	public void StartButton()
	{
		// load the game level scene
		SceneManager.LoadSceneAsync("GameLevel", LoadSceneMode.Single);
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
