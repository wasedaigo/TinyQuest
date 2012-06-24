using UnityEngine;
using System.Collections;

using TinyQuest.Scene.Model;
public class CombatScene : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		CombatModel combatModel = new CombatModel();
		
		ControlPanelController controlPanelController = this.gameObject.GetComponent<ControlPanelController>();
		controlPanelController.SetModels(combatModel);
		
		CombatController combatController = this.gameObject.GetComponent<CombatController>();
		combatController.SetModels(combatModel);
		combatController.StartBattle();
	}
}
