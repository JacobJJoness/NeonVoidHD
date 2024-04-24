using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] musicTracks; // Assign these in the inspector
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayMusic());
    }

    IEnumerator PlayMusic()
    {
        foreach (AudioClip clip in musicTracks)
        {
            audioSource.clip = clip;
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
        }
    }
}
