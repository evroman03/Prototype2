using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Makes Class a Singleton Class.
    #region Singleton
    private static PlayerController instance;
    public static PlayerController Instance
    {
        get
        {
            if (Instance == null)
                instance = FindAnyObjectByType(typeof(PlayerController)) as PlayerController;
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
