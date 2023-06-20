using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//a.k.a. rocket punch
public class ViolanceTrap : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigid;
    private PausableRigidSaver rigidSaver;
    private SpriteRenderer spriteRenderer;
    private int dirMag;
    private Vector2 angle;
    private bool isLaunch = false;
    private readonly WaitForSecondsPausable animDelay = new WaitForSecondsPausable(0.75f);
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        dirMag = (spriteRenderer.flipX) ? -1 : 1;
        rigidSaver = new PausableRigidSaver(rigid);
    }
    // Start is called before the first frame update
    void Start()
    {
        angle = transform.rotation * Vector2.right;
        StartCoroutine(PreLaunch());
    }

    void FixedUpdate()
    {
        rigidSaver.Run();
        if(PauseManager.paused) return;
        
        if(!isLaunch) return;
        Vector2 velocity = rigid.velocity;
        velocity += dirMag * angle * speed;
        if(velocity.magnitude > maxSpeed)
        {
            velocity = Vector2.ClampMagnitude(rigid.velocity, maxSpeed);
        }
        rigid.velocity = velocity;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if(other == null) return;
        HandleWallHit(other);
        HandleStudentHit(other);
    }

    void Flip()
    {
        isLaunch = false;
        spriteRenderer.flipX = !spriteRenderer.flipX;
        dirMag *= -1;
        rigid.velocity = Vector2.zero;
        rigid.position = Utils.GetCellObjectPosition(rigid.position) + dirMag * angle * 0.001f;
        StartCoroutine(PreLaunch());
    }

    IEnumerator PreLaunch()
    {
        animator.Play("rocketPunchPre");
        yield return animDelay;
        isLaunch = true;
    }

    void HandleWallHit(GameObject other)
    {
        if(other.layer != Constants.platform) return;
        Flip();
    }
    void HandleStudentHit(GameObject other)
    {
        IAssignmentRunner student = other.GetComponent<IAssignmentRunner>();
        if(student == null) return;
        student.HitObstacle();
    }
}
