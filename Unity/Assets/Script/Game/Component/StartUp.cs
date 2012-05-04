using UnityEngine;
using System.Collections;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using TinyQuest.Data.Request;
using Async;

public class StartUp : MonoBehaviour {
	
	void Awake () {
		Application.targetFrameRate = 60;
	}
	
	void Start () {
		RequestFactory.Instance.EnableMock(true);
		MasterDataRequest masterRequest = RequestFactory.Instance.GetMasterDataRequest();
		
		Async.Async.Instance.Parallel(new System.Action<System.Action>[] {
			(next) => {
				masterRequest.GetStartUpData(
					(string result) => {
						MasterDataCache.Instance.Set(result);
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
