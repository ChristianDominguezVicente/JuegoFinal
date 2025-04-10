using UnityEngine;

public class KeyInteractuable : MonoBehaviour, IInteractuable
{
    [SerializeField] private string interactText;
    [SerializeField] private float rotationVelocity = 50f;
    [SerializeField] private Player player;

    public string GetInteractText()
    {
        return interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Interact(Transform interactorTransform)
    {
        if (player != null)
        {
            player.HasKey = true;
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationVelocity * Time.deltaTime);
    }
}
