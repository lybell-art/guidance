using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Guidance.Ending
{
	public class SpeechBubble : DialogueBox
	{
		public static readonly Color red = new Color(1f, 0.2f, 0.2f, 1f);
		public static readonly Color defaultColor = new Color(0.1f, 0.1f, 0.1f, 1f);

		private RectTransform rectTransform;
		protected override void Awake()
		{
			base.Awake();
			rectTransform = GetComponent<RectTransform>();
			SetColor(defaultColor);
		}
		public void MoveToGameObject(GameObject obj)
		{
			Camera cam = Camera.main;
			Vector3 position = obj.transform.position + new Vector3(0f, 1.5f, 0f);
			Vector2 screenPos = cam.WorldToScreenPoint(position);
			RectTransform canvasRect = transform.parent.GetComponent<RectTransform>();
			Vector2 finalPos;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, cam, out finalPos);
			rectTransform.anchoredPosition = finalPos;
		}
	}
}