using JsonFx.Json;
using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Data.Cache;
using TinyQuest.Data;
using TinyQuest.Core;
using TinyQuest.Model;
using TinyQuest.Factory.Model;
using TinyQuest.Object;

public class CombatPanelController : MonoBehaviour {
	public GameObject[] Slots;
	public ZoneStageController ZoneStageController;

	public void Slot0Clicked() {
		this.click(0);	
	}

	public void Slot1Clicked() {
		this.click(1);	
	}
	
	public void Slot2Clicked() {
		this.click(2);	
	}
	
	public void Slot3Clicked() {
		this.click(3);	
	}
	
	public void Slot4Clicked() {
		this.click(4);	
	}
	
	public void Slot5Clicked() {
		this.click(5);	
	}
	
	private void click(int slotIndex) {
		CombatController controller = this.ZoneStageController.GetComponent<CombatController>();
		if (controller != null) {
			controller.InvokeCommand(slotIndex);
		}
	}
}