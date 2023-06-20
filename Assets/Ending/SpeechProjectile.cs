using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guidance.Ending
{
    public class SpeechProjectile : MonoBehaviour
    {
        private enum Phase
        {
            inactive,
            rotating,
            shoot
        }

        private Phase phase = Phase.inactive;
        private const float rotationMaxDuration = 0.5f;
        private float rotateDuration = rotationMaxDuration;
        private Vector2 direction;
        private float angle;

        private Rigidbody2D rigid;
        private AudioSource audioSource;
        // Start is called before the first frame update
        void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            audioSource = GetComponent<AudioSource>();
        }
        void Start()
        {
            gameObject.SetActive(false);
        }
        void FixedUpdate()
        {
            if(phase == Phase.inactive) return;
            if(phase == Phase.rotating)
            {
                rigid.MoveRotation(angle * (rotationMaxDuration - rotateDuration) / rotationMaxDuration);
                rotateDuration -= Time.fixedDeltaTime;
                if(rotateDuration <= 0f)
                {
                    phase = Phase.shoot;
                    rigid.MoveRotation(angle);
                    audioSource.Play();
                }
            }
            else
            {
                rigid.AddForce(direction, ForceMode2D.Impulse);
            }
        }
        public void Shoot(Transform target)
        {
            direction = (Vector2)( target.position - transform.position ).normalized;
            angle = Vector2.SignedAngle(Vector2.left, direction);
            phase = Phase.rotating;
        }
        public void OnCollisionEnter2D(Collision2D other)
        {
            if(other.gameObject == null) return;
            StudentHitter student = other.gameObject.GetComponent<StudentHitter>();
            if(student != null) student.HitProjectile();
            Destroy(gameObject);
        }
    }
}
