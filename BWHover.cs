using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BWHover : MonoBehaviour
{
    [SerializeField]
    private GameObject bwSplash;

    [SerializeField]
    private GameObject bwSplash2;

    [SerializeField]
    private CharacterSelect cs;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void mouseHover()
    {
        if (cs.turn == 0)
        {
            bwSplash.SetActive(true);
        }
        else if (cs.turn == 1)
        {
            bwSplash2.SetActive(true);
        }
    }

    public void MouseExit()
    {
        if (cs.turn == 0)
        {
            bwSplash.SetActive(false);
        }
        else if (cs.turn == 1)
        {
            bwSplash2.SetActive(false);
        }
    }

    public void disableBW()
    {
        bwSplash.SetActive(false);
        bwSplash2.SetActive(false);
    }
}
