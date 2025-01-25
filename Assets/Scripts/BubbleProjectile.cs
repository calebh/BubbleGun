using UnityEngine;

public class BubbleProjectile : MonoBehaviour
{
    public GameObject PlayerOfOrigin;

    public float Speed;
    private Vector3 Direction;

    public void Initialize(Vector3 direction) {
        Direction = direction;
    }

    public void Update() {
        transform.position += (Direction * Speed * Time.deltaTime);
    }
}
