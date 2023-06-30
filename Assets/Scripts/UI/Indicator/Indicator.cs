using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    private enum Direction
    {
        up,
        down,
        left,
        right,
        center
    }
    private const float offset = 10f;

    private Image image;
    private RectTransform rectTransform;
    [SerializeField] private Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    
    public void DrawIndicator(Vector3 position)
    {
        float angle = GetTargetAngle(position);
        Direction section = GetSection(angle);
        float linearPos = GetAnchoredLinearPosition(angle, section);
        Vector2 anchoredPosition = GetAnchoredPosition(section, linearPos);
        Vector2 anchor = GetAnchor(section);

        SetAnchor(anchor);
        SetAnchoredPosition(anchoredPosition);
        SetImage(section);
    }
    
    private float GetTargetAngle(Vector3 targetPosition)
    {
        Vector2 screenPosition = (Vector2)Camera.main.WorldToScreenPoint(targetPosition);
        screenPosition -= new Vector2(Screen.width/2, Screen.height/2);
        float angle = Vector2.Angle(Vector2.up, screenPosition);
        if(screenPosition.x < 0f) return -angle;
        return angle; 
    }
    private Direction GetSection(float angle)
    {
        float screenAngle = Vector2.Angle(Vector2.up, new Vector2(Screen.width, Screen.height));
        if(-screenAngle <= angle && angle <= screenAngle) return Direction.up;
        if(-(180 - screenAngle) <= angle && angle < -screenAngle) return Direction.left;
        if(screenAngle < angle && angle <= 180-screenAngle) return Direction.right;
        return Direction.down;
    }
    private float GetAnchoredLinearPosition(float angle, Direction dir)
    {
        RectTransform parentRectTransform = rectTransform.parent as RectTransform;
        float width = parentRectTransform?.rect.width ?? Screen.width;
        float height = parentRectTransform?.rect.height ?? Screen.height;

        float radian = angle * Mathf.Deg2Rad;
        switch(dir)
        {
            case Direction.up:
                return (height/2 - offset) * Mathf.Tan(radian);
            case Direction.down:
                return -(height/2 - offset) * Mathf.Tan(radian);
            case Direction.left:
                return -(width/2 - offset) / Mathf.Tan(radian);
            case Direction.right:
                return (width/2 - offset) / Mathf.Tan(radian);
            default:
                return 0f;
        }
    }
    private Vector2 GetAnchor(Direction section)
    {
        switch(section)
        {
            case Direction.up:
                return new Vector2(0.5f,1f);
            case Direction.down:
                return new Vector2(0.5f,0f);
            case Direction.left:
                return new Vector2(0f,0.5f);
            case Direction.right:
                return new Vector2(1f,0.5f);
            default:
                return new Vector2();
        }
    }
    private void SetAnchor(Vector2 anchor)
    {
        rectTransform.anchorMin = anchor;
        rectTransform.anchorMax = anchor;
    }
    private Vector2 GetAnchoredPosition(Direction section, float linearPos)
    {
        switch(section)
        {
            case Direction.up:
                return new Vector2(linearPos, -offset);
            case Direction.down:
                return new Vector2(linearPos, offset);
            case Direction.left:
                return new Vector2(offset, linearPos);
            case Direction.right:
                return new Vector2(-offset, linearPos);
            default:
                return new Vector2();
        }
    }

    private void SetAnchoredPosition(Vector2 anchoredPosition)
    {
        rectTransform.anchoredPosition = anchoredPosition;
    }

    private void SetImage(Direction section)
    {
        if(sprites.Length == 0 || sprites.Length <= (int)section) return;
        image.sprite = sprites[(int)section];
    }
}
//RectTransformUtility.ScreenPointToLocalPointInRectangle(arrowRectTransform.parent as RectTransform, screenPosition, null, out anchoredPosition);
