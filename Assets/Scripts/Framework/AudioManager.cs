using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public GameObject AudioPrefab;

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
            playAssualtRifle();
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
        playSound(audioManager.AssaultRifleShot);
    }

    public static void playSound(AudioClip clip)
    {

        GameObject obj = Instantiate(audioManager.AudioPrefab);
        AudioClip obj_clip = obj.GetComponent<AudioClip>();

        obj_clip = clip;
        AudioSource source = obj.GetComponent<AudioSource>();

        source.clip = obj_clip;
    }

}
