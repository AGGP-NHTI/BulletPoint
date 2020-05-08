using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManager;

    public static AudioSource MusicSource;

    public AudioClip Music;
    public AudioClip AssaultRifleShot;

    private void Start()
    {
        playMusic();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playAssualtRifle(MusicSource);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {

        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {

        }
    }


    public static void playMusic()
    {
        MusicSource.clip = audioManager.Music;
        MusicSource.Play();
    }

    public static void playAssualtRifle()
    {
        source.clip = audioManager.AssaultRifleShot;
    }

}
