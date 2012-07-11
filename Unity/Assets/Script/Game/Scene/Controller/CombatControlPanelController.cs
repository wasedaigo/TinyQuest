using UnityEngine;

using TinyQuest.Scene.Model;
using TinyQuest.Data;
public class CombatControlPanelController : MonoBehaviour {
	public System.Action<int> InvokeSkill;
	public GameObject[] Slots;
	public const int GroupType = 0;
	private CombatModel combatModel;
	private SkillButtonView[] views;
	public void SetModels(CombatModel combatModel) {
		this.combatModel = combatModel;
		this.views = new SkillButtonView[Slots.Length];
		for (int i = 0; i < Slots.Length; i++) {
			this.views[i] = Slots[i].GetComponent<SkillButtonView>();
		}
	}
	
	public void Slot1Clicked() {
		this.click(0);
	}

	public void Slot2Clicked() {
		this.click(1);
	}
		
	public void Slot3Clicked() {
		this.click(2);
	}
		
	public void Slot4Clicked() {
		this.click(3);
	}
		
	public void Slot5Clicked() {
		this.click(4);
	}
		
	public void Slot6Clicked() {
		this.click(5);
	}
		
	private void click(int slotNo) {
		this.InvokeSkill(slotNo);
	}
	
	protected void ChangeActorStatus(CombatActionResult result) {
		if (result.combatUnit.groupType != GroupType) { return; }
		
		this.views[result.combatUnit.index].SetLife(result.life, result.maxLife);
	}
	
	protected void UpdateStatus() {
		if (!this.gameObject.active) {return;}
		for (int i = 0; i < Slots.Length; i++) {
			CombatUnit combatUnit = this.combatModel.GetCombatUnit(GroupType, i);
			this.views[i].UpdateStatus(combatUnit);
		}
	}
}