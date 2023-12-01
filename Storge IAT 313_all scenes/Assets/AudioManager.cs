using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip background;
    public AudioClip footsteps;
    public AudioClip hurt;
    public AudioClip demon_attack;
    public AudioClip gramohphone;
    public AudioClip grandfatherclock;
    public AudioClip tendril;








    private void Start()
    {
        

    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);

    }
    public void PlaySFXWithPitch(AudioClip clip, float pitch)
    {

        SFXSource.pitch = pitch;
        SFXSource.PlayOneShot(clip);
    }
    public void PlaySFXWithLowSound(AudioClip clip, float volume)
    {

        SFXSource.volume = volume;
        SFXSource.PlayOneShot(clip);
    }
    public void PlaySFXAtLocation(AudioClip clip, Vector3 position, float volume = .5f, float spatialBlend = 1f)
    {
        // Create a temporary game object to play the clip
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = position;

        // Add an AudioSource to the temporary game object
        AudioSource audioSource = tempGO.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.spatialBlend = spatialBlend; // Set to 1 for full 3D sound

        // Play the clip
        audioSource.Play();

        // Destroy the temporary game object after the clip has finished
        Destroy(tempGO, clip.length);
    }
    public void PlaySFXAtLocationwithLowerVol(AudioClip clip, Vector3 position, float volume = .3f, float spatialBlend = 1f)
    {
        // Create a temporary game object to play the clip
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = position;

        // Add an AudioSource to the temporary game object
        AudioSource audioSource = tempGO.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.spatialBlend = spatialBlend; // Set to 1 for full 3D sound

        // Play the clip
        audioSource.Play();
        audioSource.loop = true;



        // Destroy the temporary game object after the clip has finished
        //Destroy(tempGO, clip.length);
    }
}
