using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FadeInMusic : MonoBehaviour
{
    public AudioMixerSnapshot fullVoulme;

    // Start is called before the first frame update
    void Start()
    {
        
        fullVoulme.TransitionTo(5f);

    }

}
