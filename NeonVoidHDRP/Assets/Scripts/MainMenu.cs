using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioClip[] musicTracks; // Array to hold your music tracks
    private AudioSource audioSource;

    void Start()
    {
        // Configure the AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false; // Set loop to false to play through the playlist

        // Start playing the music playlist
        StartCoroutine(PlayMusicPlaylist());
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1); // Load your game scene
    }

    private IEnumerator PlayMusicPlaylist()
    {
        int trackIndex = 0; // Start with the first track

        while (true) // Infinite loop to continuously play music
        {
            audioSource.clip = musicTracks[trackIndex];
            audioSource.Play();

            // Wait for the current track to finish before moving to the next one
            yield return new WaitForSeconds(audioSource.clip.length);

            // Increment track index, loop back to the first track if at the end of the playlist
            trackIndex = (trackIndex + 1) % musicTracks.Length;
        }
    }
}
