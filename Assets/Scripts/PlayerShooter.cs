using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public AudioClip gunShotClip;
    public Transform firePoint;

    public float fireRate = 0.1f;    
    public Gun gun;
    
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
            if (gun != null)
            {
                gun.Fire();
            }
        }
    }    
}