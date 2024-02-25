using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInput PlayerInput;
    private Animator _animator;
    private const string HoleTag = "Hole";
    private const string GroundTag = "Ground";
    private const string OneBlockTag = "OneBlock";
    private const string TwoBlockTag = "TwoBlock";
    //Makes Class a Singleton Class.
    #region Singleton
    private static PlayerController instance;
    public static PlayerController Instance
    {
        get
        {
            if (instance == null)
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
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
        PlayerInput.currentActionMap.FindAction("Jump").started += Jump;
        PlayerInput.currentActionMap.FindAction("Jump").canceled += JumpCanceled;
    }
    public void Action(string actionName)
    {
        CheckNextSquare();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private string CheckNextSquare()
    {
        RaycastHit hit;
        Vector3 parentScan = transform.parent.position+ transform.parent.forward * 1f;
        Debug.DrawRay(parentScan, transform.parent.up*0.1f, Color.red);
        if(Physics.Raycast(parentScan, transform.parent.up * 0.1f, out hit))
        {
            string temp = hit.collider.tag.ToString();
            switch(temp)
            {
                case GroundTag:
                    return (GroundTag);
                case OneBlockTag:
                    return (OneBlockTag);
                case TwoBlockTag:
                    return (TwoBlockTag);
                default:
                    return (HoleTag);
            }
        }
        return ("ERROR");
    }
    void Jump(InputAction.CallbackContext ctx)
    {
        
    }
    void JumpCanceled(InputAction.CallbackContext ctx)
    {
       
    }
    void CheckFalling()
    {
        UpdatePos();
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
