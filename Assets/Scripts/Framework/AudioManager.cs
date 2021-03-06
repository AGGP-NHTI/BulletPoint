﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public GameObject AudioPrefab;

    public static AudioManager audioManager;

    public AudioSource MusicSource;

    public AudioClip musicClip;

    public AudioClip AssaultRifleShot;
    public AudioClip SniperShot;
    public AudioClip BodyHit;

    bool isMusicPlaying = false;
    private void Start()
    {
        if (!audioManager)
        {
            audioManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Debug.Log(Game.game.currentSceneLoaded);
        if (!isMusicPlaying && Game.game.currentSceneLoaded == 2)
        {
            playMusic();
            isMusicPlaying = true;
        }

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
        audioManager.MusicSource.clip = audioManager.musicClip;
        audioManager.MusicSource.Play();
    }

    public static void playAssualtRifle()
    {
        playSound(audioManager.AssaultRifleShot, 0.2f);
    }

    public static void playSniper()
    {
        playSound(audioManager.SniperShot,0.2f);
    }

    public static void playBodyHit()
    {
        playSound(audioManager.BodyHit, 0.5f);
    }

    public static void playSound(AudioClip clip, float audioDeviation)
    {

        GameObject obj = Instantiate(audioManager.AudioPrefab);
        if (obj)
        {
            AudioClip obj_clip = obj.GetComponent<AudioClip>();

            obj_clip = clip;
            AudioSource source = obj.GetComponent<AudioSource>();
            source.volume = audioDeviation;
            source.clip = obj_clip;

            source.Play();


            source.volume = 1;

            obj.GetComponent<AudioItem>().Delete(obj_clip.length);
        }
    }

}
