using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public PlayerInput PlayerInput;
    public CinemachineVirtualCamera virtualCamera; // Reference to your Cinemachine Virtual Camera
    private bool isMoving;
    float panInput;


    // Start is called before the first frame update

    #region Singleton
    private static CameraController instance;
    public static CameraController Instance
    {
        get
        {
            if (instance == null)
                instance = FindAnyObjectByType(typeof(CameraController)) as CameraController;
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
        PlayerInput.currentActionMap.FindAction("PanCamera").started += PanCamera;
        PlayerInput.currentActionMap.FindAction("PanCamera").canceled += PanCameraCanceled;
    }

    private void Update()
    {
        while(isMoving)
        {
            MoveCamera();
        }
    }

    void PanCamera(InputAction.CallbackContext ctx)
    {
        isMoving = true;
        panInput = ctx.ReadValue<float>();
        Debug.Log("pressed");
    }

    void PanCameraCanceled(InputAction.CallbackContext ctx)
    {
        isMoving = false;
        Debug.Log("released");
    }

    void MoveCamera()
    {

        // Get the CinemachineTrackedDolly from the Virtual Camera
        
        CinemachineTrackedDolly dolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();

        // Adjust the position of the dolly based on input
        dolly.m_PathPosition += panInput/10;
        }
    }
