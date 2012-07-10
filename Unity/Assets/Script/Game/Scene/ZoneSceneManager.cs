using UnityEngine;
using System.Collections;
using TinyQuest.Data;
using TinyQuest.Data.Request;

public class ZoneSceneManager : MonoBehaviour {
	public GameObject UICombatPanel;
	public GameObject UIProgressPanel;
	
			// Use this for initialization
	void Start () {
		//this.gameObject.AddComponent<ZoneEventController>();
		//this.UIProgressPanel.SetActiveRecursively(true);
		
		this.gameObject.AddComponent<CombatController>();
		this.UICombatPanel.SetActiveRecursively(true);
		
		LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
		req.LoadZone(this.OnLoaded);
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnLoaded(CombatUnitGroup[] combatUnitGroups) {
		CombatUnit[] activeUnits = new CombatUnit[Constant.GroupTypeCount];
		for (int i = 0; i < Constant.GroupTypeCount; i++) {
			CombatUnitGroup combatUnitGroup = combatUnitGroups[i];
			foreach (CombatUnit combatUnit in combatUnitGroup.combatUnits) {
			   this.SendMessage("SpawnActor", combatUnit);
			}
			activeUnits[i] = combatUnitGroup.combatUnits[combatUnitGroup.activeIndex];
		}
		
		this.SendMessage("ShowActors", activeUnits);
		this.SendMessage("UpdateStatus");
	}
}
