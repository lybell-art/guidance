using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoKeyboardIndicator : MonoBehaviour
{
    private TextMeshProUGUI text;
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    public void Activate()
    {
        StartCoroutine(PlayIndicator());
    }
    private IEnumerator PlayIndicator()
    {
        WaitForSecondsPausable delay1 = new WaitForSecondsPausable(2f);
        WaitForSecondsPausable delay2 = new WaitForSecondsPausable(0.02f);
        text.alpha = 1f;
        text.SetText("잠깐, 학생을 직접 조작하려고 하시나요?");
        yield return delay1;
        text.SetText("당신은 학생에게 가야 할 길을 제시해야 할 뿐, 학생을 직접 조작해서 떠먹여줄 수 없습니다.");
        yield return delay1;
        text.SetText("대신 과제를 주세요. 학생은 과제를 향해 스스로 나아갈 것입니다.");
        yield return delay1;
        for(int i=0; i<20; i++) {
            text.alpha = (20-i)/20f;
            yield return delay2;
        }
        gameObject.SetActive(false);
        GameManager.Instance?.SaveFlag("noKeyboard", true);
    }
}