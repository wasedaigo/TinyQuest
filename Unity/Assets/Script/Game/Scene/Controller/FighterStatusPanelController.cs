using UnityEngine;
using System.Collections;
using TinyQuest.Data;

public class FighterStatusPanelController : MonoBehaviour {
	public UILabel LifeLabel;
	public UILabel PowerLabel;
	public GameObject LifeIcon;
	public GameObject PowerIcon;
	
	private CombatUnit showingUnit;
	public void Start() {
		this.ShowPanels(false);
	}
	
	private void ShowPanels(bool show) {
		this.LifeIcon.SetActiveRecursively(show);
		this.PowerIcon.SetActiveRecursively(show);
		this.LifeLabel.gameObject.SetActiveRecursively(show);
		this.PowerLabel.gameObject.SetActiveRecursively(show);
	}
	
	protected void ShowFighterStatus(CombatUnit unit) {
		this.showingUnit = unit;
		this.ShowPanels(true);
		this.LifeLabel.text = unit.hp.ToString();
		this.PowerLabel.text = unit.GetUserUnit().Power.ToString();
	}
	
	protected void HideFighterStatus() {
		if (this.showingUnit == null) {return;}
		
		this.ShowPanels(false);
		this.showingUnit = null;
	}
}
