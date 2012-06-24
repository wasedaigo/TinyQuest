
using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Object;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using TinyQuest.Data.Request;

public class ZoneLoading : MonoBehaviour {

	void Start () {
		this.FinishLoading();
	}

	private void FinishLoading() {
		Application.LoadLevel("Zone");	
	}
}
