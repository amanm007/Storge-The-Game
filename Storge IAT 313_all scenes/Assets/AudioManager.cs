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
}
