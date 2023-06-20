using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deliberator : MonoBehaviour
{
	private IDeliberateTaskGenerator deliberateGenerator;
	private IScoreManager scoreManager;
	private IAssignmentRunner runner;
	private IEnumerator currentDeliberation;
	private Animator animator;
	private Animator bubbleAnimator;
	[SerializeField] private int wizdom = 0;

	[SerializeField] private GameObject scoreManagerObj;
	[SerializeField] private GameObject goalObj;

	void Awake()
	{
		runner = GetComponent<IAssignmentRunner>();
		deliberateGenerator = goalObj.GetComponent<IDeliberateTaskGenerator>();
		scoreManager = scoreManagerObj.GetComponent<IScoreManager>();
		animator = GetComponent<Animator>();
		bubbleAnimator = transform.GetChild(0)?.GetComponent<Animator>();
	}
	void Start()
	{
		StartDeliberation();
	}
	void OnEnable()
	{
		if(runner == null) return;
		runner.onTaskFilled += CancelDeliberation;
		runner.onTaskEmpty += StartDeliberation;
	}
	void OnDisable()
	{
		if(runner == null) return;
		runner.onTaskFilled -= CancelDeliberation;
		runner.onTaskEmpty -= StartDeliberation;
	}
	public void AddWizdom()
	{
		scoreManager?.AddWizdom();
		wizdom++;
	}
	public void StartDeliberation()
	{
		currentDeliberation = Deliberate();
		StartCoroutine(currentDeliberation);
	}
	public void CancelDeliberation()
	{
		ResetAnimationTrigger();
		if(currentDeliberation != null) StopCoroutine(currentDeliberation);
	}
	private IEnumerator Deliberate()
	{
		yield return new WaitForSecondsPausable(2f);
		bubbleAnimator.ResetTrigger("blank");
		if(wizdom == 0) animator.SetBool("rest", true);
		else animator.SetBool("thinking", true);

		if(wizdom == 0)
		{
			bubbleAnimator.SetTrigger("gaming");
		}
		else if(wizdom == 1)
		{
			bubbleAnimator.SetTrigger("confused1");
			yield return new WaitForSecondsPausable(1f);
			animator.SetTrigger("dontknow");
			yield return new WaitForSecondsPausable(1f);
			animator.SetBool("thinking", false);
		}
		else if(wizdom == 2)
		{
			bubbleAnimator.SetTrigger("confused2");
			yield return new WaitForSecondsPausable(3f);
			animator.SetTrigger("dontknow");
			yield return new WaitForSecondsPausable(1f);
			animator.SetBool("thinking", false);
		}
		else
		{
			float speedMult = GetDeliberateSpeedMult(wizdom);
			bubbleAnimator.SetFloat("deliberateSpeed", speedMult);
			bubbleAnimator.SetTrigger("deliberate");
			yield return new WaitForSecondsPausable(5f / speedMult);
			animator.SetBool("thinking", false);
			runner.AssignTask(deliberateGenerator.deliberateAssignment);
		}
	}
	private void ResetAnimationTrigger()
	{
		bubbleAnimator.SetTrigger("blank");
		bubbleAnimator.ResetTrigger("confused1");
		bubbleAnimator.ResetTrigger("confused2");
		bubbleAnimator.ResetTrigger("deliberate");
		animator.SetBool("rest", false);
		animator.SetBool("thinking", false);
		animator.ResetTrigger("dontknow");
	}
	private float GetDeliberateSpeedMult(int wizdom)
	{
		if(wizdom <= 3) return 1f;
		if(wizdom >= Constants.maxWizdomCount) return 1.8f;
		return 1f + 0.1f * (wizdom - 3);
	}
}