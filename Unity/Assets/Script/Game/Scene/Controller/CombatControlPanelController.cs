using UnityEngine;

using TinyQuest.Scene.Model;
using TinyQuest.Data;
using TinyQuest.Core;
using TinyQuest.Object;

public class CombatControlPanelController : MonoBehaviour {
	public System.Action<int> CardSelected;
	public System.Action<int> CardExecuted;
	
	public GameObject[] Cards;
	public bool IsEnemy;
	
	private Vector3[] cardOrigins;
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
		
		this.selectingCardIndex = -1;
	}
	
	private void ResetCardPositions() {
		for (int i = 0; i < Constant.UnitCount; i++) {
			if (i == this.selectingCardIndex) {
				continue;	
			}
			GameObject card = this.Cards[i];
			Vector3 pos = card.transform.localPosition;	
			card.transform.localPosition = new Vector3(pos.x, 0, pos.z);
			UIImageButton btn = card.GetComponent<UIImageButton>();
			UIButtonMessage btnMessage = card.GetComponent<UIButtonMessage>();
			btn.pressedSprite = "papet_btn";
			//btnMessage.trigger = UIButtonMessage.Trigger.OnPress;
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
		GameObject card = this.Cards[index];
		
		Vector3 pos = card.transform.localPosition;
		
		UIImageButton btn = card.GetComponent<UIImageButton>();
		UIButtonMessage btnMessage = card.GetComponent<UIButtonMessage>();
		
		if (this.selectingCardIndex == index) {
			
			this.CardExecuted(this.selectingCardIndex);
		} else {
			//btnMessage.trigger = UIButtonMessage.Trigger.OnClick;
			btn.pressedSprite = "papet_btn_on";
			card.transform.localPosition = new Vector3(pos.x, 16 * GetDir(), pos.z);
			this.selectingCardIndex = index;
			ResetCardPositions();
			this.CardSelected(this.selectingCardIndex);
		}
	}
	
	protected void ChangeActorStatus(CombatActionResult result) {
		if (result.life <= 0) {
			this.selectingCardIndex = -1;
		}
		ResetCardPositions();
		
		this.views[result.combatUnit.index].SetLife(result.life, result.maxLife);
	}
	
	public void SetTouchEnabled(bool enabled) {
		for (int i = 0; i < this.views.Length; i++) {
			this.views[i].SetTouchEnabled(enabled);
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