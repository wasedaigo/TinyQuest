using UnityEngine;
using System.Collections;
using TinyQuest.Scene;

public class MessageBoxController : MonoBehaviour {
	public  BaloonMessageBox baloonMessageBox;
	private BaloonMessageBox visibleMessageBox;

	public void ShowMessageBox(string message) {
		this.HideMessageBox();
		BaloonMessageBox box = (BaloonMessageBox)Instantiate(baloonMessageBox, new Vector3 (0, 0, 0), Quaternion.identity);
		box.transform.parent = this.gameObject.transform;
		box.transform.localScale = new Vector3(0.003f, 0.003f, 1);
		box.transform.localPosition = new Vector3(0, 0.45f, 0);
		box.ArrowFaceRight = true;
		box.Width = 256;
		box.Height = 64;
		box.Message = message;
		
		this.visibleMessageBox = box;
	}
	
	public void HideMessageBox() {
		if (this.visibleMessageBox != null) {
			this.visibleMessageBox.transform.parent = null;
			Destroy(this.visibleMessageBox.gameObject);
			this.visibleMessageBox = null;
		}
	}
}