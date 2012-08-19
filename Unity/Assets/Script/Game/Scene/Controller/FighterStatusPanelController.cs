using UnityEngine;
using System.Collections;
using TinyQuest.Data;

public class FighterStatusPanelController : MonoBehaviour {
	public UILabel LifeLabel;
	public GameObject LifeIcon;
	
	private CombatUnit showingUnit;
	public void Start() {
		this.ShowPanels(false);
	}
	
	private void ShowPanels(bool show) {
		this.LifeIcon.SetActiveRecursively(show);
		this.LifeLabel.gameObject.SetActiveRecursively(show);
	}
	
	public void ShowFighterStatus(CombatUnit unit) {
		this.showingUnit = unit;
		this.ShowPanels(true);
		this.LifeLabel.text = unit.hp.ToString();
	}
	
	public void HideFighterStatus() {
		if (this.showingUnit == null) {return;}
		
		this.ShowPanels(false);
		this.showingUnit = null;
	}
	
}
