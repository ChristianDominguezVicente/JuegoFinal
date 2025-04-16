using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fall : MonoBehaviour
{
    [SerializeField] private Image fadeOut;
    [SerializeField] private TextMeshProUGUI dieText;
    [SerializeField] private float durationFade;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        float tiempo = 0f;
        Color screen = fadeOut.color;
        Color textColor = dieText.color;

        dieText.color = new Color(textColor.r, textColor.g, textColor.b, 0f);

        while (tiempo < durationFade)
        {
            tiempo += Time.deltaTime;
            float alpha = Mathf.Clamp01(tiempo / durationFade);
            fadeOut.color = new Color(screen.r, screen.g, screen.b, alpha);
            dieText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
