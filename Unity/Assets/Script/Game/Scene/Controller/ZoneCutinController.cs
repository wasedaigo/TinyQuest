using UnityEngine;
using System.Collections;

public class ZoneCutinController : MonoBehaviour {
	public GameObject ZoneCutinPrefab;
	public GameObject UILayer;
	
	public struct CutinParam {
		public string message;
		public System.Action callback;
		
		public CutinParam(string message, System.Action callback) {
			this.message = message;
			this.callback = callback;
		}
	}
	
	private GameObject cutIn;
	protected void ShowZoneCutin(CutinParam param) {
		if (this.cutIn != null) {
			Destroy(this.cutIn);
			this.cutIn = null;
		}
		this.cutIn = Instantiate(ZoneCutinPrefab) as GameObject;
		this.cutIn.transform.parent = UILayer.transform;
		
		cutIn.transform.localPosition = new Vector3(2, cutIn.transform.localPosition.y, cutIn.transform.localPosition.z);
		iTween.MoveTo(cutIn.gameObject, iTween.Hash("time", 0.5f, "x", 0,  "easeType", "easeOutCubic"));
		iTween.MoveTo(cutIn.gameObject, iTween.Hash("time", 0.5f, "delay", 1, "x", -2,  "easeType", "easeInCubic", "oncomplete", "OnCutinFinished", "oncompletetarget", this.gameObject, "oncompleteparams", param));
		UILabel label = cutIn.transform.FindChild("Label").GetComponent<UILabel>();
		label.text = param.message;
	}
	
	private void OnCutinFinished(CutinParam param) {
		if (this.cutIn != null) {
			//Destroy(this.cutIn);
			this.cutIn = null;
		}
		param.callback();
	}
}