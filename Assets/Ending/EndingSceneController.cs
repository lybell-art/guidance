using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guidance.Ending
{
    public class EndingSceneController : MonoBehaviour
    {
        private Animator student1Animator;
        private Animator motherAnimator;
        private Animator fatherAnimator;
        private SpeechBubble bubble;
        private EndingCameraController cam;

        private EndingBGMManager audioPlayer;
        private GameObject[] projectiles;
        private StudentHitter student1Hitter;
        private StudentFaller student1Faller;
        private INextEndingSceneController subController;
        private Fader fader;
        private IEnumerator endingSceneCoroutine;

        [SerializeField] private GameObject cameraObj;
        [SerializeField] private GameObject student1;
        [SerializeField] private GameObject mother;
        [SerializeField] private GameObject father;
        [SerializeField] private GameObject flashObj;
        [SerializeField] private GameObject speechBubbleObj;
        [SerializeField] private GameObject resultUIObj;

        [SerializeField] private AudioClip[] talkSfx;
        [SerializeField] private AudioClip[] pianoSfx;

        void Awake()
        {
            student1Animator = student1.GetComponent<Animator>();
            motherAnimator = mother.GetComponent<Animator>();
            fatherAnimator = father.GetComponent<Animator>();
            bubble = speechBubbleObj.GetComponent<SpeechBubble>();
            cam = cameraObj.GetComponent<EndingCameraController>();
            projectiles = new GameObject[]{
                GameObject.Find("그래서"),
                GameObject.Find("언제"),
                GameObject.Find("취직할"),
                GameObject.Find("거니"),
            };
            student1Hitter = student1.GetComponent<StudentHitter>();
            student1Faller = student1.GetComponent<StudentFaller>();
            audioPlayer = GetComponent<EndingBGMManager>();
            subController = GetComponent<INextEndingSceneController>();
            fader = GameObject.Find(Constants.faderName).GetComponent<Fader>();
        }

        void Start()
        {
            endingSceneCoroutine = PlayCommonEndingScene();
            StartCoroutine(endingSceneCoroutine);
        }

        public void SkipEnding()
        {
            StopCoroutine(endingSceneCoroutine);
            audioPlayer.FullStop();
            GameObject[] allEndingObjects = GameObject.FindGameObjectsWithTag("EndingObject");
            foreach(GameObject obj in allEndingObjects)
            {
                obj.SetActive(false);
            }
            foreach(GameObject projectile in projectiles)
            {
                if(projectile == null || projectile.Equals(null)) continue;
                projectile.SetActive(false);
            }
            ShowResultUI();
        }

        private IEnumerator PlayCommonEndingScene()
        {
            // #1
            student1.transform.position = new Vector3(-2f, -2.5f, 0f);
            cam.Cut(0f, 0f, 4.5f);
            audioPlayer.PlayFade(0, 2f);
            yield return new WaitForSeconds(2f);
            yield return Talk(mother, "그동안 공부하느라 수고\n많았어. 졸업 축하해.");
            yield return Talk(father, "드디어 졸업을 했구나!\n네가 너무 자랑스럽단다.");
            yield return Talk(student1, "고마워요, 엄마, 아빠.");
            yield return Talk(father, "지금까지 열심히 노력했으니\n이제 좋은 일이 있을 거야.");
            yield return Talk(student1, "그랬으면 좋겠네요.");

            // #2
            yield return new WaitForSeconds(0.5f);
            cam.Cut(0.875f, -2f, 2.5f);
            yield return new WaitForSeconds(0.8f);
            audioPlayer.FullStop();
            student1Animator.Play("shock");
            yield return TalkEvil(mother, "그래서, 언제 취직할 거니?");
            audioPlayer.Play(1);
            yield return ShootProjectile();

            // #3
            yield return FallStudent();
            yield return LookMonoCity();
            yield return null;

            // #4
            audioPlayer.PlayFade(2, 1f);
            cam.Cut(50f, 0f, 4.5f);
            yield return subController?.Run();

            // #ending result
            fader.Show();
            ShowResultUI();
            yield return fader.FadeIn(1f);
        }

        private void ShowResultUI()
        {
            resultUIObj.SetActive(true);
            GameManager.Instance?.ApplySave();
        }

        private IEnumerator Talk(GameObject speaker, string dialogue)
        {
            Animator animator = speaker.GetComponent<Animator>();
            if(animator != null) animator.Play("talk");
            speechBubbleObj.SetActive(true);
            bubble.MoveToGameObject(speaker);
            yield return bubble.Play(dialogue, GetTalkSound(speaker));
            if(animator != null) animator.Play("idle");
            yield return null;
        }

        private IEnumerator TalkEvil(GameObject speaker, string dialogue)
        {
            Animator animator = speaker.GetComponent<Animator>();
            if(animator != null) animator.Play("evil");
            speechBubbleObj.SetActive(true);
            bubble.MoveToGameObject(speaker);
            bubble.SetColor(SpeechBubble.red);
            yield return bubble.Play(dialogue, GetTalkSound(speaker), 0.3f, false);
            if(animator != null) animator.Play("evilIdle");
            bubble.SetColor(Color.white);
            yield return null;
        }

        private IEnumerator ShootProjectile()
        {
            WaitForSeconds interval = new WaitForSeconds(0.75f);
            foreach(GameObject projectile in projectiles)
            {
                projectile.SetActive(true);
            }
            foreach(GameObject projectile in projectiles)
            {
                projectile.GetComponent<SpeechProjectile>().Shoot(student1.transform);
                yield return interval;
            }
            speechBubbleObj.SetActive(false);
            yield return new WaitUntil(() => student1Hitter.fullyHit);
            flashObj.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            flashObj.SetActive(false);
        }

        private IEnumerator FallStudent()
        {
            student1Faller.StartFall();
            cam.Follow(student1);
            yield return new WaitUntil(() => student1Faller.onGrounded);
            cam.EndFollow();
        }

        private IEnumerator LookMonoCity()
        {
            WaitForSeconds camDelay = new WaitForSeconds(1f);

            yield return new WaitForSeconds(2.5f);
            student1Animator.Play("monoTurn");
            yield return new WaitForSeconds(1f);
            audioPlayer.PlayOneShot(pianoSfx[0]);
            yield return cam.Pan(6f, -38, 0.3f);
            yield return camDelay;
            audioPlayer.PlayOneShot(pianoSfx[1]);
            yield return cam.Pan(-7.5f, -39, 0.3f);
            yield return camDelay;
            audioPlayer.PlayOneShot(pianoSfx[2]);
            yield return cam.Pan(11.5f, -38, 0.3f);
            yield return camDelay;
            audioPlayer.PlayOneShot(pianoSfx[3]);
            yield return cam.Pan(-11.5f, -38, 0.3f);
            yield return new WaitForSeconds(2f);
            student1Animator.Play("monoPanic");
            audioPlayer.PlayOneShot(pianoSfx[4]);
            cam.Cut(0, -42.5f, 4f);
            cam.MakeTransition(0f, -40, 6f, 5f);
            yield return new WaitForSeconds(4f);
            yield return fader.Fade(1f);
        }

        private AudioClip GetTalkSound(GameObject obj)
        {
            if(obj == father) return talkSfx[1];
            if(obj == mother) return talkSfx[2];
            return talkSfx[0];
        }
    }
}
