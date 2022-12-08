using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    //Constants that limit the zoom amount
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;

    float moveSpeed;
    float rotationSpeed;
    float zoomAmount;
    [SerializeField] Vector3 moveVector;
    [SerializeField] Vector3 rotationVector;
    [SerializeField] Vector3 inputMoveDir;
    [SerializeField] Vector3 followOffset;

    [SerializeField] Vector3 targetFollowOffset; //The zoom that we are going to reach
    CinemachineTransposer cinemachineTransposer;
    float zoomSpeed;

    private void Start()
    {
        moveSpeed = 10f;
        rotationSpeed = 100f;
        zoomAmount= 1f;
        zoomSpeed = 5f;

        //The offset property needs to be accessed like this in the cinemachine...
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }
    private void Update()
    {
        //Movement of camera control
        HandleMovement();


        //Rotation of camera control
        HandleRotation();


        //Zoom Control
        HandleZoom();
    }

    private void HandleZoom()
    {
        if (Input.mouseScrollDelta.y > 0f)
        {
            targetFollowOffset.y -= zoomAmount;
        }
        if (Input.mouseScrollDelta.y < 0f)
        {
            targetFollowOffset.y += zoomAmount;
        }
        //Clamp the zoom and then apply it
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
        //To make the movement smooth, we use Lerp, taking the current zoom, to the target zoom
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
    }

    private void HandleRotation()
    {
        rotationVector = Vector3.zero;
        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y = +1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y = -1f;
        }
        transform.eulerAngles += rotationSpeed * Time.deltaTime * rotationVector;
    }

    private void HandleMovement()
    {
        inputMoveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z = +1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = +1f;
        }

        //Need to set the move Vector, otherwise we don't take into account the camera rotation when moving "forward" with W for example, and would move "forward" on the world.
        moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveSpeed * Time.deltaTime * moveVector;
    }
}
