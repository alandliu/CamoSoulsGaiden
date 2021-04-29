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


    public int turn = 0;
    public int player1Index;
    public int player2Index;

    // Start is called before the first frame update
    void Start()
    {
        
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
            turn++;
        }
        else if (turn == 1) {
            characterSS2[index].SetActive(true);
            nextButton.SetActive(true);
            turn++;
            player2Index = index;
        }
    }

    public void toCombat()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.SetInt("Player1", player1Index);
        PlayerPrefs.SetInt("Player2", player2Index);
    }
}
