using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using TinyQuest.Data.Request;
using Async;

public class StartUp : MonoBehaviour {
	
	void Start () {
		MasterDataRequest masterRequest = RequestFactory.Instance.GetMasterDataRequest();
		LocalUserDataRequest localUserDataRequest = RequestFactory.Instance.GetLocalUserRequest();
		
		Async.Async.Instance.Parallel(new List<System.Action<System.Action>> {
			(next) => {
				masterRequest.GetStartUpData(
					(string result) => {
						CacheFactory.Instance.GetMasterDataCache().Set(result);
						next();
					}
				);
			},
			(next) => {
				masterRequest.GetLocalizedText("en",
					(string result) => {
						CacheFactory.Instance.GetLocalizedTextCache().Set(result);
						next();
					}
				);
			},
			(next) => {
				localUserDataRequest.Get(
					(string result) => {
						CacheFactory.Instance.GetLocalUserDataCache().Set(result);
						next();
					}
				);
			}
		}, this.FinishLoading);

	}
	
	private void FinishLoading() {
		Application.LoadLevel("Home");	
	}
}
