using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public AudioClip gunShotClip;
    public Transform firePoint;

    public float fireRate = 0.1f;
    private float nextFireTime = 0f;

    private AudioSource gunAudioSource;

    private void Awake()
    {
        gunAudioSource = GetComponent<AudioSource>();
        if (gunAudioSource == null)
        {
            gunAudioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + fireRate;
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        // 총소리 재생
        if (gunShotClip != null && gunAudioSource != null)
        {
            gunAudioSource.PlayOneShot(gunShotClip);
        }
        
        Debug.Log("타타탕! 연사 중...");
    }
}