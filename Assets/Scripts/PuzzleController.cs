using System.Collections;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] private int[] sequence = new int[4];
    [SerializeField] private GameObject key;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip puzzleCompleto;

    [Header("Spheres")]
    [SerializeField] private SphereInteractuable[] spheres;

    private int indiceActual = 0;
    private Animator chestAnim;
    private bool isResetting = false;

    private void Awake()
    {
        chestAnim = GetComponent<Animator>();
    }

    public void RegisterInput(int inputID, SphereInteractuable sphere)
    {
        if (isResetting) return;

        if (inputID == sequence[indiceActual])
        {
            sphere.SetColorCorrect();
            indiceActual++;

            if (indiceActual >= sequence.Length)
            {
                chestAnim.SetTrigger("open");
                audioSource.PlayOneShot(puzzleCompleto);
            }
        }
        else
        {
            isResetting = true;
            sphere.SetColorIncorrect();
            StartCoroutine(ResetPuzzleWithDelay());
        }
    }

    private IEnumerator ResetPuzzleWithDelay()
    {
        yield return new WaitForSeconds(1f);

        foreach (var s in spheres)
        {
            s.ResetColor();
        }

        indiceActual = 0;
        isResetting = false;
    }

    public void ActivateKey()
    {
        key.SetActive(true);
    }
}
