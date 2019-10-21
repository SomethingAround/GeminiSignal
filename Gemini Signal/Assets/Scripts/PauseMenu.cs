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
    /*
     * Start is called before the first frame update
    */
    void Start()
    {
    }

    /*
     * Update is called once per frame
    */
    void Update()
    {
        //When they press Escape the pause menu pops up and the game is frozen
        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("Escape key was pressed");
        }
    }

    public void Quit()
    {
        //Quits the game
        Application.Quit();
        Debug.Log("Exiting game");
    }

    public void Continue()
    {

    }

    public void MainMenu()
    {
        //Goes to the Main Menu
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
    }
}
