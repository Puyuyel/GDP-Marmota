using System;
using UnityEngine;

public class Dragger : MonoBehaviour
{
    private Camera _cam;
    private bool _isDragging = false;
    private GameObject draggedTower;

    void Start()
    {
        _cam = Camera.main;
    }

    void Update()
    {
        if(_isDragging && draggedTower != null)
        {
            // Mover la torreta junto con el cursor
            Vector3 mousePosition = _cam.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            draggedTower.transform.position = mousePosition;

            // Soltar la torreta con el click izquierdo
            if(Input.GetMouseButton(0))
            {
                PlaceTower();
            }
        }
    }

    public void StartDragging(GameObject towerPrefab)
    {
        if (_isDragging) return; // Evitar que se arrastre más de una

        _isDragging = true;
        draggedTower = Instantiate(towerPrefab);
    }

    void PlaceTower()
    {
        TurretSlot closestSlot = FindClosestAvailableSlot(draggedTower.transform.position);

        if (closestSlot != null)
        {
            draggedTower.transform.position = closestSlot.transform.position;
            closestSlot.isOccupied = true;
            _isDragging = false;
            draggedTower = null;
        }
        else
        {
            Destroy(draggedTower); // Si no hay slot válido, destruir la torreta
            _isDragging = false;
        }
    }

    private TurretSlot FindClosestAvailableSlot(Vector3 position)
    {
        float minDistance = Mathf.Infinity;
        TurretSlot closest = null;
        TurretSlot[] anchorSlots = FindObjectsByType<TurretSlot>(FindObjectsSortMode.None);

        foreach (TurretSlot slot in anchorSlots)
        {
            if (slot.isOccupied) continue;

            float dist = Vector3.Distance(position, slot.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = slot;
            }
        }

        return (minDistance < 1.5f) ? closest : null; // Solo si está dentro del rango    
    }
}
