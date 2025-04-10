using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class BossInteractuable : MonoBehaviour, IInteractuable
{
    [SerializeField] private string interactText;
    [SerializeField, TextArea(1, 5)] private string[] frasesSinKey;
    [SerializeField, TextArea(1, 5)] private string[] frasesConKey;
    [SerializeField] private float tiempoEntreLetras;
    [SerializeField] private GameObject cuadroDialogo;
    [SerializeField] private TextMeshProUGUI textoDialogo;
    [SerializeField] private Player player;
    [SerializeField] private Image fadeOut;
    [SerializeField] private float durationFade = 2f;

    private bool hablando = false;
    private int indiceActual = -1;
    private Animator anim;
    private string[] frases;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {

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
        cuadroDialogo.SetActive(true);
        if(!hablando)
        {
            if (indiceActual == -1)
            {
                anim.SetBool("talking", true);
            }

            if (player.HasKey)
            {
                frases = frasesConKey;
            }
            else
            {
                frases = frasesSinKey;
            }
            SiguienteFrase();
        }
        else
        {
            CompletarFrase();
        }
    }

    private void SiguienteFrase()
    {
        indiceActual++;
        if(indiceActual >= frases.Length)
        {
            TerminarDialogo();
        }
        else
        {
            StartCoroutine(EscribirFrase());
        }
    }

    private void TerminarDialogo()
    {
        hablando = false;
        textoDialogo.text = "";
        indiceActual = -1;
        cuadroDialogo.SetActive(false);
        anim.SetBool("talking", false);

        if (player.HasKey)
        {
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator EscribirFrase()
    {
        hablando = true;
        textoDialogo.text = "";
        //Subdividir la frase en caracteres.
        char[] caracteresFrase = frases[indiceActual].ToCharArray();
        foreach (char caracter in caracteresFrase)
        {
            textoDialogo.text += caracter;
            yield return new WaitForSeconds(tiempoEntreLetras);
        }
        hablando = false;
    }

    private void CompletarFrase()
    {
        StopAllCoroutines();
        textoDialogo.text = frases[indiceActual];
        hablando = false;
    }

    private IEnumerator FadeOut()
    {
        float tiempo = 0f;
        Color screen = fadeOut.color;

        while (tiempo < durationFade)
        {
            tiempo += Time.deltaTime;
            float alpha = Mathf.Clamp01(tiempo / durationFade);
            fadeOut.color = new Color(screen.r, screen.g, screen.b, alpha);
            yield return null;
        }

        SceneManager.LoadScene("Level2");
    }
}
