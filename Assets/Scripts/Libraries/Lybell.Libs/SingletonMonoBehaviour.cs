using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lybell.Libs
{
	public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T instance;

		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					// 싱글톤 게임 오브젝트가 없을 때 생성
					GameObject singletonObject = new GameObject();
					instance = singletonObject.AddComponent<T>();
					singletonObject.name = typeof(T).ToString() + " (Singleton)";

					DontDestroyOnLoad(singletonObject);
				}

				return instance;
			}
		}

		protected virtual void Awake()
		{
			if (instance == null)
			{
				instance = this as T;
				DontDestroyOnLoad(this.gameObject);
			}
			else Destroy(this.gameObject);
		}
	}
}