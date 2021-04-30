using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class P1WinToScreen : MonoBehaviour
{
    public void backToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
