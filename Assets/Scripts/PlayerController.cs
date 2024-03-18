using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
    public BlockID facingBlock;
    bool isFalling = false;

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
        Application.targetFrameRate = 60;
        BlockID[] allBlocks = FindObjectsOfType<BlockID>();
        blockIDs = new List<BlockID>(allBlocks);
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
        gM = GameManager.Instance;
        PlayerInput.currentActionMap.FindAction("Restart").performed += Restart;
        PlayerInput.currentActionMap.FindAction("Quit").performed += Quit;
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
        bool isTrue = Mathf.Abs(playerPos.x - nextBlockPos.x) < 0.5f && Mathf.Abs(playerPos.z - nextBlockPos.z) < 0.5f;

        if (isTrue)
        {
            facingBlock = block;
        }
        return isTrue;
    }
    public void Action(string actionName) 
    {
        string thisSquare = "";
        string nextSquare = "";
        //string thisSquare = CheckSquareType(transform.parent.position + transform.parent.up * 5);
        //string nextSquare = CheckSquareType(transform.parent.position + transform.parent.forward + transform.parent.up * 5);

        bool isOnABlock = false;
        foreach (BlockID block in blockIDs)
        {
            if(CurrentBlock(block))            // Check if player is on this block
            {
                thisSquare = block.Type.ToString();
                if(thisSquare == "StartBlock")
                {
                    thisSquare = "Ground";
                }
                if(thisSquare == "FinishBlock")
                {
                    SceneManager.LoadScene("EndScene");
                }
                isOnABlock= true;
            }
            if (FacingBlock(block))    // Check if player is facing this block
            {
                nextSquare= block.Type.ToString();
                if (nextSquare == "StartBlock")
                {
                    nextSquare = "Ground";
                }
                if(nextSquare == "FinishBlock")
                {
                    nextSquare = "Ground";
                }
            }
            
        }
        if(!isOnABlock)
        {
            _animator.SetTrigger("Falling");
            SoundManager.Instance.Falling();
        }
            
        if (actionName == "Turn Left Card")
        {  
             _animator.SetTrigger("TurnLeft");
            SoundManager.Instance.Turning();
        }
        else if (actionName == "Turn Right Card")
        {
            _animator.SetTrigger("TurnRight");
            SoundManager.Instance.Turning();
        }
        else if (actionName == "Jump Card")
        {
            switch (thisSquare)
            {
                case "Ground":
                    switch (nextSquare)
                    {
                        case "Ground":
                            //_animator.SetTrigger("Jump");
                            _animator.SetTrigger("JumpAcross");
                            SoundManager.Instance.Jump();
                            break;
                        case "OneBlock":
                            _animator.SetTrigger("Jump");
                            SoundManager.Instance.Jump();
                            break;
                        case "TwoBlock":
                            _animator.SetTrigger("FailJump");
                            SoundManager.Instance.HitWall();
                            break;
                        default:
                            //_animator.SetTrigger("JumpDown");
                            _animator.SetTrigger("JumpAcross");
                            SoundManager.Instance.Jump();
                            break;
                    }
                    break;
                case "OneBlock":
                    switch (nextSquare)
                    {
                        case "Ground":
                            _animator.SetTrigger("JumpDown2");
                            SoundManager.Instance.Jump();
                            break;
                        case "OneBlock":
                            _animator.SetTrigger("Jump");
                            SoundManager.Instance.Jump();
                            break;
                        case "TwoBlock":
                            _animator.SetTrigger("Jump");
                            SoundManager.Instance.Jump();
                            break;
                        default:
                            _animator.SetTrigger("JumpDown");
                            SoundManager.Instance.Jump();
                            break;
                    }
                    break;
                case "TwoBlock":
                    switch (nextSquare)
                    {
                        case "TwoBlock":
                            _animator.SetTrigger("Jump");
                            SoundManager.Instance.Jump();
                            break;
                        default:
                            _animator.SetTrigger("JumpDown2");
                            SoundManager.Instance.Jump();
                            break;
                    }
                    break;
                default:
                    _animator.SetTrigger("Falling");
                    SoundManager.Instance.Falling();
                    break;
            }
        }
        else if (actionName == "Move Card")
        {
            switch (thisSquare)
            {
                case "Ground":
                    switch (nextSquare)
                    {
                        case "Ground":
                            _animator.SetTrigger("Move");
                            SoundManager.Instance.Moving();
                            break;
                        case "OneBlock":
                            _animator.SetTrigger("FailMove");
                            SoundManager.Instance.HitWall();
                            break;
                        case "TwoBlock":
                            _animator.SetTrigger("FailMove");
                            SoundManager.Instance.HitWall();
                            break;
                        default:
                            _animator.SetTrigger("Move");
                            SoundManager.Instance.Moving();
                            break;
                    }
                    break;
                case "OneBlock":
                    switch (nextSquare)
                    {
                        case "Ground":
                            _animator.SetTrigger("Move");
                            SoundManager.Instance.Moving();
                            break;
                        case "OneBlock":
                            _animator.SetTrigger("Move");
                            SoundManager.Instance.Moving();
                            break;
                        case "TwoBlock":
                            _animator.SetTrigger("FailMove");
                            SoundManager.Instance.HitWall();
                            break;
                        default:
                            _animator.SetTrigger("Move");
                            SoundManager.Instance.Moving();
                            break;
                    }
                    break;
                case "TwoBlock":
                    _animator.SetTrigger("Move");
                    SoundManager.Instance.Moving();
                    break;
                default:
                    _animator.SetTrigger("Falling");
                    SoundManager.Instance.Falling();
                    break;
            }
        }
        else if (actionName == "Falling")
        {
            //switch (facingBlock.Type.ToString())
            switch(thisSquare)
            {
                case "Ground":
                    if(transform.parent.position.y > 1.7f)
                    {
                        _animator.SetTrigger("Falling");
                        SoundManager.Instance.Falling();
                    }
                    break;
                case "OneBlock":
                    if (transform.parent.position.y > 2.7f)
                    {
                        _animator.SetTrigger("Falling");
                        SoundManager.Instance.Falling();
                    }
                    break;
                case "TwoBlock":
                    if (transform.parent.position.y > 3.7f)
                    {
                        _animator.SetTrigger("Falling");
                        SoundManager.Instance.Falling();
                    }
                    break;
                default:
                    _animator.SetTrigger("Falling");
                    SoundManager.Instance.Falling();
                    break;
            }
            return;
        }
        foreach (BlockID block in blockIDs)
        {
            if (CurrentBlock(block))            // Check if player is on this block
            {
                thisSquare = block.Type.ToString();
                if (thisSquare == "StartBlock")
                {
                    thisSquare = "Ground";
                }
            }
            if (FacingBlock(block))    // Check if player is facing this block
            {
                nextSquare = block.Type.ToString();
                if (nextSquare == "StartBlock")
                {
                    nextSquare = "Ground";
                }
            }
        }
    }
    void CheckFalling()
    {
        //UpdatePos();
        //transform.parent.position = gameObject.transform.position;
        transform.parent.position = new Vector3(Mathf.RoundToInt(gameObject.transform.position.x), (float)Math.Round(gameObject.transform.position.y, 2), Mathf.RoundToInt(gameObject.transform.position.z));
        //transform.parent.position = new Vector3(facingBlock.location.x, /*facingBlock.location.y+facingBlock.height,*/transform.position.y, facingBlock.location.z);
        Action("Falling");
    }
    void UpdateRot()
    {
        //transform.parent.rotation= gameObject.transform.rotation;  
        Quaternion rotation = gameObject.transform.rotation;
        Vector3 eulerAngles = rotation.eulerAngles;

        // Round each Euler angle to the nearest whole number
        int roundedX = Mathf.RoundToInt(eulerAngles.x);
        int roundedY = Mathf.RoundToInt(eulerAngles.y);
        int roundedZ = Mathf.RoundToInt(eulerAngles.z);

        // Create a new rotation quaternion using the rounded Euler angles
        Quaternion roundedRotation = Quaternion.Euler(roundedX, roundedY, roundedZ);

        // Apply the rounded rotation to the parent transform
        transform.parent.rotation = roundedRotation;
    }
}
