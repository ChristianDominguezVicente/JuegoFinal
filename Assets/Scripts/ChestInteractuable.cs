using UnityEngine;

public class ChestInteractuable : MonoBehaviour, IInteractuable
{
    [SerializeField] private string interactText;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

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
        throw new System.NotImplementedException();
    }
}
