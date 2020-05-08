using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioItem : MonoBehaviour
{
    public AudioClip clip;
    public AudioSource source;



    public void Delete(float time)
    {
        Destroy(gameObject, time);
    }
}
