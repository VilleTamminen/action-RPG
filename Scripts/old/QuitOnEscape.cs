using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitOnEscape : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            //Close app when escape is pressed
            Application.Quit();
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }
}

