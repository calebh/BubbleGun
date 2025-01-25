using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGun : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public Transform ProjectileSpawnPoint;

    public float FiringInterval = 0.25f;

    private bool Firing = false;

    private float FiringAlarm = 0.0f;

    public void OnFire(InputAction.CallbackContext context) {
        Firing = context.action.triggered;
    }

    private void SpawnProjectile() {
        GameObject projectile = Instantiate(ProjectilePrefab, ProjectileSpawnPoint.position, Quaternion.identity);
        projectile.GetComponent<BubbleProjectile>().Initialize(transform.forward);
    }

    public void Update() {
        if (FiringAlarm > 0.0f) {
            FiringAlarm -= Time.deltaTime;
        }

        if (Firing && FiringAlarm <= 0.0f) {
            FiringAlarm = FiringInterval;
            SpawnProjectile();
        }
    }
}
