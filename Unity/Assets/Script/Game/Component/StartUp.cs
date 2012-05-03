using UnityEngine;
using System.Collections;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
public class StartUp : MonoBehaviour {

	void Start () {
		// Load up all required data
		Async.Async.Instance.Parallel(
			new Async.IAsync[]{MasterDataCache<MasterWeapon>.Instance, LocalUserDataCache<UserWeapon>.Instance},
			this.FinishLoading	
		);
	}

	private void FinishLoading() {
		Application.LoadLevel("Home");	
	}
}
