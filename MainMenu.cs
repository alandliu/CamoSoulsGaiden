using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private void Start()
    {
        FindObjectOfType<AudioManager>().Play("Music");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        // Check
        Debug.Log("Quit");
        Application.Quit();
    }

    public void secret()
    {
        // If you found this by script fuck you
        SceneManager.LoadScene(5);
    }
}
