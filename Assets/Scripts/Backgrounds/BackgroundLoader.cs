using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoader : MonoBehaviour
{
	[SerializeField] private GameObject bgPrefab;
	[SerializeField] private GameObject follower;
	[SerializeField] private BackgroundSource[] assets;
	void Awake()
	{
		for(int i=0; i<assets.Length; i++)
		{
			GameObject levelObject = new GameObject($"Stage{i+1} Background");
			levelObject.transform.SetParent(transform, false);
			ConstructLevelBackground(levelObject, assets[i]);
		}
	}
	private void ConstructLevelBackground(GameObject obj, BackgroundSource source)
	{
		for(int i=0; i<source.length; i++)
		{
			BackgroundLayerSource layerSource = source.GetLayer(i);
			GameObject layerObj = Instantiate(bgPrefab, obj.transform);

			SpriteRenderer renderer = layerObj.GetComponent<SpriteRenderer>();
        	ParallaxControl controller = layerObj.GetComponent<ParallaxControl>();
			renderer.sprite = layerSource.sprite;
			renderer.sortingOrder = -20 - i;
			controller.effectX = layerSource.effectX;
			controller.effectY = layerSource.effectY;
			controller.minY = source.minY;
			controller.maxY = source.maxY;
			controller.cam = follower;
		}
	}
}