using UnityEngine;

public class PlataformaMovilKinematic : MonoBehaviour
{
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float changeDierction;

    private Rigidbody rb;

    private void Awake()
    {
        InvokeRepeating(nameof(ChangeDirection), 0f, changeDierction);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }

    private void ChangeDirection()
    {
        velocity *= -1f;
    }
}
