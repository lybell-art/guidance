using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guidance.Ending
{
    public class StudentFaller : MonoBehaviour
    {
        private bool isActivated = false;
        private Rigidbody2D rigid;
        private Animator animator;
        private AudioSource audioSource;
        [SerializeField] private AudioClip fallClip;
        [SerializeField] private AudioClip groundClip;
        public bool onGrounded { get; private set;}

        public float gravity = 2f;
        void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }
        public void StartFall()
        {
            isActivated = true;
            rigid.drag = 1f;
            rigid.gravityScale = gravity;
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigid.velocity = Vector2.zero;
            onGrounded = false;
            animator.Play("startFall");
            audioSource.clip = fallClip;
            audioSource.Play();
        }
        public void TransMonochrome()
        {
            rigid.position = new Vector2(0f, rigid.position.y);
            animator.Play("transMonochrome");
        }
        public void OnCollisionEnter2D(Collision2D other)
        {
            if(!isActivated) return;
            if(other.gameObject == null) return;
            if(other.gameObject.layer == Constants.platform) EndFall();
        }
        public void EndFall()
        {
            isActivated = false;
            onGrounded = true;
            animator.Play("endFall");
            audioSource.Stop();
            audioSource.PlayOneShot(groundClip);
            rigid.position = new Vector2(rigid.position.x, Mathf.Round(rigid.position.y) + 0.5f);
            rigid.gravityScale = 0f;
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        }
    }
}

