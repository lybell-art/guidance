using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxControl : MonoBehaviour
{
	private Vector2 startPos;
	private Vector2 originPos;
	private Vector2 parallaxPos;
	private float baseWidth;
	private float scale = 1.0f;
	public GameObject cam;
	public float minY = -999999999f;
	public float maxY = 999999999f;
	public float effectX = 0f;
	public float effectY = 0f;
	// Start is called before the first frame update
	void Start()
	{
		startPos = transform.position;
		originPos = GetCamPos();
		parallaxPos = originPos;
		baseWidth = GetComponent<SpriteRenderer>().bounds.size.x / 3;
	}

	// Update is called once per frame
	void LateUpdate()
	{
		UpdateScale();

		Vector2 camPos = GetCamPos();
		Vector2 deltaPos = camPos - originPos;
		parallaxPos.x = parallaxPos.x + deltaPos.x * (1f - effectX);
		parallaxPos.y = parallaxPos.y + deltaPos.y * (1f - effectY);

		float width = baseWidth * scale;
		float xPos = Mathf.Floor(camPos.x / width) * width + Utils.Remain(parallaxPos.x, width);
		float yPos = parallaxPos.y;
		if (yPos > camPos.y) yPos = camPos.y;
		transform.position = new Vector2(xPos, yPos);

		originPos = camPos;
	}

	void UpdateScale()
	{
		float oldScale = scale;
		Vector2 basePos = originPos;
		scale = Camera.main.orthographicSize / Constants.cameraBaseSize;
		transform.localScale = Vector3.one * scale;

		parallaxPos = scale / oldScale * (parallaxPos - basePos) + basePos;
	}

	Vector2 GetCamPos()
    {
        Vector2 camPos = cam.transform.position;
        camPos.y = Mathf.Clamp(camPos.y, minY, maxY - Camera.main.orthographicSize * 2f);
        return camPos;
    }
}
