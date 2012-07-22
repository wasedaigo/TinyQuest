using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using TinyQuest.Data.Request;
using Async;

public class StartUp : MonoBehaviour {
	
	void Start () {
	}
	
	private void FinishLoading() {
		Application.LoadLevel("Home");	
	}
}
