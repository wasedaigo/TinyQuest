using UnityEngine;

using TinyQuest.Scene.Model;
using TinyQuest.Data;
using TinyQuest.Core;
using TinyQuest.Object;

public class CombatControlPanelController : MonoBehaviour {
	public System.Action<int> CardClicked;
	public GameObject[] Cards;
	public bool IsEnemy;
	
	private Vector3[] cardOrigins;
	private int executingCardIndex;
	private int selectingCardIndex;
	private CombatModel combatModel;
	private SkillButtonView[] views;
	
	void Start() {
	}

	public void SetModels(CombatModel combatModel) {
		this.combatModel = combatModel;
		this.views = new SkillButtonView[Cards.Length];
		this.cardOrigins = new Vector3[Cards.Length];
		for (int i = 0; i < Cards.Length; i++) {
			this.views[i] = Cards[i].GetComponent<SkillButtonView>();
			this.cardOrigins[i] = Cards[i].transform.position;
		}
		
		this.executingCardIndex = -1;
		this.selectingCardIndex = -1;
	}
	
	private void ResetCardPositions() {
		for (int i = 0; i < Constant.UnitCount; i++) {
			if (i == this.executingCardIndex) {
				continue;	
			}
			
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
	
	public int GetDir() {
		int dir = 1;
		if (IsEnemy) {
			dir *= -1;
		}	
		return dir;
	}
	
	private void click(int index) {
		int returnIndex = this.SelectCard(index);
		this.CardClicked(returnIndex);
	}
	
	public int SelectCard(int index) {
		
		ResetCardPositions();
		GameObject card = this.Cards[index];
		Vector3 pos = card.transform.localPosition;
		
		if (this.selectingCardIndex == index) {
			card.transform.localPosition = new Vector3(pos.x, 0, pos.z);
			this.selectingCardIndex = this.executingCardIndex;
		} else {
			card.transform.localPosition = new Vector3(pos.x, 16 * GetDir(), pos.z);
			this.selectingCardIndex = index;
		}
		
		return this.selectingCardIndex;
	}
	
	protected void ChangeActorStatus(CombatActionResult result) {
		this.views[result.combatUnit.index].SetLife(result.life, result.maxLife);
	}
	
	public void ExecuteCard(int index) {
		if (this.executingCardIndex == index) {
			return;	
		}
		
		GameObject card;
		Vector3 pos;
		
		if (this.executingCardIndex >= 0) {
			card = this.Cards[this.executingCardIndex];
			pos = card.transform.localPosition;
			iTween.MoveTo(card, iTween.Hash("time", 0.5f, "x", this.cardOrigins[this.executingCardIndex].x, "y", this.cardOrigins[this.executingCardIndex].y,  "easeType", "easeOutCubic", "oncomplete", "onCardBack", "onCompleteTarget", this.gameObject, "oncompleteparams", index));
			//iTween.RotateBy(card, iTween.Hash("time", 0.5f, "z", -1,  "easeType", "easeOutCubic"));
		} else {
			onCardBack(index);
		}
		
		this.executingCardIndex = index;
		
		//iTween.RotateBy(card, iTween.Hash("time", 0.5f, "z", 1,  "easeType", "easeOutCubic"));
	}
	
	protected void onCardBack(int index) {
		GameObject card = this.Cards[index];
		Vector3 pos = card.transform.localPosition;
		iTween.MoveTo(card, iTween.Hash("time", 0.5f, "x", 0.5f * GetDir(), "y", -0.3f,  "easeType", "easeOutCubic", "oncomplete", "onCardExecuted", "onCompleteTarget", this.gameObject, "oncompleteparams", card));
	}
	
	protected void onCardExecuted(GameObject card) {
	}
	
	public void UpdateStatus(int groupNo) {
		if (!this.gameObject.active) {return;}
		for (int i = 0; i < Cards.Length; i++) {
			CombatUnit combatUnit = this.combatModel.GetCombatUnit(groupNo, i);
			this.views[i].UpdateStatus(combatUnit);
		}
	}
}