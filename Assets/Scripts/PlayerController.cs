using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public PlayerInput PlayerInput;
    private Animator _animator;
    private Dictionary<string, string> actionToAnimationMap = new Dictionary<string, string>
    {
        {"Move Card", "Move"},
        {"Jump Card", "Jump"},
        {"Turn Left Card", "TurnLeft"},
        {"Turn Right Card", "TurnRight"}
        // Add more action names and corresponding animations if we need to
    };
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
        //PlayerInput.currentActionMap.FindAction("Jump").started += Jump;
        //PlayerInput.currentActionMap.FindAction("Jump").canceled += JumpCanceled;
        PlayerInput.currentActionMap.FindAction("Restart").performed += Restart;
        PlayerInput.currentActionMap.FindAction("Quit").performed += Quit;
    }
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetTrigger("Falling");
        }
    }
    public void Restart(InputAction.CallbackContext ctx)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Quit(InputAction.CallbackContext ctx)
    {
        Application.Quit();
    }
    public void Action(string actionName)
    {
        if (actionToAnimationMap.ContainsKey(actionName))
        {
            string animName = actionToAnimationMap[actionName];
            string thisSquare = CheckSquareType(transform.parent.position + transform.parent.up * 3);
            string nextSquare = CheckSquareType(transform.parent.position + transform.parent.forward + transform.parent.up * 3);
            
            if (actionName == "Turn Left Card" || actionName == "Turn Right Card")
            {
                _animator.SetTrigger(animName);
                return; // Exit early; turning doesn't involve square type logic
            }
            else if(actionName == "Jump Card")
            {
                switch (thisSquare)
                {
                    case "Ground":
                        switch (nextSquare)
                        {
                            case "Ground" :
                                _animator.SetTrigger("Jump");
                                break;
                            case "OneBlock":
                                _animator.SetTrigger("Jump");
                                break;
                            case "TwoBlock":
                                _animator.SetTrigger("FailJump");
                                break;
                            default:
                                _animator.SetTrigger("JumpDown");
                                break;
                        }
                        break;
                    case "OneBlock":
                        switch (nextSquare)
                        {
                            case "Ground":
                                _animator.SetTrigger("JumpDown");
                                break;
                            case "OneBlock":
                                _animator.SetTrigger("Jump");
                                break;
                            case "TwoBlock":
                                _animator.SetTrigger("Jump");
                                break;
                            default:
                                _animator.SetTrigger("JumpDown");
                                break;
                        }
                        break;
                    case "TwoBlock":
                        switch (nextSquare)
                        {
                            case "TwoBlock":
                                _animator.SetTrigger("Jump");
                                break;
                            default:
                                _animator.SetTrigger("JumpDown");
                                break;
                        }
                        break;
                    default:
                        _animator.SetTrigger("Falling");
                        break;
                }
            }
            else if(actionName == "Move Card")
            {
                switch (thisSquare)
                {
                    case "Ground":
                        switch (nextSquare)
                        {
                            case "Ground":
                                _animator.SetTrigger("Move");
                                break;
                            case "OneBlock":
                                _animator.SetTrigger("FailMove");
                                break;
                            case "TwoBlock":
                                _animator.SetTrigger("FailMove");
                                break;
                            default:
                                _animator.SetTrigger("Move");
                                break;
                        }
                        break;
                    case "OneBlock":
                        switch (nextSquare)
                        {
                            case "Ground":
                                _animator.SetTrigger("Move");
                                break;
                            case "OneBlock":
                                _animator.SetTrigger("Move");
                                break;
                            case "TwoBlock":
                                _animator.SetTrigger("FailMove");
                                break;
                            default:
                                _animator.SetTrigger("Move");
                                break;
                        }
                        break;
                    case "TwoBlock":
                        _animator.SetTrigger("Move");
                        break;
                    default:
                        _animator.SetTrigger("Falling");
                        break;
                }
            }
        }
        else
        {
            Debug.LogError("MEGA ERROR: Invalid action name or misspelling");
        }
    }


    private string CheckSquareType(Vector3 offset)
    {
        RaycastHit hit;
        Vector3 parentScan = transform.parent.position + offset;

        if (Physics.Raycast(parentScan, -transform.parent.up, out hit, 3f))
        {
            return hit.collider.tag.ToString();
        }
        return "";
    }
    /*
    void Jump(InputAction.CallbackContext ctx)
    {
        _animator.SetTrigger("Jump");
    }
    void JumpCanceled(InputAction.CallbackContext ctx)
    {
        _animator.ResetTrigger("Jump");
    }*/
    void CheckFalling()
    {
        UpdatePos();
        _animator.ResetTrigger("Falling");
        if (CheckSquareType(transform.parent.position + transform.parent.up).Equals("Tile"))
        {
            return;
        }
        //_animator.SetTrigger("Falling");
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
