using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guidance.Ending
{
    public class StudentHitter : MonoBehaviour
    {
        [SerializeField] private AudioClip hitSFX;
        [SerializeField] private AudioClip bigHitSFX;
        private Animator animator;
        private AudioSource audioSource;
        private int hitCount = 0;
        public bool fullyHit
        {
            get { return hitCount >= 4; }
        }
        void Awake()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }
        public void HitProjectile()
        {
            hitCount++;
            if(hitCount < 4)
            {
                animator.Play("hit");
                audioSource.PlayOneShot(hitSFX);
            }
            else audioSource.PlayOneShot(bigHitSFX);
        }
    }
}

