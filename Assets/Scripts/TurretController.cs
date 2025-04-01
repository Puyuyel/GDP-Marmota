using UnityEngine;

public class TurretController : MonoBehaviour
{
    private bool _belongsToLowerCamera;
    private Transform _firePoint;
    private Turret _turretScript;

    void Start()
    {
        _belongsToLowerCamera = transform.position.y < -5;

        _firePoint = transform.Find("FirePoint");
        _turretScript = GetComponent<Turret>();
    }

    void OnEnable()
    {
        AlternateCamera.OnCameraToggle += HandleCameraToggle;
    }

    void OnDisable()
    {
        AlternateCamera.OnCameraToggle -= HandleCameraToggle;
    }

    void HandleCameraToggle(bool isDown)
    {
        bool shouldBeActive = (isDown == _belongsToLowerCamera);

        _turretScript.enabled = shouldBeActive;

        if (_firePoint != null)
        {
            _firePoint.gameObject.SetActive(shouldBeActive);
        }
    }
}
