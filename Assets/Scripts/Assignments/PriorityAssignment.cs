using UnityEngine;

public class PriorityAssignment : Assignment
{
	private Animator animator;
	[SerializeField] private AnimationClip[] clips;
	protected override void Start()
	{
		base.Start();
		animator = GetComponent<Animator>();
		PlayAnimation(this.priority-1);
	}
	public override void Initialize(params object[] additionalArgs)
	{
		int newPriority = 1;
		if(additionalArgs.Length > 0) newPriority = (int)(additionalArgs[0] ?? 1);
		this.priority = newPriority;
	}
	private void PlayAnimation(int index)
	{
		if(index < 0 || index >= clips.Length) return;
		animator.Play(clips[index].name);
	}
}