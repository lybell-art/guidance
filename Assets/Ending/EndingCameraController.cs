using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guidance.Ending
{
    public class EndingCameraController : MonoBehaviour
    {
        private enum CameraMode
        {
            idle,
            follow,
            transition
        }
        private CameraMode mode;
        private Camera cam;
        private Transform toFollow = null;
        private Vector3 fromTransition = new Vector3();
        private Vector3 toTransition = new Vector3();
        private float transitionElapsedtime = 0f;
        private float toTransitionDuration = 0f;
        public bool isIdle
        {
            get { return mode == CameraMode.idle; }
        }
        void Awake()
        {
            cam = GetComponent<Camera>();
        }
        void LateUpdate()
        {
            if(mode == CameraMode.idle) return;
            if(mode == CameraMode.transition) DoTransition();
            else if(mode == CameraMode.follow) DoFollow();
        }
        // cut
        public void Cut(Vector2 vector)
        {
            cam.transform.position = new Vector3(vector.x, vector.y, -10f);
        }
        public void Cut(Vector3 vector)
        {
            cam.transform.position = new Vector3(vector.x, vector.y, -10f);
        }
        public void Cut(float x, float y)
        {
            cam.transform.position = new Vector3(x, y, -10f);
        }
        public void Cut(float x, float y, float orthoSize)
        {
            cam.transform.position = new Vector3(x, y, -10f);
            cam.orthographicSize = orthoSize;
        }
        // set size
        public void SetSize(float orthoSize)
        {
            cam.orthographicSize = orthoSize;
        }
        // transition
        public void MakeTransition(float x, float y, float duration)
        {
            MakeTransition(x, y, cam.orthographicSize, duration);
        }
        public void MakeTransition(float x, float y, float orthoSize, float duration)
        {
            mode = CameraMode.transition;
            Vector3 camPos = cam.transform.position;
            fromTransition = new Vector3(camPos.x, camPos.y, cam.orthographicSize);
            toTransition = new Vector3(x, y, orthoSize);
            toTransitionDuration = duration;
            transitionElapsedtime = 0f;
        }
        public IEnumerator Pan(float x, float y, float duration)
        {
            MakeTransition(x, y, cam.orthographicSize, duration);
            yield return new WaitUntil(()=>mode == CameraMode.idle);
        }
        public IEnumerator Pan(float x, float y, float orthoSize, float duration)
        {
            MakeTransition(x, y, orthoSize, duration);
            yield return new WaitUntil(()=>mode == CameraMode.idle);
        }
        // follow
        public void Follow(Transform target)
        {
            mode = CameraMode.follow;
            toFollow = target;
        }
        public void Follow(GameObject target)
        {
            mode = CameraMode.follow;
            toFollow = target.transform;
        }
        public void EndFollow()
        {
            mode = CameraMode.idle;
            toFollow = null;
        }

        // private functions
        private void DoTransition()
        {
            float interpolationRatio = transitionElapsedtime / toTransitionDuration;
            Vector3 lerper = Vector3.Lerp(fromTransition, toTransition, interpolationRatio);
            Cut(lerper);
            cam.orthographicSize = lerper.z;
            transitionElapsedtime += Time.deltaTime;
            if(transitionElapsedtime >= toTransitionDuration) mode = CameraMode.idle;
        }
        private void DoFollow()
        {
            if(toFollow == null) return;
            Cut(toFollow.position);
        }
    }
}