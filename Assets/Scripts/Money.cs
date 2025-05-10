using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int loot = 1000;

    public bool GastarDinero(int cantidad)
    {
        if (loot >= cantidad)
        {
            loot -= cantidad;
            return true;
        }
        return false;
    }
}
