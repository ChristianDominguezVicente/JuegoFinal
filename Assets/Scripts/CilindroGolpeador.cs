using UnityEngine;

public class CilindroGolpeador : MonoBehaviour
{
    [SerializeField] private float impulseForce;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddTorque(Vector3.up * impulseForce, ForceMode.VelocityChange);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
