using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAutoLoader : MonoBehaviour

{
    public string sceneName = "Mine"; // Cambio a escena Mine
    public float delay = 5f; // Tiempo en segundos antes de cambiar de escena

    void Start()
    {
        Invoke("LoadScene", delay);
    }

    void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}