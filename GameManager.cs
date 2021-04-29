using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] characterPrefabs;

    [SerializeField]
    private GameObject[] characterPrefabs2;

    [SerializeField]
    private GameObject spawnPoint1;

    [SerializeField]
    private GameObject spawnPoint2;



    // Start is called before the first frame update
    void Start()
    {
        LoadCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadCharacter()
    {
        int characterIndex1 = PlayerPrefs.GetInt("Player1");
        int characterIndex2 = PlayerPrefs.GetInt("Player2");

        GameObject player1 = Instantiate(characterPrefabs[characterIndex1]);
        GameObject player2 = Instantiate(characterPrefabs2[characterIndex2]);
        player1.transform.position = spawnPoint1.transform.position;
        player2.transform.position = spawnPoint2.transform.position;

        player1.GetComponent<CharacterController>().opponent = player2;
        player2.GetComponent<CharacterControllerP2>().opponent = player1;
    }
}
