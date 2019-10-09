using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 *  File Name: Pause Menu
 *  Author: Steven Pham
 *  Description: displays a pause menu that the player either continue the game or go to main menu or quit the game
 *  Creation Date: 8/8/2019
 *  Last Modified : 9/8/2019
*/

public class PauseMenu : MonoBehaviour
{
    /*
     * Start is called before the first frame update
    */
    void Start()
    {
        GameObject.Find("ContinueButton").GetComponentInChildren<Text>().text = "Continue Button";
        GameObject.Find("MainMenuButton").GetComponentInChildren<Text>().text = "Main Menu Button";
        GameObject.Find("QuitButton").GetComponentInChildren<Text>().text = "Quit Button";
    }

    /*
     * Update is called once per frame
    */
    void Update()
    {
        
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Exiting game");
    }

    public void Continue()
    {

    }

    public void MainMenu()
    {
        //Application.LoadLevel("Name of Main Menu");
    }
}
