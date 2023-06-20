using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guidance.Ending
{
	public class BadEndSceneController : MonoBehaviour, INextEndingSceneController
	{
		private Animator studentAnimator;
		private EndingBGMManager audioSource;
		private BottomDialogueBox dialogueBox;
		private Fader fader;
		private DialogueBox lastSentence;
		private int selection = 0;
		[SerializeField] private GameObject studentPortrait;
		[SerializeField] private GameObject dialogueBoxObj;
		[SerializeField] private GameObject faderObj;
		[SerializeField] private GameObject selectionObj;
		[SerializeField] private GameObject lastTextObj;
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
			yield return StudentTalk("그런데 아직도 더 남아있다니요.$8 $2저는 대체 어디로 가야 하는 걸까요?$");
			dialogueBoxObj.SetActive(false);

			// #5
			selectionObj.SetActive(true);
			yield return new WaitUntil( ()=>selection != 0 );
			selectionObj.SetActive(false);
			yield return TeacherTalk(PlayerAnswer(selection));
			dialogueBoxObj.SetActive(false);

			// #6
			yield return new WaitForSeconds(1f);
			yield return StudentTalk("아니에요. 전부 원론적이고 피상적인 것들뿐이에요. 말이야 좋죠. 하지만 제가 뭘 해야 할지 $4구체적으로$ 결정해주지 않아요.");
			yield return StudentTalk("이건 그냥 성공한 사람들이 하던 것들을 끼워맞춘 것 뿐이라고요.");
			yield return StudentTalk("저는 졸업만 하면 행복하게 살 줄 알았어요.$8 $근데 취직을 하라네요? 앞으로 가야 할 길이 더 많다네요?");
			yield return StudentTalk("그래서 행복하게 살았습니다는$4...$8 $저에겐 왜 없는 건데요.");
			yield return TeacherTalk("지금까지 과제를 따라가면서 스스로 나아갔잖니. 난 가이드만 제공했을 뿐, 졸업까지 가는 건 너의 노력이었어.");
			yield return StudentTalk("과제는 그저 주어지니까 했던 거였죠. 이제 과제가 없잖아요. 아무도, 심지어 선생님조차도 저에게 졸업 후의 세계를 알려주지 않았어요.");
			yield return StudentTalk("그리고 졸업 후에 어디로 가라고 알려주지도 않았죠. $2이제 전 $4어떻게 해야 하는 걸까요...$");
			dialogueBoxObj.SetActive(false);
			audioSource.FadeOut(2f);
			yield return fader.Fade(2f);
			studentPortrait.SetActive(false);
			faderObj.SetActive(false);

			// #7
			yield return TeacherTalk("$2교육은 실패했다. 이번에도 학생을 끝까지 이끌 수 없었다. 무엇이 문제였을까?");
			yield return TeacherTalk("$2학생을 올바른 방향으로 이끌지 못한 나의 문제였는가, 아니면 나의 지도를 따라가지 못한 학생의 문제였는가.");
			yield return TeacherTalk("$2아니다. 학생도, 선생도, 모두 시스템 하에서 주어진 역할을 성실하게 수행했을 뿐이다.");
			yield return TeacherTalk("$2그럼에도 불구하고 실패할 수 밖에 없는 것은,$8 $학생이 '$4무엇을$2'을 갈구할 수 밖에 없는 형태로 자랄 수밖에 없었던 것은,");
			dialogueBoxObj.SetActive(false);
			lastTextObj.SetActive(true);
			yield return new WaitForSeconds(2f);
			yield return lastSentence.Play("시스템.", null, 0.5f);
			yield return new WaitForSeconds(2f);

			GameManager.Instance?.SaveFlag("seen_badend", true);
		}
		public void SetAnswer(int selection)
		{
			this.selection = selection;
		}
		private IEnumerator StudentTalk(string dialogue)
		{
			dialogueBoxObj.SetActive(true);
			studentAnimator.Play("talk");
			yield return dialogueBox.Play(dialogue, studentTalkSound, 0.075f, false);
			studentAnimator.Play("idle");
			yield return null;
		}
		private IEnumerator TeacherTalk(string dialogue)
		{
			dialogueBoxObj.SetActive(true);
			yield return dialogueBox.Play(dialogue, teacherTalkSound, 0.075f, false);
		}
		private string PlayerAnswer(int selection)
		{
			switch (selection)
			{
				case 1: return "이해해. 목표가 불분명해질 수도 있는 거지. 하지만 그럴 때일수록 새로운 기회를 찾을 수도 있을 거야.";
				case 2: return "졸업은 끝이지만, 새로운 시작이기도 해. 이제부터는 다양한 경험과 도전으로 자신의 잠재력을 더 발휘할 수 있을 거야.";
				case 3: return "네가 좋아하는 것과 잘하는 것을 찾아 봐. 그걸 목표로 하면 되는 거지.";
			}
			return "...";
		}
	}
}