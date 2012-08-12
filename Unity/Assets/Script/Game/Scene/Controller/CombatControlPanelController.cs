using UnityEngine;

using TinyQuest.Scene.Model;
using TinyQuest.Data;
using TinyQuest.Core;
using TinyQuest.Object;

public class CombatControlPanelController : MonoBehaviour {
	public System.Action<int> CardClicked;
	public GameObject[] Cards;
	public float SelectMoveDistance;
	
	private CombatModel combatModel;
	private SkillButtonView[] views;
	
	void Start() {
	}
	
	public void SetModels(CombatModel combatModel) {
		this.combatModel = combatModel;
		this.views = new SkillButtonView[Cards.Length];
		for (int i = 0; i < Cards.Length; i++) {
			this.views[i] = Cards[i].GetComponent<SkillButtonView>();
		}
	}
	
	private void ResetCardPositions() {
		for (int i = 0; i < Constant.UnitCount; i++) {
			GameObject card = this.Cards[i];
			Vector3 pos = card.transform.localPosition;	
			card.transform.localPosition = new Vector3(pos.x, 0, pos.z);
		}
	}
	
	public void Card1Clicked() {
		this.click(0);
	}

	public void Card2Clicked() {
		this.click(1);
	}
		
	public void Card3Clicked() {
		this.click(2);
	}
		
	public void Card4Clicked() {
		this.click(3);
	}
		
	public void Card5Clicked() {
		this.click(4);
	}
		
	public void Card6Clicked() {
		this.click(5);
	}
		
	private void click(int index) {
		ResetCardPositions();
		GameObject card = this.Cards[index];
		Vector3 pos = card.transform.localPosition;
		card.transform.localPosition = new Vector3(pos.x, SelectMoveDistance, pos.z);
		
		this.CardClicked(index);
	}
	
	protected void ChangeActorStatus(CombatActionResult result) {
		this.views[result.combatUnit.index].SetLife(result.life, result.maxLife);
	}
	
	public void UpdateStatus(int groupNo) {
		if (!this.gameObject.active) {return;}
		for (int i = 0; i < Cards.Length; i++) {
			CombatUnit combatUnit = this.combatModel.GetCombatUnit(groupNo, i);
			this.views[i].UpdateStatus(combatUnit);
		}
	}
}