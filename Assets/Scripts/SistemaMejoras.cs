using System.Collections.Generic;
using UnityEngine;

public class MejoraTorreta : MonoBehaviour
{
    public GameObject torretaPrefab;
    public Transform[] posicionesTorreta;
    private List<GameObject> torretas = new List<GameObject>();

    public int da�oBase = 10;
    public float velocidadDisparoBase = 1f;

    public int costoAgregarTorreta = 200;
    public int costoMejorarDa�o = 150;
    public int costoMejorarVelocidad = 150;

    public void AgregarTorreta()
    {
        PlayerStats stats = FindFirstObjectByType<PlayerStats>();
        if (stats.GastarDinero(costoAgregarTorreta) && torretas.Count < posicionesTorreta.Length)
        {
            GameObject nueva = Instantiate(torretaPrefab, posicionesTorreta[torretas.Count].position, Quaternion.identity);
            torretas.Add(nueva);
            nueva.GetComponent<Turret>().ConfigurarMejoras(velocidadDisparoBase, da�oBase);
        }
    }

    public void MejorarDa�o()
    {
        PlayerStats stats = FindFirstObjectByType<PlayerStats>();
        if (stats.GastarDinero(costoMejorarDa�o))
        {
            da�oBase += 5;
            ActualizarTorretas();
        }
    }

    public void MejorarVelocidad()
    {
        PlayerStats stats = FindFirstObjectByType<PlayerStats>();
        if (stats.GastarDinero(costoMejorarVelocidad))
        {
            velocidadDisparoBase *= 0.9f; // disparamos m�s r�pido
            ActualizarTorretas();
        }
    }

    private void ActualizarTorretas()
    {
        foreach (GameObject torreta in torretas)
        {
            torreta.GetComponent<Turret>().ConfigurarMejoras(velocidadDisparoBase, da�oBase);
        }
    }
}