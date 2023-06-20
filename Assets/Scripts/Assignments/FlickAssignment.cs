using UnityEngine;

public class FlickAssignment : Assignment
{
	private Animator animator;
	private Vector2 flickDirection;
	protected override void Start()
	{
		base.Start();
		animator = GetComponent<Animator>();
		animator.SetFloat("dirX", flickDirection.x);
		animator.SetFloat("dirY", flickDirection.y);
	}
	public override void Initialize(params object[] additionalArgs)
	{
		flickDirection = (Vector2)additionalArgs[0];
		flickDirection = flickDirection.normalized;
	}
	public override void OnTriggerEnter2D(Collider2D other)
	{
		IStudent student = other.GetComponent<IStudent>();
		if(student == null) return;

		student.Launch(flickDirection);
		base.OnTriggerEnter2D(other);
	}
}
