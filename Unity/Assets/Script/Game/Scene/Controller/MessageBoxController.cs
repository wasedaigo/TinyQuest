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
		
		float targetScale = 0.00415f;
		BaloonMessageBox box = (BaloonMessageBox)Instantiate(baloonMessageBox, new Vector3 (0, 0, 0), Quaternion.identity);
		box.transform.parent = UILayer.transform;
		box.transform.localScale = new Vector3(0.0001f, 0.0001f, 1);
		
		if (messageCutScene.pos == 0) {
			box.transform.localPosition = new Vector3(0.1f, -0.2f, 0);
			box.ArrowFaceRight = true;
		} else {
			box.transform.localPosition = new Vector3(-0.1f, -0.2f, 0);
			box.ArrowFaceRight = false;
		}
		
		box.Width = 200;
		box.Height = 64;
		box.Message = messageCutScene.text;
		this.visibleMessageBox = box;
		
		iTween.ScaleTo(box.gameObject, iTween.Hash("time", 0.25f, "x", targetScale, "y", targetScale,  "easeType", "easeOutCubic", "oncomplete", "onShowMessageComplete", "onCompleteTarget", this.gameObject));
	}
	
	public void HideMessage() {
		if (this.visibleMessageBox != null) {
			this.visibleMessageBox.transform.parent = null;
			Destroy(this.visibleMessageBox.gameObject);
			this.visibleMessageBox = null;
		}
	}
}