using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CharacterSelect : MonoBehaviour
{
    [SerializeField]
    private GameObject[] characterSS;

    [SerializeField]
    private GameObject[] characterSS2;

    [SerializeField]
    private GameObject nextButton;

    public string[] start = new string[] {"kS", "rS", "aS"};


    public int turn = 0;
    public int player1Index;
    public int player2Index;

    // Start is called before the first frame update
    void Start()
    {
        //FindObjectOfType<AudioManager>().Play("Music");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ChangeCharacter(int index)
    {


        for (int i = 0; i < characterSS.Length; i++)
        {
            if (turn == 0) characterSS[i].SetActive(false);
            else  if (turn == 1) characterSS2[i].SetActive(false);
        }

        if (turn == 0)
        {
            characterSS[index].SetActive(true);
            player1Index = index;
            if (player1Index > 2) FindObjectOfType<AudioManager>().Play(start[player1Index - 1]);
            else if (player1Index < 2) FindObjectOfType<AudioManager>().Play(start[player1Index]);
            turn++;
        }
        else if (turn == 1) {
            characterSS2[index].SetActive(true);
            nextButton.SetActive(true);
            turn++;
            player2Index = index;

            if (player2Index > 2) FindObjectOfType<AudioManager>().Play(start[player2Index - 1]);
            else if (player2Index < 2) FindObjectOfType<AudioManager>().Play(start[player2Index]);
        }
    }

    public void toCombat()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.SetInt("Player1", player1Index);
        PlayerPrefs.SetInt("Player2", player2Index);
    }
}
