using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float factorGravedad;
    [SerializeField] private float alturaDeSalto;
    [SerializeField] private Transform camara;
    [SerializeField] private InputManagerSO inputManager;

    [Header("Detección suelo")]
    [SerializeField] private Transform pies;
    [SerializeField] private float radioDeteccion;
    [SerializeField] private LayerMask queEsSuelo;

    [Header("Interact")]
    [SerializeField] private float interactRange = 2f;

    private CharacterController controller;
    private Animator anim;
    private Vector3 direccionMovimiento;
    private Vector3 direccionInput;
    private Vector3 velocidadVertical;
    private Rigidbody[] rbs;
    private bool hasKey = false;

    public bool HasKey { get => hasKey; set => hasKey = value; }

    private void Awake()
    {
        rbs = GetComponents<Rigidbody>();
        ChangeBoneState(true);
    }

    private void OnEnable()
    {
        inputManager.OnSaltar += Saltar;
        inputManager.OnMover += Mover;
        inputManager.OnInteract += Interact;
    }

    // Solo se va a ejecutar cuando se actualice el input de movimiento
    private void Mover(Vector2 ctx)
    {
        direccionInput = new Vector3(ctx.x, 0, ctx.y);
    }

    private void Saltar()
    {
        if (EstoyEnSuelo())
        {
            velocidadVertical.y = Mathf.Sqrt(-2 * factorGravedad * alturaDeSalto);
            anim.SetTrigger("jump");
        }
    }
    private void Interact()
    {
        IInteractuable interactuable = GetInteractuable();
        if (interactuable != null)
        {
            interactuable.Interact(transform);
        }
    }

    public IInteractuable GetInteractuable()
    {
        List<IInteractuable> interactuableList = new List<IInteractuable>();
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange); // detecta todos los colliders en el radio
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out IInteractuable interactuable))
            {
                interactuableList.Add(interactuable);
            }
        }

        IInteractuable closestInteractuable = null;
        foreach (IInteractuable interactuable in interactuableList)
        {
            if (closestInteractuable == null)
            {
                closestInteractuable = interactuable;
            }
            else
            {
                if (Vector3.Distance(transform.position, interactuable.GetTransform().position) <
                    Vector3.Distance(transform.position, closestInteractuable.GetTransform().position))
                {
                    closestInteractuable = interactuable;
                }
            }
        }

        return closestInteractuable;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        direccionMovimiento = camara.forward * direccionInput.z + camara.right * direccionInput.x;
        direccionMovimiento.y = 0;
        controller.Move(direccionMovimiento * velocidadMovimiento * Time.deltaTime);
        anim.SetFloat("velocidad", controller.velocity.magnitude);

        if(direccionMovimiento.sqrMagnitude > 0)
        {
            RotarHaciaDestino();
        }

        //Si hemos aterrizado..
        if(EstoyEnSuelo() && velocidadVertical.y < 0)
        {
            //Entonces, reseteo mi velocidad vertical.
            velocidadVertical.y = 0;
            anim.ResetTrigger("jump");
        }
        AplicarGravedad();
    }

    private void AplicarGravedad()
    {
        velocidadVertical.y += factorGravedad * Time.deltaTime;
        controller.Move(velocidadVertical * Time.deltaTime);
    }

    private bool EstoyEnSuelo()
    {
        return Physics.CheckSphere(pies.position, radioDeteccion, queEsSuelo);
    }

    private void RotarHaciaDestino()
    {
        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);
        transform.rotation = rotacionObjetivo;
    }

    private void ChangeBoneState(bool state)
    {
        foreach (var bone in rbs)
        {
            bone.isKinematic = state;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(pies.position, radioDeteccion);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
