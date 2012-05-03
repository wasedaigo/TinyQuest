using UnityEngine;
using System.Collections;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using TinyQuest.Data.Request;
using Async;

public class StartUp : MonoBehaviour {

	void Start () {
		RequestFactory.Instance.EnableMock(true);
		MasterDataRequest<MasterWeapon> masterWeaponRequest = RequestFactory.Instance.GetMasterDataRequest<MasterDataRequest<MasterWeapon>, MasterWeapon>();
		LocalUserDataRequest<UserWeapon> userWeaponRequest = RequestFactory.Instance.GetUserDataRequest<LocalUserDataRequest<UserWeapon>, UserWeapon>();
		
		Async.Async.Instance.Parallel(new System.Action<System.Action>[] {
			(next) => {
				masterWeaponRequest.Get(
					(MasterDataCollection<MasterWeapon> result) => {
						MasterDataCache<MasterWeapon>.Instance.Set(result);
						next();
					}
				);
			},
			(next) => {
				userWeaponRequest.Get(
					(MasterDataCollection<UserWeapon> result) => {
						MasterDataCache<UserWeapon>.Instance.Set(result);
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
