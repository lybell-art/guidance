using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lybell.Libs;

namespace Guidance.Ending
{
	public class DialogueBox : MonoBehaviour
	{
		protected TextMeshProUGUI textUI;
		protected AudioSource audioSource;
		protected virtual void Awake()
		{
			audioSource = GetComponent<AudioSource>();
			textUI = transform.GetComponentInChildren<TextMeshProUGUI>();
		}

		public void SetColor(Color color)
		{
			textUI.color = color;
		}
		public IEnumerator Play(string str, AudioClip clip, float duration = 0.075f, bool autoInactive = true)
		{
			yield return new WaitInteruptable(PlayAuto(str, clip, duration), PlayImmediate(str), GetKey);
			if(duration <= 0f || autoInactive) gameObject.SetActive(false);
		}

		private IEnumerator PlayAuto(string str, AudioClip clip, float duration = 0.075f)
		{
			if(duration <= 0f)
			{
				textUI.SetText(str);
				yield return new WaitForSeconds(2f);
				yield break;
			}

			int length = str.Length;
			StringBuilder speech = new StringBuilder(length);
			WaitForSeconds delay = new WaitForSeconds(duration);
			int speedMultiplier = 1;
			if(audioSource != null) audioSource.clip = clip;
			for(int i=0; i<length; i++)
			{
				if(str[i] == '$')
				{
					if(i+1 >= length) break;
					if(System.Char.IsDigit(str[i+1]))
					{
						speedMultiplier = (str[i+1] - '0');
						i++;
					}
					else speedMultiplier = 1;
				}
				else
				{
					speech.Append(str[i]);
					textUI.SetText(speech);
					if(audioSource != null && IsSpecialCharacter(str[i]) == false) audioSource.Play();
					for (int d=0; d<speedMultiplier; d++) yield return delay;
				}
			}
			yield return new WaitForSeconds(1f);
		}
		private IEnumerator PlayImmediate(string str)
		{
			textUI.SetText(str);
			yield return new WaitForSeconds(0.5f);
		}

		private bool GetKey()
		{
			return Input.GetMouseButton(0);
		}
		private bool IsSpecialCharacter(char c)
	    {
	        const string specialCharacters = " \n\t`~!@#$%^&*()_+-=[]{};:'\",./<>?";
	        return specialCharacters.IndexOf(c) >= 0;
	    }
	}
}