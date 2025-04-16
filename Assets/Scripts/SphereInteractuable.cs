using UnityEngine;

public class SphereInteractuable : MonoBehaviour, IInteractuable
{
    [SerializeField] private int sphereID;
    [SerializeField] private PuzzleController puzzleController;
    [SerializeField] private string interactText;
    [SerializeField] private Player player;

    [Header("Colors")]
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private MeshRenderer sphereRenderer;

    [Header("Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip incorrectSound;

    private bool isActivated = false;

    private void Start()
    {
        ResetColor();
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
        if (isActivated) return;

        puzzleController.RegisterInput(sphereID, this);
        player.Interacting = false;
    }

    public void SetColorCorrect()
    {
        sphereRenderer.material = greenMaterial;
        audioSource.PlayOneShot(correctSound);
        isActivated = true;
    }

    public void SetColorIncorrect()
    {
        sphereRenderer.material = redMaterial;
        audioSource.PlayOneShot(incorrectSound);
    }

    public void ResetColor()
    {
        sphereRenderer.material = blueMaterial;
        isActivated = false;
    }
}
