using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public PlayerInput PlayerInput;
    private GameManager gM;
    private Animator _animator;
    public List<BlockID> blockIDs;

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
        BlockID[] allBlocks = FindObjectsOfType<BlockID>();
        blockIDs = new List<BlockID>(allBlocks);
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
        gM = GameManager.Instance;
        //PlayerInput.currentActionMap.FindAction("Jump").started += Jump;
        //PlayerInput.currentActionMap.FindAction("Jump").canceled += JumpCanceled;
        PlayerInput.currentActionMap.FindAction("Restart").performed += Restart;
        PlayerInput.currentActionMap.FindAction("Quit").performed += Quit;
    }
    void Update()
    {
        //Debug.DrawLine(transform.parent.position, transform.parent.position + transform.parent.forward + transform.parent.up * 5, Color.red);
        /*
         if (Input.GetKeyDown(KeyCode.Space))
         {
             _animator.SetTrigger("Falling");
         }*/
    }
    public void Restart(InputAction.CallbackContext ctx)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Quit(InputAction.CallbackContext ctx)
    {
        Application.Quit();
    }
    public void CheckIfHasAction()
    {
        if(gM.gameState==GameManager.STATE.Lv1)
        {
            gM.PlaySequence();
        }    
    }
    bool CurrentBlock(BlockID block)
    {
        // Check if player's position is close enough to block's position
        Vector3 blockPos = block.location;
        Vector3 playerPos = transform.parent.position;
        float tolerance = 0.25f;
        return (Mathf.Abs(playerPos.x - blockPos.x) < tolerance) && (Mathf.Abs(playerPos.z - blockPos.z) < tolerance); // Adjust this threshold as needed
    }

    bool FacingBlock(BlockID block)
    {
        Vector3 blockPos = block.location;
        Vector3 playerPos = transform.parent.position;

        // Calculate the position of the block the player is facing
        Vector3 nextBlockPos = blockPos + -transform.parent.forward;

        return Mathf.Abs(playerPos.x - nextBlockPos.x) < 0.5f && Mathf.Abs(playerPos.z - nextBlockPos.z) < 0.5f;
    }
    public void Action(string actionName)
    {
        string thisSquare = "";
        string nextSquare = "";
        //string thisSquare = CheckSquareType(transform.parent.position + transform.parent.up * 5);
        //string nextSquare = CheckSquareType(transform.parent.position + transform.parent.forward + transform.parent.up * 5);
        foreach (BlockID block in blockIDs)
        {
            if(CurrentBlock(block))            // Check if player is on this block
            {
                thisSquare = block.Type.ToString();
                if(thisSquare == "StartBlock")
                {
                    thisSquare = "Ground";
                }
            }

            if (FacingBlock(block))    // Check if player is facing this block
            {
                nextSquare= block.Type.ToString();
                if (nextSquare == "StartBlock")
                {
                    nextSquare = "Ground";
                }
            }
        }
            
        if (actionName == "Turn Left Card")
        {  
             _animator.SetTrigger("TurnLeft");
        }
        else if (actionName == "Turn Right Card")
        {
            _animator.SetTrigger("TurnRight");
        }
        else if (actionName == "Jump Card")
        {
            switch (thisSquare)
            {
                case "Ground":
                    switch (nextSquare)
                    {
                        case "Ground":
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
        else if (actionName == "Move Card")
        {
            //POYOprint("HERE");
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
                            print("FOUNDONEBLCOK");
                            break;
                        case "TwoBlock":
                            _animator.SetTrigger("FailMove");
                            //POYOprint("FOUNDTWOBLCOK");
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
                            print("FOUNDTWOBLCOK");
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
        else if (actionName == "Falling")
        {
            _animator.SetTrigger("Falling");
        }
        print("Card: " + actionName + " ThisSquare: "+ thisSquare + " Next Square: " + nextSquare);
    }

    /*
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
        Action("Falling");
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
