using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovingPlatform : MonoBehaviour, IMovablePlatform, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private bool isDragging = false;
    private float offset;
    private float toMove;
    private Rigidbody2D rigid;
    private Camera mainCamera;
    private Vector2 mousePosition;
    [SerializeField] private bool isVertical = false;
    public bool isMoving
    {
        get {return isDragging;}
    }
    public Vector2 position
    {
        get {return rigid.position;}
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        mainCamera = Camera.main;
        if(isVertical)
        {
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rigid.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        rigid.isKinematic = true;
    }

    void FixedUpdate()
    {
        if(isDragging)
        {
            Vector2 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            if(!isVertical) toMove = worldPosition.x - offset;
            else toMove = worldPosition.y - offset;

            Vector2 position = rigid.position;
            if(!isVertical)
            {
                float horizCap = 1.2f;
                if(position.x - toMove < -horizCap) position.x = position.x + 0.8f;
                else if(position.x - toMove > horizCap) position.x = position.x - 0.8f;
                else position.x = toMove;
            }       
            else
            {
                float vertCap = 0.9f;
                if(position.y - toMove < -vertCap) position.y = position.y + 0.5f;
                else if(position.y - toMove > vertCap) position.y = position.y - 0.5f;
                else position.y = toMove;
            }
            rigid.MovePosition(position);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        mousePosition = eventData.position;
        Vector2 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        if(!isVertical)
        {
            offset = worldPosition.x - rigid.position.x;
            toMove = rigid.position.x;
        }
        else
        {
            offset = worldPosition.y - rigid.position.y;
            toMove = rigid.position.y;
        }
        rigid.velocity = Vector2.zero;
        rigid.isKinematic = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            mousePosition = eventData.position;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(!isVertical) toMove = Mathf.Floor(rigid.position.x) + 0.5f;
        else toMove = Mathf.Floor(rigid.position.y) + 0.5f;
        StartCoroutine(DelayOffDragging());
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        IMovableAttachable target = other.GetComponent<IMovableAttachable>();
        if(target == null) return;
        target.Attach(this);
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        IMovableAttachable target = other.GetComponent<IMovableAttachable>();
        if(target == null) return;
        target.Detach(this);
    }

    private IEnumerator DelayOffDragging()
    {
        yield return new WaitForSecondsPausable(0.05f);
        isDragging = false;
        rigid.isKinematic = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 Collider가 static Collider인 경우
        if (collision.collider.gameObject.isStatic)
        {
            // 키네마틱 Rigidbody를 이전 위치로 되돌림
            rigid.position = rigid.position - rigid.velocity * Time.deltaTime;
        }
    }
}
