using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeSceneLoader : MonoBehaviour
{
    public Image fadeImage;         // El Image del panel negro
    public float fadeDuration = 1f; // Duración del fade
    public float delayBeforeFade = 3f; // Tiempo antes de empezar el fade
    public string sceneToLoad = "Mine";

    void Start()
    {
        StartCoroutine(FadeAndLoad());
    }

    IEnumerator FadeAndLoad()
    {
        yield return new WaitForSeconds(delayBeforeFade);

        // Asegurar el panel está activo
        fadeImage.gameObject.SetActive(true);

        Color color = fadeImage.color;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // Cargar la nueva escena después del fade
        SceneManager.LoadScene(sceneToLoad);
    }
}