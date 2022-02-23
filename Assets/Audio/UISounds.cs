using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISounds : MonoBehaviour
{
    private AudioSource audioSource;

    public void UIClickedRandom()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = (Random.Range(0.7f, 1.3f));
        audioSource.Play();
    }
}
