using UnityEngine;

using TinyQuest.Scene.Model;
using TinyQuest.Data;
using TinyQuest.Core;
using TinyQuest.Object;

public class CombatControlPanelController : MonoBehaviour {
	public GameObject[] StandByUnits1;
	public GameObject[] StandByUnits2;
	public System.Action<int> InvokeSkill;
	public GameObject[] Slots;
	public GameObject[][] StandByUnits;
	
	private Roga2dNode[][] standByUnitNodes;
	private CombatModel combatModel;
	private SkillButtonView[] views;
	
	void Start() {
		StandByUnits = new GameObject[][] {StandByUnits1, StandByUnits2};
		standByUnitNodes = new Roga2dNode[2][] {new Roga2dNode[3], new Roga2dNode[3]};
	}
	
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
		if (result.combatUnit.groupType != CombatGroupInfo.Instance.GetPlayerGroupType(0)) { return; }
		
		this.views[result.combatUnit.index].SetLife(result.life, result.maxLife);
	}
	
	public void UpdateStatus() {
		if (!this.gameObject.active) {return;}
		for (int i = 0; i < Slots.Length; i++) {
			CombatUnit combatUnit = this.combatModel.GetCombatUnit(CombatGroupInfo.Instance.GetPlayerGroupType(0), i);
			this.views[i].UpdateStatus(combatUnit);
		}
	}
	
	public void UpdateStandByUnits(int[][] standByUnits) {
		for (int i = 0; i < standByUnits.Length; i++) {
			for (int j = 0; j < standByUnits[i].Length; j++) {
	
				int dir = -1;
				if (i == 1 && standByUnits[i][j] > 0) {
					dir = 1;
				}
				
				if (this.standByUnitNodes[i][j] != null) {
					this.standByUnitNodes[i][j].Destroy();
					this.standByUnitNodes[i][j] = null;
				}
				
				FaceIcon icon = new FaceIcon(standByUnits[i][j]);
				icon.Transform.parent = StandByUnits[i][j].transform;
				icon.Transform.localPosition = new Vector3(0, 0, 0);
				icon.Transform.localEulerAngles = new Vector3(0, 0, 180);
				icon.Transform.localScale = new Vector3(24 * dir, 24, 0);
				Utils.SetLayerRecursively(icon.Transform, 5);
				this.standByUnitNodes[i][j] = icon;
						
			}
		}
	}
}