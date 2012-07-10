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
		foreach (CombatUnitGroup combatUnitGroup in combatUnitGroups) {
			foreach (CombatUnit combatUnit in combatUnitGroup.combatUnits) {
				this.SendMessage("SpawnActor", combatUnit);
			}
		}
		this.SendMessage("UpdateStatus");
	}
}
