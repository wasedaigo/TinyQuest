
using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Object;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using TinyQuest.Data.Request;

public class ZoneLoading : MonoBehaviour {

	void Start () {
		RequestFactory.Instance.EnableMock(true);
		LocalUserDataRequest<UserZone> userZoneRequest = RequestFactory.Instance.GetUserDataRequest<LocalUserDataRequest<UserZone>, UserZone>();
	
		// Load up all required data
		Async.Async.Instance.Parallel(new System.Action<System.Action>[] {
			(next) => {
				userZoneRequest.Get(
					(string result) => {
						MasterDataCache.Instance.SetZone(result);
						next();
					}
				);
			}
		}, this.FinishLoading);
	}

	private void FinishLoading() {
		Application.LoadLevel("Zone");	
	}
}
