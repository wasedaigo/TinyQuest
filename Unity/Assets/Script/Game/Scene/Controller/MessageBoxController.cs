using UnityEngine;
using System.Collections;
using TinyQuest.Scene;
using TinyQuest.Data;

public class MessageBoxController : MonoBehaviour {
	public  GameObject UILayer;
	public  BaloonMessageBox baloonMessageBox;
	private BaloonMessageBox visibleMessageBox;
	
	public void ShowMessage(ZoneMessageCutScene messageCutScene) {
		this.HideMessage();
		BaloonMessageBox box = (BaloonMessageBox)Instantiate(baloonMessageBox, new Vector3 (0, 0, 0), Quaternion.identity);
		box.transform.parent = UILayer.transform;
		box.transform.localScale = new Vector3(0.00415f, 0.00415f, 1);
		
		if (messageCutScene.pos == 0) {
			box.transform.localPosition = new Vector3(-0.1f, 0.55f, 0);
			box.ArrowFaceRight = false;
		} else {
			box.transform.localPosition = new Vector3(0.1f, 0.55f, 0);
			box.ArrowFaceRight = true;
		}
		
		box.Width = 256;
		box.Height = 96;
		box.Message = messageCutScene.text;
		
		this.visibleMessageBox = box;
	}
	
	public void HideMessage() {
		if (this.visibleMessageBox != null) {
			this.visibleMessageBox.transform.parent = null;
			Destroy(this.visibleMessageBox.gameObject);
			this.visibleMessageBox = null;
		}
	}
}