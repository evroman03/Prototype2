using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInput PlayerInput;
    private Animator _animator;
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

    void Start()
    {
        if(_animator ==null)
        {
            _animator= GetComponent<Animator>();
        }
        PlayerInput.currentActionMap.FindAction("Jump").started += Jump;
        PlayerInput.currentActionMap.FindAction("Jump").canceled += JumpCanceled;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Jump(InputAction.CallbackContext ctx)
    {
        _animator.SetTrigger("TurnLeft");
    }
    void JumpCanceled(InputAction.CallbackContext ctx)
    {
        _animator.ResetTrigger("TurnLeft");
    }
    void UpdatePos()
    {
        transform.parent.position = gameObject.transform.position;
    }
    void UpdateRot()
    {
        transform.parent.rotation= gameObject.transform.rotation;  
    }
}
