using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemTutorialPanel : MonoBehaviour, IPointerClickHandler
{
    private Image itemImage;
    private TextMeshProUGUI titleText;
    private TextMeshProUGUI descriptionText;
    private CanvasGroup canvasGroup;
    void Awake()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
        titleText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        descriptionText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
        gameObject.SetActive(false);
    }
    void OnEnable()
    {
        PauseManager.Instance?.Pause(1);
    }
    void OnDisable()
    {
        StopAllCoroutines();
        PauseManager.Instance?.Resume(1);
    }
    public void SetData(ItemTutorialData data)
    {
        itemImage.sprite = data.sprite;
        titleText.SetText(data.title);
        descriptionText.SetText(data.description);
    }
    public void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(Fade(1.0f, 0.4f));
        StartCoroutine(AutoHide());
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(Fade(0.0f, 0.4f));
    }
    public IEnumerator Fade(float alpha, float duration)
    {
        if(duration <= 0f)
        {
            canvasGroup.alpha = alpha;
            if(alpha == 0.0f) gameObject.SetActive(false);
            yield break;
        }

        float time = 0f;
        float startAlpha = canvasGroup.alpha;
        while(time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, alpha, time / duration);
            yield return null;
            time += Time.deltaTime;
        }
        canvasGroup.alpha = alpha;
        if(alpha == 0.0f) gameObject.SetActive(false);
    }
    public IEnumerator AutoHide()
    {
        yield return new WaitForSeconds(5f);
        yield return Fade(0.0f, 0.4f);
    }
}