using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemTutorialPanel : FadableContainer, IPointerClickHandler
{
    private Image itemImage;
    private TextMeshProUGUI titleText;
    private TextMeshProUGUI descriptionText;
    protected override void Awake()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
        titleText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        descriptionText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        base.Awake();
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
    public override void Show()
    {
        base.Show();
        StartCoroutine(AutoHide());
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Hide();
    }
    public IEnumerator AutoHide()
    {
        yield return new WaitForSeconds(5f);
        yield return Fade(0.0f, baseDuration);
        gameObject.SetActive(false);
    }
}