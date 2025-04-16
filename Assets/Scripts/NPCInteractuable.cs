using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class NPCInteractuable : MonoBehaviour, IInteractuable
{
    [SerializeField] private string interactText;
    [SerializeField, TextArea(1, 5)] private string[] frases;
    [SerializeField] private float tiempoEntreLetras;
    [SerializeField] private GameObject cuadroDialogo;
    [SerializeField] private TextMeshProUGUI textoDialogo;
    [SerializeField] private Player player;

    private bool hablando = false;
    private int indiceActual = -1;
    private Animator anim;

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

        player.Interacting = false;
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
}
