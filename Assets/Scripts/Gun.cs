using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GunData gunData;
    public Transform firePoint;
    public AudioClip gunShotClip;

    private AudioSource gunAudioSource;
    private LineRenderer lineRenderer;
    private float nextFireTime = 0f;

  private void Awake()
{
    gunAudioSource = GetComponent<AudioSource>();
    if (gunAudioSource == null)
    {
        gunAudioSource = gameObject.AddComponent<AudioSource>();
    }

    if (firePoint != null)
    {
        lineRenderer = firePoint.GetComponent<LineRenderer>();
    }
    
    if (lineRenderer == null)
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    if (lineRenderer != null)
    {
        lineRenderer.enabled = false;
    }
}

    public void Fire()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + gunData.timeBetFire;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (gunShotClip != null && gunAudioSource != null)
        {
            gunAudioSource.PlayOneShot(gunShotClip);
        }

        Vector3 shootOrigin = firePoint != null ? firePoint.position : transform.position;

        Vector3 shootDirection = firePoint != null ? firePoint.forward : transform.forward;

        Ray ray = new Ray(shootOrigin, shootDirection);

        int layerMask = ~LayerMask.GetMask("Player");
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, layerMask);

        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        Vector3 hitPosition = shootOrigin + shootDirection * 100f;

        foreach (var h in hits)
        {
            if (h.collider.transform.root == transform.root) continue;

            hitPosition = h.point;

            IDamageable target = h.collider.GetComponent<IDamageable>();
            if (target != null)
            {
                target.OnDamage(gunData.damage, h.point, h.normal);
            }
            break; 
        }

        StartCoroutine(ShowLaserRoutine(hitPosition));

    }

    private IEnumerator ShowLaserRoutine(Vector3 hitPosition)
    {
        if (lineRenderer != null && firePoint != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitPosition);

            yield return new WaitForSeconds(0.05f);

            lineRenderer.enabled = false;
        }
    }
}