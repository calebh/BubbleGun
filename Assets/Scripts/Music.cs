using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioSource Head;
    public AudioSource Body;

    public void Start() {
        Head.PlayScheduled(AudioSettings.dspTime + 0.25);
        Body.PlayScheduled(AudioSettings.dspTime + 0.25 + Head.clip.length);

        DontDestroyOnLoad(gameObject);
    }
}
