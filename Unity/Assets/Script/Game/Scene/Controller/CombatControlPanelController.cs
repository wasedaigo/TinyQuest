using UnityEngine;

using TinyQuest.Scene.Model;
using TinyQuest.Data;
using TinyQuest.Core;
using TinyQuest.Object;

public class CombatControlPanelController : MonoBehaviour {
	public System.Action<int> CardSelected;
	
	public GameObject AttackMark;
	public GameObject StandByMark;
	public GameObject[] Cards;
	public bool IsEnemy;
	
	private Vector3[] cardOrigins;
	private int selectingCardIndex;
	private CombatModel combatModel;
	private SkillButtonView[] views;
	private bool[] cardFlags;
	
	void Start() {
	}

	public void SetModels(CombatModel combatModel) {
		this.combatModel = combatModel;
		this.views = new SkillButtonView[Cards.Length];
		this.cardOrigins = new Vector3[Cards.Length];
		this.cardFlags = new bool[Cards.Length];
		for (int i = 0; i < Cards.Length; i++) {
			this.views[i] = Cards[i].GetComponent<SkillButtonView>();
			this.cardOrigins[i] = Cards[i].transform.position;
		}
		
		this.selectingCardIndex = -1;
	}
	
	public int GetSelectingCardIndex() {
		return this.selectingCardIndex;	
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
		this.SelectCard(index);
		if (!this.cardFlags[index]) {
			GameObject card = this.Cards[index];
			Vector3 pos = card.transform.localPosition;
			StandByMark.transform.localPosition = new Vector3(pos.x, pos.y, -1);
		}
	}
	
	private float GetCardDelta() {
		if (IsEnemy) {
			return 0.4f;
		} else {
			return -0.4f;
		}	
	}

	public void ChooseAttackingCard(int index) {
		if (!this.cardFlags[index]) {
			this.cardFlags[index] = true;
				
			for (int i = 0; i < Cards.Length; i++) {
				int delta = i - index;
				
				GameObject card = Cards[i];
				Vector3 pos = card.transform.localPosition;
				//card.transform.localPosition = new Vector3(pos.x + delta, pos.y, pos.z);
				if (delta != 0) {
					iTween.ScaleTo(card.gameObject, iTween.Hash("x", 2, "y", 2, "time", 0.3f));
				}
			}
			
			//if (AttackMark != null) {
				GameObject card = this.Cards[index];
				iTween.ScaleTo(card.gameObject, iTween.Hash("x", 3, "y", 3, "time", 0.3f));
			//}
			//iTween.MoveBy(this.Cards[index].gameObject, iTween.Hash("y", GetCardDelta(), "easeType", "linear", "time", 0.2f));
		}
	}

	public void BackAttackingCard(int index) {
		if (this.cardFlags[index]) {
			this.cardFlags[index] = false;
			//iTween.MoveBy(this.Cards[index].gameObject, iTween.Hash("y",-GetCardDelta(), "easeType", "linear", "time", 0.2f));
		}
	}
	
	public void SelectCard(int index) {
		this.selectingCardIndex = index;
		if (index < 0) {
			this.selectingCardIndex = -1;
			StandByMark.transform.localPosition = new Vector3(-1000, 0, -1);
		} else {
			if (!this.cardFlags[index]) {
				this.CardSelected(this.selectingCardIndex);
			}
		}
	}
	
	protected void ChangeActorStatus(CombatActionResult result) {
		if (result.life <= 0) {
			this.selectingCardIndex = -1;
		}
		
		this.views[result.combatUnit.index].SetLife(result.life, result.maxLife);
	}
	
	public void SetTouchEnabled(bool enabled) {
		for (int i = 0; i < this.views.Length; i++) {
			//this.views[i].SetTouchEnabled(enabled);
		}
	}
	
	public void UpdateStatus(int groupNo) {
		if (!this.gameObject.active) {return;}
		for (int i = 0; i < Cards.Length; i++) {
			CombatUnit combatUnit = this.combatModel.GetCombatUnit(groupNo, i);
			this.views[i].UpdateStatus(combatUnit);
		}
	}
}