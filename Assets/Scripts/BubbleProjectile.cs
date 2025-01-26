using UnityEngine;

public class BubbleProjectile : MonoBehaviour
{
    public GameObject PlayerOfOrigin;

    public Transform Mesh;

    public float Speed;
    private Vector3 Direction;

    private PlayerMovement TrappedDuck;
    private float TrappedAlarm;
    private bool TrappedStartIsKinematic = false;
    private float TrappedY = 0.0f;

    public float TrappedDuration = 1.25f;
    public float TrappedScale = 3.0f;

    public AudioClip EscapeClip;

    public void Initialize(GameObject playerOfOrigin, Vector3 direction) {
        Direction = direction;
        PlayerOfOrigin = playerOfOrigin;
    }

    public void Update() {
        Vector3 pos = transform.position;
        pos += (Direction * Speed * Time.deltaTime);
        if (TrappedDuck != null) {
            pos.y = MathUtils.Damp(pos.y, TrappedY, 5.0f, Time.deltaTime);
        }
        transform.position = pos;

        if (TrappedDuck != null) {
            float scale = Mesh.localScale.x;
            scale = MathUtils.Damp(scale, TrappedScale, 3.0f, Time.deltaTime);
            Mesh.localScale = new Vector3(scale, scale, scale);

            TrappedDuck.transform.localPosition = MathUtils.DampVector3(TrappedDuck.transform.localPosition, Vector3.zero, 10.0f, Time.deltaTime);
            TrappedAlarm -= Time.deltaTime;
            if (TrappedAlarm <= 0.0f) {
                TrappedDuck.transform.SetParent(null);
                TrappedDuck.IsTrappedInBubble = false;
                TrappedDuck.gameObject.GetComponent<Rigidbody>().isKinematic = TrappedStartIsKinematic;
                TrappedDuck.gameObject.GetComponent<AudioSource>().PlayOneShot(EscapeClip);
                Destroy(gameObject);
            }
        }
    }

    public void OnTriggerEnter(Collider other) {
        if (TrappedDuck != null) {
            return;
        }

        if (other.gameObject != PlayerOfOrigin) {
            var otherMovement = other.gameObject.GetComponent<PlayerMovement>();
            if (otherMovement != null) {
                otherMovement.IsTrappedInBubble = true;
                TrappedDuck = otherMovement;
                other.gameObject.transform.SetParent(gameObject.transform);
                Speed *= 0.5f;
                TrappedAlarm = TrappedDuration;
                Rigidbody trappedRigidBody = other.gameObject.GetComponent<Rigidbody>();
                TrappedStartIsKinematic = trappedRigidBody.isKinematic;
                trappedRigidBody.isKinematic = true;
                TrappedY = transform.position.y + 2.0f;
            }
        }
    }
}
