using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guidance.Ending
{
    public class GoodEndSceneController : MonoBehaviour, INextEndingSceneController
    {
        private Animator studentAnimator;
        private EndingBGMManager audioSource;
        private BottomDialogueBox dialogueBox;
        private Fader fader;
        private DialogueBox lastSentence;
        private bool enlighten = false;
        [SerializeField] private GameObject studentPortrait;
        [SerializeField] private GameObject dialogueBoxObj;
        [SerializeField] private GameObject faderObj;
        [SerializeField] private GameObject lastTextObj;
        [SerializeField] private GameObject talkBG;
        [SerializeField] private GameObject skipButtonObj;
        [SerializeField] private AudioClip studentTalkSound;
        [SerializeField] private AudioClip teacherTalkSound;
        void Awake()
        {
            studentAnimator = studentPortrait.GetComponent<Animator>();
            dialogueBox = dialogueBoxObj.GetComponent<BottomDialogueBox>();
            fader = faderObj.GetComponent<Fader>();
            lastSentence = lastTextObj.GetComponent<DialogueBox>();
            audioSource = GetComponent<EndingBGMManager>();
        }
        public IEnumerator Run()
        {
            // #4
            studentPortrait.SetActive(true);
            yield return fader.FadeIn(1f);
            yield return new WaitForSeconds(2f);
            yield return StudentTalk("$4선생님.$");
            yield return TeacherTalk("졸업 축하한단다. 무슨 일 있니?");
            yield return StudentTalk("선생님,$8 $이제 어떻게 해야 할지 모르겠어요.");
            yield return TeacherTalk("모르겠다니? 지금까지 열심히 해 왔잖니?");
            yield return StudentTalk("모르겠어요. 이제 목표가 사라진 것 같은 느낌이에요.");
            yield return StudentTalk("대학을 졸업하면 끝이라고 생각했어요. 지금까지 열심히 해 왔다고요. 졸업을 위해서.");
            yield return StudentTalk("그런데 아직도 더 남아있다니요.$8 $2저는 대체 어떻게 가야 하는 걸까요?$");
            dialogueBoxObj.SetActive(false);

            // #5
            yield return new WaitForSeconds(1f);
            yield return TeacherTalk("$2잠깐 생각해 볼까?");
            yield return StudentTalk("네?\n하지만, 저희에겐 시간이 없는걸요.");
            yield return TeacherTalk("시간이 없어 보일 수는 있어.$8 $하지만, 시간이 없다는 건 시스템이 $4멋대로$ 정한 것일 뿐이야.");
            yield return TeacherTalk("더 깊게, 충분히 생각한다면, 스스로 답을 찾을 수 있을 거야.");
            dialogueBoxObj.SetActive(false);
            studentAnimator.Play("thinking");
            yield return new WaitForSeconds(5f);

            // #6
            enlighten = true;
            audioSource.PlayFade(3, 1f);
            studentAnimator.Play("smile");
            yield return StudentTalk("그래,$4 $2이제 알 것 같아.$4 $내가 $2어떻게 $가야 할지를.");
            yield return TeacherTalk("이제 알 것 같니?");
            yield return StudentTalk("선생님, 감사합니다. 이제 실마리가 풀린 것 같아요.");
            yield return StudentTalk("지금까지는, 시간에 쫓겨서, 계속 답만 갈구했던 것 같아요. 시간은 없고, 어떻게든 성과는 내야만 했으니까.");
            yield return StudentTalk("하지만, 선생님은 시간에 맞는 성과보다는, 생각하는 방법을 알려주신 것 같아요.");
            yield return StudentTalk("선생님 덕분에, 저는 답을 찾을 수 있는 방법을 알게 되었어요. 그리고, 이제 홀로 설 수 있어요.");
            yield return StudentTalk("언젠가, 선생님과 같은 선생이 되어서, 시스템을 바꿀 거에요.");
            yield return StudentTalk("$4감사합니다, 선생님!");
            dialogueBoxObj.SetActive(false);
            yield return fader.WhiteFade(2f);
            studentPortrait.SetActive(false);
            talkBG.GetComponent<SpriteRenderer>().color = Color.white;
            skipButtonObj.GetComponent<TMPro.TextMeshProUGUI>().color = Color.black;
            faderObj.SetActive(false);

            // #7
            yield return TeacherTalk("$2당초에는 대학 졸업까지 학생을 보내는 것만이 목적이었다.");
            yield return TeacherTalk("$2시스템은 시간에 맞춰서 학생을 성인으로 만들기를 원했다. 그리고 나도 이를 성실하게 수행했다.");
            yield return TeacherTalk("$2하지만, 나는 학생을 성인이 아닌, 어른으로 만들고 싶었다. $학생에게 답을 찾는 과정을 진정으로 끌어올리게 하고 싶었다.");
            yield return TeacherTalk("$2그러기 위해서는, 시스템을 거부할 필요가 있었다. 학생이 숙고할 시간을 박탈하는 시스템을.");
            yield return TeacherTalk("$2그리고, 학생이 생각할 힘을 길러주기 위해, 지식의 방향으로 학생을 인도했다.");
            yield return TeacherTalk("$2시간만 준다고 학생이 바로 숙고할 수 있는 것은 아니기 때문이었다.");
            dialogueBoxObj.SetActive(false);
            yield return new WaitForSeconds(1f);
            yield return TeacherTalk("$2우리는 시간이 미덕이라고 생각해 왔다. 그리고, 우리가 빠르고 바쁜 세상을 만들었다.");
            yield return TeacherTalk("$2하지만 돌아온 것은 자립하지 못하는 학생이었다. 우리는 책임을 질 필요가 있다.");
            dialogueBoxObj.SetActive(false);
            lastTextObj.SetActive(true);
            yield return new WaitForSeconds(2f);
            yield return lastSentence.Play("학생을, 어른으로 만들기 위해.", null, 0.2f);
            audioSource.FadeOut(2f);
            yield return new WaitForSeconds(2f);

            GameManager.Instance?.SaveFlag("seen_goodend", true);
        }
        private IEnumerator StudentTalk(string dialogue)
        {
            dialogueBoxObj.SetActive(true);
            studentAnimator.Play("talk");
            yield return dialogueBox.Play(dialogue, studentTalkSound, 0.075f, false);
            if(enlighten)
            {
                studentAnimator.Play("smile");
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                studentAnimator.Play("idle");
                yield return null;
            }
        }
        private IEnumerator TeacherTalk(string dialogue)
        {
            dialogueBoxObj.SetActive(true);
            yield return dialogueBox.Play(dialogue, teacherTalkSound, 0.075f, false);
        }
    }
}