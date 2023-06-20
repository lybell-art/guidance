using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guidance.Ending
{
    public class EndingBGMManager : MonoBehaviour
    {
        private AudioSource source;
        [SerializeField] private AudioClip[] bgms;
        void Awake()
        {
            source = GetComponent<AudioSource>();
        }
        public void Play(int trackNo)
        {
            source.clip = bgms[trackNo];
            source.volume = 1f;
            source.Play();
        }
        public void PlayFade(int trackNo, float fadeSpeed)
        {
            StartCoroutine(PlayFadeCoroutine(trackNo, fadeSpeed));
        }
        public void PlayOneShot(AudioClip clip)
        {
            source.PlayOneShot(clip);
        }
        public void FadeOut(float duration)
        {
            StartCoroutine(FadeOutCoroutine(duration));
        }
        public void FullStop()
        {
            source.volume = 0f;
            source.Stop();
        }
        private IEnumerator PlayFadeCoroutine(int trackNo, float fadeSpeed)
        {
            bool hasPreviousBGM = source.isPlaying;
            if(hasPreviousBGM) yield return FadeOutCoroutine(fadeSpeed * 0.5f);
            source.clip = bgms[trackNo];
            source.Play();
            yield return FadeIn(fadeSpeed * (hasPreviousBGM ? 0.5f : 1f));
        }
        public IEnumerator FadeIn(float duration)
        {
            float time = 0;
            while(time < duration)
            {
                source.volume = time / duration;
                time += Time.deltaTime;
                yield return null;
            }
            source.volume = 1f;
        }
        private IEnumerator FadeOutCoroutine(float duration)
        {
            float time = 0;
            while(time < duration)
            {
                source.volume = 1f - time / duration;
                time += Time.deltaTime;
                yield return null;
            }
            source.volume = 0f;
            source.Stop();
        }
    }
}