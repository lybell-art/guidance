using UnityEngine;

public class Distractor : AssignmentBase, ISaveableObject
{
	private AssignmentManager manager;
	private SpriteRenderer spriteRenderer;
	[SerializeField] private Sprite[] sprites;

	void Awake()
	{
		GameObject managerObject = GameObject.Find(Constants.ingameManagerName);
		manager = managerObject.GetComponent<AssignmentManager>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	void Start()
	{
		if(sprites.Length > 0)
		{
			spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
		}
		priority = 2;
	}
	public void SetManager(AssignmentManager manager)
	{
		this.manager = manager;
	}
	public void OnTriggerEnterByBody()
	{
		Complete();
	}
	public void OnDetectStudent()
	{
		AssignmentStateHandler handler = new AssignmentStateHandler(this);
		manager.AddItem(handler);
		Activate();
	}
	public MapObjectData ToObjectData()
	{
		DistractorData objectData = new DistractorData();
		objectData.position = transform.position;
		return objectData;
	}
}
