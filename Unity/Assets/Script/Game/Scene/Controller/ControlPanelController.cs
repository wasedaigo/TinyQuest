using UnityEngine;

using TinyQuest.Scene.Model;
using TinyQuest.Data;
public class ControlPanelController : MonoBehaviour {
	public GameObject[] Slots;
	public const int GroupType = 0;
	private CombatModel combatModel;
	public void SetModels(CombatModel combatModel) {
		this.combatModel = combatModel;
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
		this.SendMessage("InvokeSkill", slotNo);
	}
	
	protected void UpdateStatus() {
		for (int i = 0; i < Slots.Length; i++) {
			SkillButtonView view = Slots[i].GetComponent<SkillButtonView>();
			CombatUnit combatUnit = this.combatModel.GetCombatUnit(GroupType, i);
			view.UpdateStatus(combatUnit);
		}
	}
}