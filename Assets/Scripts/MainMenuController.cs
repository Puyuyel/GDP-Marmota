using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    public void IniciarJuego()
    {
        SceneManager.LoadScene("NexusDefense");
    }
    public void SalirDelJuego()
    {
        Application.Quit();
        Debug.Log("Salir del juego");
    }   

}