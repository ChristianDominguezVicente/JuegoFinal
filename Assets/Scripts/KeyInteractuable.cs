using UnityEngine;

public class KeyInteractuable : MonoBehaviour, IInteractuable
{
    [SerializeField] private string interactText;
    [SerializeField] private float rotationVelocity;
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
            player.KeyCount++;
            gameObject.SetActive(false);
            player.Interacting = false;
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationVelocity * Time.deltaTime);
    }
}
