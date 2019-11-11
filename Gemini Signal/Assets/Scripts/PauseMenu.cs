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
 *  Last Modified : 4/11/2019
*/

public class PauseMenu : MonoBehaviour
{
    private GameObject m_pauseMenu;
    private GameObject m_swapBar;
    public void Start()
    {
        Time.timeScale = 1;
        m_pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        m_pauseMenu.SetActive(false);
        m_swapBar = GameObject.FindGameObjectWithTag("SwapBar");

    }

    /*
     * Breif: Update is called once per frame
    */
    void Update()
    {
        /*
         * Brief: When they press Escape the pause menu pops up and the game is frozen
        */
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            /*
             * Game is paused and the Pause Menu is displayed
            */
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                m_pauseMenu.SetActive(true);
                m_swapBar.SetActive(false);
            }
            /*
             * Brief: Game is unpaused and the Pause Menu is hidden 
            */
            else if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                m_pauseMenu.SetActive(false);
            }

        }
    }

    /*
     * Brief: Continues the game
    */
    public void Continue()
    {
        //SceneManager.LoadSceneAsync("GameLevel", LoadSceneMode.Single);
        Time.timeScale = 1;
        m_pauseMenu.SetActive(false);
        m_swapBar.SetActive(true);

    }

    /*
     * Breif: Load the Main Menu
    */
    public void MainMenu()
    {
        //Goes to the Main Menu
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
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
}
