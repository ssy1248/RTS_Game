using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    private AudioSource infantryAttackChannel;
    public AudioClip infantryAttackClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        infantryAttackChannel = gameObject.AddComponent<AudioSource>();
        infantryAttackChannel.volume = 0.1f;
        infantryAttackChannel.playOnAwake = false;
    }

    public void PlayInfantryAttackSound()
    {
        if (infantryAttackChannel.isPlaying)
        {
            infantryAttackChannel.Stop(); // 소리가 재생 중이면 중지
        }

        infantryAttackChannel.PlayOneShot(infantryAttackClip); // 소리를 다시 재생
    }
}
