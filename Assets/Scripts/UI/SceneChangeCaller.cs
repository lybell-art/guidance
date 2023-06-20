using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeCaller : MonoBehaviour
{
	public void MoveToIngame()
	{
		GameManager.Instance.GameStart();
	}
	public void MoveToTitle()
	{
		GameManager.Instance.ToTitle();
	}
}