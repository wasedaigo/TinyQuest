using UnityEngine;
using System.Collections;
using TinyQuest.Object;
using TinyQuest.Core;

public class NextUnitPanel : MonoBehaviour {
	public GameObject Target;
	private Roga2dNode targetNode;
	
	// Update is called once per frame
	
	public void UpdateStandbyUnit(int unitId) {
		if (this.targetNode != null) {
			this.targetNode.Destroy();
			this.targetNode = null;
		}
		FaceIcon icon = new FaceIcon(unitId);
		icon.Transform.parent = Target.transform;
		icon.Transform.localPosition = new Vector3(0, 0, 0);
		icon.Transform.localEulerAngles = new Vector3(0, 0, 180);
		icon.Transform.localScale = new Vector3(-24 , 24, 0);
		Utils.SetLayerRecursively(icon.Transform, 5);
		this.targetNode = icon;
	}
}
