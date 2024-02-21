using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    //Makes Class a Singleton Class.
    #region Singleton
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (Instance == null)
                instance = FindAnyObjectByType(typeof(UIManager)) as UIManager;
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
