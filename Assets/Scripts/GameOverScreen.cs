using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
            SceneManager.LoadScene("Scenes/Menus");
    }
}
