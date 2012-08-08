using UnityEngine;
using System.Collections;
using TinyQuest.Object;
using TinyQuest.Core;
using TinyQuest.Data;

public class NextUnitPanel : MonoBehaviour {
	public GameObject Target;
	private Roga2dNode targetNode;
	private CombatUnit combatUnit;
	
	// Update is called once per frame
	
	public void UpdateStandbyUnit(CombatUnit combatUnit) {
		if (this.combatUnit == combatUnit) {
			return;
		}

		this.combatUnit = combatUnit;
		if (this.targetNode != null) {
			this.targetNode.Destroy();
			this.targetNode = null;
		}
		
		if (combatUnit == null) {
			return;	
		}
		int unitId = combatUnit.userUnit.unit;
		
		FaceIcon icon = new FaceIcon(unitId);
		icon.Transform.parent = Target.transform;
		icon.Transform.localPosition = new Vector3(0, 0, 0);
		icon.Transform.localEulerAngles = new Vector3(0, 0, 180);
		icon.Transform.localScale = new Vector3(-24 , 24, 0);
		Utils.SetLayerRecursively(icon.Transform, 5);
		this.targetNode = icon;
	}
}
