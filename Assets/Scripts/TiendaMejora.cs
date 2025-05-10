using System.Collections.Generic;
using UnityEngine;

public class TiendaMejoras : MonoBehaviour
{
    public List<Turret> torretas; // Referencias a torretas colocadas en el domo
    public float mejoraFireRate = 0.9f; // Reduce 10%
    public int mejoraDaño = 5;

    private float fireRateBase = 0.5f;
    private int dañoBase = 20;

    // Llamado por el botón de Mejorar Daño
    public void MejorarDaño()
    {
        dañoBase += mejoraDaño;
        AplicarMejorasATodas();
    }

    // Llamado por el botón de Mejorar Cadencia
    public void MejorarCadencia()
    {
        fireRateBase *= mejoraFireRate;
        AplicarMejorasATodas();
    }

    private void AplicarMejorasATodas()
    {
        foreach (Turret t in torretas)
        {
            if (t != null)
            {
                t.ConfigurarMejoras(fireRateBase, dañoBase);
            }
        }
    }
}