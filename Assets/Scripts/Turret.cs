using UnityEngine;
using System.Collections;
using System.IO;

public class Turret : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public float detectionRange = 5f;

    private Coroutine _shootingCoroutine;
    private int _damage;
    private Transform _prevTarget;

    private void Start()
    {
        Projectile projectilePrefabScript = projectilePrefab.GetComponent<Projectile>();
        _damage = projectilePrefabScript.damage;
        _prevTarget = null;

        if (!IsVisible(transform.position))
        {
            enabled = false;
        }
    }

    void OnEnable()
    {
        _shootingCoroutine = StartCoroutine(ShootAtEnemies());
    }

    void OnDisable()
    {
        if (_shootingCoroutine != null)
        {
            StopCoroutine(_shootingCoroutine);
            _shootingCoroutine = null;
        }
    }

    IEnumerator ShootAtEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            Transform target = FindClosestEnemy();
            if(target != null && target != _prevTarget)
            {
                _prevTarget = target;

                Enemy enemyScript = target.GetComponent<Enemy>();
                int projectilesNeeded = Mathf.CeilToInt((float)enemyScript.health / _damage);

                for (int i = 0; i < projectilesNeeded; i++)
                {
                    if(!enemyScript.ReserveDamage(_damage)) break;
                    Shoot(target);
                    if (i == projectilesNeeded - 1) break;
                    yield return new WaitForSeconds(fireRate);
                }
            }
        }
    }

    void Shoot(Transform target)
    {
        if (target != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Projectile projectileScript = projectile.GetComponent<Projectile>();

            if (projectileScript != null)
            {
                projectileScript.SetTarget(target);
            }
        }
    }
    Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            if (!IsVisible(enemy.transform.position) || enemy.transform == _prevTarget) continue;

            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance && distance <= detectionRange)
            {
                shortestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }

    bool IsVisible(Vector3 position)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return false;

        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(position);

        return viewportPoint.x > 0 && viewportPoint.x < 1 &&
               viewportPoint.y > 0 && viewportPoint.y < 1 &&
               viewportPoint.z > 0;
    }

    public void ConfigurarMejoras(float velocidadDisparoBase, int dañoBase)
    {
        fireRate = velocidadDisparoBase;
        _damage = dañoBase;
    }
}
