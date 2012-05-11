using UnityEngine;
using System.Collections;

public class MessageBoxController : BaseStageController {
	public System.Action MessageFinish;
	
	public void ShowText(string text) {
		this.ShowMessage(text);
	}
	
	public void OnNextButtonClick() {
		this.HideMessage();
		if (this.MessageFinish != null) {
			this.MessageFinish();
		}
	}
}
