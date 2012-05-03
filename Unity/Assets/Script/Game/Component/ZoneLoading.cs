
using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Object;
using TinyQuest.Data;
using TinyQuest.Data.Cache;

public class ZoneLoading : MonoBehaviour {

	void Start () {
		// Load up all required data
		Async.Async.Instance.Parallel(
			new Async.IAsync[]{LocalUserDataCache<UserZone>.Instance},
			this.FinishLoading	
		);
	}

	private void FinishLoading() {
		Application.LoadLevel("Zone");	
	}
}
