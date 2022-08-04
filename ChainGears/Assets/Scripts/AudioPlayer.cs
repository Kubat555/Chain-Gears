using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private GameObject engineAudioGameObject;

    public static AudioPlayer instance;
    AudioSource audio;

    void Start()
    {

        audio = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audio.PlayOneShot(clip);
    }

    public void StopSound()
    {
        StartCoroutine(TimerForStopSound());
    }

    IEnumerator TimerForStopSound()
    {
        yield return new WaitForSeconds(0.3f);
        audio.Stop();
        engineAudioGameObject.SetActive(false);
    }
}
