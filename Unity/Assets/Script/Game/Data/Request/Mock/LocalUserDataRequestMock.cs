using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;
using System.IO;
using TinyQuest.Data;
using TinyQuest.Scene.Model;
using TinyQuest.Data.Cache;

namespace TinyQuest.Data.Request {
	

	public class LocalUserDataRequestMock : LocalUserDataRequest
	{
	    protected override IEnumerator HandleStartBattle(WWW www, System.Action callback)
	    {
			yield return true;
			this._isRequesting = true;
			
			CombatGroupInfo.Instance.SetPlayerGroupType(0);

			TextAsset txt = (TextAsset)Resources.Load("Data/CombatMock", typeof(TextAsset));
			Debug.Log("WWW Ok!: " + txt.text);
			CacheFactory.Instance.GetLocalUserDataCache().SetData(txt.text);	
			this._isRequesting = false;
			callback();
	    }
			
	    protected override IEnumerator HandleProgressTurn(WWW www, int playerIndex, int turn, System.Action callback)
	    {
			this._isRequesting = false;
			yield return true;
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
			data.fightingUnitIndexes[CombatGroupInfo.Instance.GetPlayerGroupType(0)] = playerIndex;
			data.fightingUnitIndexes[CombatGroupInfo.Instance.GetPlayerGroupType(1)] = this.GetFirstAliveUnit(CombatGroupInfo.Instance.GetPlayerGroupType(1)).index;
			
			data.skillRands = new int[2];
			for (int i = 0; i < data.skillRands.Length; i++) {
				data.skillRands[i] = Random.Range(0, 100);
			}
			
			data.featureRands = new int[2];
			for (int i = 0; i < data.featureRands.Length; i++) {
				data.featureRands[i] = Random.Range(0, 100);
			}
			
			data.turnRand = Random.Range(0, 100);
			
			this._isRequesting = false;
			callback();
	    }
	}
}
