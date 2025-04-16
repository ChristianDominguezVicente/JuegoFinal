using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    [SerializeField] private Vector3 velocity;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = velocity;
        InvokeRepeating(nameof(ChangeDirection), 0f, 3f);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeDirection()
    {
        rb.linearVelocity *= -1f;
    }
}
