using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 *  File Name: Pause Menu
 *  Author: Steven Pham
 *  Description: displays a pause menu that the player either continue the game or go to main menu or quit the game
 *  Creation Date: 8/8/2019
 *  Last Modified : 21/8/2019
*/

public class PauseMenu : MonoBehaviour
{
    public Canvas pauseMenu;
    public void Start()
    {
        Time.timeScale = 1;
        pauseMenu = GetComponent<Canvas>();
        pauseMenu.enabled = false;
    }
    /*
     * Breif: Update is called once per frame
    */
    void Update()
    {
        //When they press Escape the pause menu pops up and the game is frozen
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                pauseMenu.enabled = true;
            }
            else if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                pauseMenu.enabled = false;
            }

        }
    }

    /*
     * Breif: Quits the game
    */
    public void Quit()
    {
        //Quits the game
        Application.Quit();
        Debug.Log("Exiting game");
    }
    /*
     * Continues the game
    */
    public void Continue()
    {
        SceneManager.LoadSceneAsync("GameLevel", LoadSceneMode.Single);
    }

    /*
     * Breif: Load the Main Menu
    */
    public void MainMenu()
    {
        //Goes to the Main Menu
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
    }
}
