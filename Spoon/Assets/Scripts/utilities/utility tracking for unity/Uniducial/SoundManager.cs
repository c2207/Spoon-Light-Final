using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // 1

    public AudioClip pickUp; // 2
    public AudioClip dropTrash;
    public AudioClip knifeCut;
    public AudioClip fryPan;
    //public AudioClip sheepHitClip; // 3

    private Vector3 cameraPosition; // 5

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this; // 1
        cameraPosition = Camera.main.transform.position; // 2

    }

    private void PlaySound(AudioClip clip) // 1
    {
        AudioSource.PlayClipAtPoint(clip, cameraPosition); // 2
    }

    /// 
    public void PlayPickUp()
    {
        PlaySound(pickUp);
    }
    ///
    public void PlayDropTrash()
    {
        PlaySound(dropTrash);
    }

    public void PlayKnifeCut()
    {
        PlaySound(knifeCut);
    }

    public void PlayFryPan()
    {
        PlaySound(fryPan);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
