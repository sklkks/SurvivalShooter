using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public static readonly int IdDeath = Animator.StringToHash("Death");
    public Image uiHealth;

    public AudioClip hitClip;
    public AudioClip deathClip;

    private AudioSource playerAudioSource;
    private Animator playerAnimator;

    private PlayerMovement playerMovement;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerAudioSource = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerInput = GetComponent<PlayerInput>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
        
        if (playerInput != null)
        {
            playerInput.enabled = true;
        }

        if (uiHealth != null)
        {
            uiHealth.gameObject.SetActive(true);
            uiHealth.fillAmount = Health / startingHealth;
        }
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!IsDead)
        {
            playerAudioSource.PlayOneShot(hitClip);
        }

        base.OnDamage(damage, hitPoint, hitNormal);
        uiHealth.fillAmount = Health / startingHealth;
    }

    public override void Death()
    {
        base.Death();
      
        playerAnimator.SetTrigger(IdDeath);
        playerAudioSource.PlayOneShot(deathClip);

        playerMovement.enabled = false;
        playerInput.enabled = false;
        uiHealth.gameObject.SetActive(false);
        
        LookAt lookAt = GetComponent<LookAt>();
        if (lookAt != null)
        {
            lookAt.enabled = false;
        }

        PlayerShooter playerShooter = GetComponent<PlayerShooter>();
        if (playerShooter != null)
        {
            playerShooter.enabled = false;
        }

        UIManager uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.SetActiveGameOverUI(true);
        }
    }
}
