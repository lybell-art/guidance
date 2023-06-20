using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guidance.Ending
{
	public class BottomDialogueBox : DialogueBox
	{
		protected override void Awake()
		{
			base.Awake();
			SetColor(Color.white);
		}
	}
}