using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerGun : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public Transform ProjectileSpawnPoint;

    public float FiringInterval = 0.25f;

    private bool Firing = false;

    private float FiringAlarm = 0.0f;

    private PlayerMovement Movement;

    public AudioClip Shoot;

    private AudioSource AudioSource;

    public void Awake() {
        Movement = GetComponent<PlayerMovement>();
        AudioSource = GetComponent<AudioSource>();
    }

    public void OnFire(InputAction.CallbackContext context) {
        Firing = context.action.triggered;
    }

    private void SpawnProjectile() {
        GameObject projectile = Instantiate(ProjectilePrefab, ProjectileSpawnPoint.position, Quaternion.identity);
        projectile.GetComponent<BubbleProjectile>().Initialize(gameObject, transform.forward);
        AudioSource.PlayOneShot(Shoot);
    }

    public void Update() {
        if (FiringAlarm > 0.0f) {
            FiringAlarm -= Time.deltaTime;
        }

        if (Firing && FiringAlarm <= 0.0f && !Movement.IsTrappedInBubble) {
            FiringAlarm = FiringInterval;
            SpawnProjectile();
        }
    }
}
