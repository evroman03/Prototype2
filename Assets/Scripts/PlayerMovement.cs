using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Makes Class a Singleton Class.
    #region Singleton
    private static PlayerMovement instance;
    public static PlayerMovement Instance
    {
        get
        {
            if (Instance == null)
                instance = FindAnyObjectByType(typeof(PlayerMovement)) as PlayerMovement;
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
