using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using TinyQuest.Core;
using TinyQuest.Object;

public class CombatTimeline : MonoBehaviour {
	
	public CombatModel combatModel;
	private Dictionary<CombatUnit, FaceIcon> faceIconDictionary = new Dictionary<CombatUnit, FaceIcon>();
	public void Start() {
		this.combatModel.SetupBattle();
		
		CombatUnitGroup[] combatUnitGroups = this.combatModel.GetCombatUnits();
		for (int i = 0; i < Constant.GroupCount; i++) {
			for (int j = 0; j < combatUnitGroups[i].combatUnits.Count; j++) {
				CombatUnit combatUnit = combatUnitGroups[i].combatUnits[j];
				this.AddIcon(combatUnit);
			}
		}
	}

	public void Update() {
		this.combatModel.ReduceAllRemainTime(Time.deltaTime);
		foreach (KeyValuePair<CombatUnit, FaceIcon> pair in this.faceIconDictionary) {
			float newX = Mathf.FloorToInt(pair.Key.remainTime);
			Vector2 pos = pair.Value.LocalPosition;
			pair.Value.LocalPosition = new Vector2(newX, pos.y);
		}
	}

	private void AddIcon(CombatUnit combatUnit) {
		FaceIcon faceIcon = new FaceIcon(combatUnit.userUnit.unit);
		faceIcon.Transform.parent = this.transform;
		faceIcon.LocalPosition = new Vector2(0, 0);
		faceIcon.LocalRotation = 180.0f;
		faceIcon.LocalScale = new Vector2(-1, 1);
		Utils.SetLayerRecursively(faceIcon.Transform, 5);
		faceIconDictionary[combatUnit] = faceIcon;
	}
}
