using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCutsceneEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CutsceneEvent()
    {
        SceneManager.LoadScene("Scenes/HazardTest");
    }

    public void GoBackToMenu()
    {
        SceneManager.LoadScene("Scenes/Menus");
    }
}
