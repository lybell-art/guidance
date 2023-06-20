using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentSoundManager : MonoBehaviour
{
    private bool isWalkingSoundPlaying = false;
    [SerializeField] private AudioSource sfxPlayer;
    [SerializeField] private AudioSource walkPlayer;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landSound;
    [SerializeField] private AudioClip launchSound;
    [SerializeField] private AudioClip getItemSound;
    [SerializeField] private AudioClip walkSound;

    void Awake()
    {
        //audioPlayer = GetComponent<AudioSource>();
    }
    public void PlayJump()
    {
        if(sfxPlayer == null || jumpSound == null) return;
        sfxPlayer.PlayOneShot(jumpSound);
    }
    public void PlayLand()
    {
        if(sfxPlayer == null || landSound == null) return;
        sfxPlayer.PlayOneShot(landSound);
    }
    public void PlayLaunch()
    {
        if(sfxPlayer == null || launchSound == null) return;
        sfxPlayer.PlayOneShot(launchSound);
    }
    public void PlayGetItem()
    {
        if(sfxPlayer == null || getItemSound == null) return;
        sfxPlayer.PlayOneShot(getItemSound);
    }
    public void PlayWalk()
    {
        if(walkPlayer == null || walkSound == null) return;
        if(!isWalkingSoundPlaying)
        {
            walkPlayer.clip = walkSound;
            walkPlayer.Play();
            isWalkingSoundPlaying = true;
        }
    }
    public void EndWalk()
    {
        if(walkPlayer == null) return;
        if(isWalkingSoundPlaying)
        {
            walkPlayer.Stop();
            isWalkingSoundPlaying = false;
        }
    }
}
