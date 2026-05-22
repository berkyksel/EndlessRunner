using System;
using UnityEngine;
using UnityEngine.Apple;

public class Player : MonoBehaviour
{
    PlayerInputs playerInput;

    [SerializeField]
    Transform[] LaneTransforms;
    [SerializeField] float MoveSpeed = 20f;
    [SerializeField] float JumpHeight = 2.5f;

    [SerializeField] Transform GroundCheckTransform;
    [SerializeField] [Range(0, 1)] float GroundCheckRadius = 0.2f;
    [SerializeField] LayerMask GroundCheckMask;

    

    int CurrentLaneIndex;

    Vector3 Destination;

    Animator animator;

    Camera playerCamera;
    Vector3 playerCameraOffset;

    // === MOBIL ÝÇÝN DIÞARIDAN ÇAÐRILACAK METOTLAR ===
    public void SwipeLeft()
    {
        if (!IsOnGround()) return;
        MoveLeft();
    }

    public void SwipeRight()
    {
        if (!IsOnGround()) return;
        MoveRight();
    }

    public void SwipeUp()
    {
        if (!IsOnGround()) return;

        Rigidbody rigidBody = GetComponent<Rigidbody>();
        if (rigidBody != null)
        {
            float jumpUpSpeed = Mathf.Sqrt(2 * JumpHeight * Physics.gravity.magnitude);
            rigidBody.AddForce(Vector3.up * jumpUpSpeed, ForceMode.VelocityChange);
        }
    }


    private void OnEnable()
    {
        if (playerInput == null) { 
            playerInput = new PlayerInputs();
        }
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput.gameplay.Move.performed += MovePerformed;
        playerInput.gameplay.Jump.performed += JumpPerformed;
        for (int i = 0; i < LaneTransforms.Length; i++) 
            {
                if (LaneTransforms[i].position == transform.position)
                {
                    CurrentLaneIndex = i;
                    Destination = LaneTransforms[i].position;
                }
        }

        animator = GetComponent<Animator>();

        playerCamera = Camera.main;
        playerCameraOffset = playerCamera.transform.position - transform.position;
    }

    private void JumpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
       

        if (IsOnGround()) 
        {
            Rigidbody rigidBody = GetComponent<Rigidbody>();
            if (rigidBody != null)
            {
                float jumpUpSpeed = Mathf.Sqrt(2 * JumpHeight * Physics.gravity.magnitude);
                rigidBody.AddForce(new Vector3(0.0f, jumpUpSpeed, 0.0f), ForceMode.VelocityChange);
            }
        } 
    }

    private void MovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        

        if (!IsOnGround())
        {
            return;
        }

        float InputValue = obj.ReadValue<float>();
        if (InputValue > 0) {
            MoveRight();
        }
        else
        {
            MoveLeft();
        }

    }

    private void MoveLeft()
    {
        if (CurrentLaneIndex == 0) { 
            return;
        }

        CurrentLaneIndex--;
        Destination = LaneTransforms[CurrentLaneIndex].position;
    }

    private void MoveRight() 
    {
        if (CurrentLaneIndex == LaneTransforms.Length - 1) {
            return;
        }

        CurrentLaneIndex++;
        Destination = LaneTransforms[CurrentLaneIndex].position;

    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOnGround()) 
        {
            animator.SetBool("isOnGround", false);
            //Debug.Log("We are not on ground");
            return;
        }
        animator.SetBool("isOnGround", true);
        //Debug.Log("We are on ground");

        float TransformX = Mathf.Lerp(transform.position.x, Destination.x, Time.deltaTime * MoveSpeed);
        transform.position = new Vector3(TransformX, transform.position.y, transform.position.z);
    }
    private void LateUpdate()
    {
        playerCamera.transform.position = transform.position + playerCameraOffset;
    }
    bool IsOnGround()
    {
        return Physics.CheckSphere(GroundCheckTransform.position, GroundCheckRadius, GroundCheckMask);

    }
}
