using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    //IMPORTANTE: Si queremos que rote junto con la camara --> aplicar el FirstPersonController.

    [Header("Movement")]
    [SerializeField] private float movementForce;
    [SerializeField] private float rotationSmoothFactor;

    [Header("Ground Detection")]
    [SerializeField] private Transform feet;
    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask whatIsGround;

    private Vector3 lastMovementDirection;

    private Animator anim;
    private Camera cam;
    private PlayerInput playerInput;
    private Vector2 input;
    private float currentSpeed;

    private float rotationVelocity;

    private Rigidbody rb;

    private bool canControl = true;

    private RagdollSystem ragdollSystem;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        ragdollSystem = GetComponent<RagdollSystem>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        playerInput.actions["Move"].performed += UpdateMovement;
        playerInput.actions["Move"].canceled += UpdateMovement;
        playerInput.actions["Jump"].started += Jump;
    }

    private void UpdateMovement(InputAction.CallbackContext ctx)
    {
        input = ctx.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (canControl)
        {
            MoveAndRotate();
        }
    }

    private void MoveAndRotate()
    {
        // Si hay input...
        if(input.sqrMagnitude > 0)
        {
            // Se calcula el angulo en base a los inputs y (+) la camara
            float angleToRotate = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;

            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angleToRotate, ref rotationVelocity, rotationSmoothFactor);

            // Se aplica dicha rotacion al cuerpo
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

            // Se rota el vector (0, 0, 1) a dicho angulo
            Vector3 movementInput = Quaternion.Euler(0, angleToRotate, 0) * Vector3.forward;


            //Añado la fuerza de forma libre
            rb.AddForce(movementInput * movementForce, ForceMode.Force);

            //Represento mi vector en el plano XZ
            Vector3 XZMovement = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

            //Limito dicho vector
            Vector3 XZmvementClamped = Vector3.ClampMagnitude(XZMovement, 7);

            //Sobreescribo mi velocidad con esa limitación
            rb.linearVelocity = new Vector3(XZmvementClamped.x, rb.linearVelocity.y, XZmvementClamped.z);
        }

        currentSpeed = rb.linearVelocity.magnitude;

        anim.SetFloat("velocidad", currentSpeed / movementForce);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Cilindro"))
        {
            if (other.relativeVelocity.magnitude > 10)
            {
                rb.linearVelocity = Vector3.zero;
                rb.constraints = RigidbodyConstraints.None;
                ragdollSystem.ToggleRagdollState(false);
                canControl = false;
            }
        }
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        //if (isGrounded)
        //{
            
        //}
    }

    private void OnDisable()
    {
        playerInput.actions["Move"].performed -= UpdateMovement;
        playerInput.actions["Move"].canceled -= UpdateMovement;
        playerInput.actions["Jump"].started -= Jump;
    }
}
