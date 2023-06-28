using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignmentGenerator : MonoBehaviour
{
	private ClickManager clickManager;
	private AssignmentManager assignmentManager;
	private PriorityCursor holdCursor;

	[SerializeField] private bool hasPriorityAbility = false;
	[SerializeField] private bool hasFlickAbility = false;
	private AbilityHaveUI priorityUI;
	private AbilityHaveUI flickUI;
	private ItemTutorialPanelController itemDescPanel;
	private System.Action<Vector3> OnHoldCanceledWrapper;

	[SerializeField] private GameObject holdCursorObj;
	[SerializeField] private GameObject priorityAbilityUIObj;
	[SerializeField] private GameObject flickAbilityUIObj;

	void Awake()
	{
		assignmentManager = GetComponent<AssignmentManager>(); 
		clickManager = GetComponent<ClickManager>();
		holdCursor = holdCursorObj.GetComponent<PriorityCursor>();
		priorityUI = priorityAbilityUIObj.GetComponent<AbilityHaveUI>();
		flickUI = flickAbilityUIObj.GetComponent<AbilityHaveUI>();
		itemDescPanel = GetComponent<ItemTutorialPanelController>();
		OnHoldCanceledWrapper = (_)=>{this.OnHoldCanceled();};
	}
	void OnEnable()
	{
		clickManager.OnClick += this.OnClick;
		clickManager.OnHoldStart += this.OnHoldStart;
		clickManager.OnHolding += this.OnHolding;
		clickManager.OnHoldEnd += this.OnHoldEnd;
		clickManager.OnDrag += this.OnHoldCanceledWrapper;
		clickManager.OnFlick += this.OnFlick;
		clickManager.OnPointerCanceled += this.OnHoldCanceled;
	}
	void OnDisable()
	{
		clickManager.OnClick -= this.OnClick;
		clickManager.OnHoldStart -= this.OnHoldStart;
		clickManager.OnHolding -= this.OnHolding;
		clickManager.OnHoldEnd -= this.OnHoldEnd;
		clickManager.OnDrag -= this.OnHoldCanceledWrapper;
		clickManager.OnFlick -= this.OnFlick;
		clickManager.OnPointerCanceled -= this.OnHoldCanceled;
	}

	public void EarnPriorityAbility()
	{
		hasPriorityAbility = true;
		priorityUI.ActivateIcon();
		itemDescPanel.Show(Constants.priorityAssignmentName);
	}
	public void EarnFlickAbility()
	{
		hasFlickAbility = true;
		flickUI.ActivateIcon();
		itemDescPanel.Show(Constants.flickAssignmentName);
	}

	// 클릭으로 판정했을 시 발생
	void OnClick(Vector3 mousePos)
	{
		assignmentManager.TryPlaceAssignment(mousePos, AssignmentType.normal);
	}
	// 홀드 시작할 때(홀드 커서를 발동시킨다)
	void OnHoldStart(Vector3 mousePos)
	{
		if(!hasPriorityAbility) return;
		if(!assignmentManager.CanPlaceAssignment(mousePos)) return;
		holdCursorObj.SetActive(true);
		holdCursor.SetPosition(mousePos);
		holdCursor.SetPriority(1);
	}
	// 홀드 중일 때(홀드 커서를 발동시킨다)
	void OnHolding(Vector3 mousePos, float time)
	{
		if(!hasPriorityAbility) return;
		holdCursor.SetPriority(ConvertTimeToPriority(time));
	}
	// 홀드로 판정했을 시 발생
	void OnHoldEnd(Vector3 mousePos, float time)
	{
		if(!hasPriorityAbility) return;
		holdCursorObj.SetActive(false);
		assignmentManager.TryPlaceAssignment(mousePos, AssignmentType.priority, ConvertTimeToPriority(time));
	}
	void OnHoldCanceled()
	{
		holdCursorObj.SetActive(false);
	}
	// 플릭으로 판정했을 시 발생
	void OnFlick(Vector3 mousePos, Vector2 flickDirection)
	{
		if(!hasFlickAbility) return;
		holdCursorObj.SetActive(false);
		assignmentManager.TryPlaceAssignment(mousePos, AssignmentType.flick, flickDirection);
	}

	private int ConvertTimeToPriority(float time)
	{
		int result = (int)(time / 0.4f);
		if(result < 1) return 1;
		if(result > 3) return 3;
		return result;
	}
}
