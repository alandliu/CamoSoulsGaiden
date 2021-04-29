using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] characterPrefabs;

    [SerializeField]


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadCharacter()
    {
        int characterIndex1 = PlayerPrefs.GetInt("Player1");
        int characterIndex2 = PlayerPrefs.GetInt("Player2");
    }
}
