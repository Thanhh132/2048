using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource sfxSource;  

    public AudioClip swipeClip;    
    public AudioClip mergeClip;     


    public void PlaySwipe()
    {
        if (swipeClip != null && sfxSource != null) 
            sfxSource.PlayOneShot(swipeClip);
    }

    public void PlayMerge()
    {
        if (mergeClip != null && sfxSource != null) 
            sfxSource.PlayOneShot(mergeClip);
    }
}