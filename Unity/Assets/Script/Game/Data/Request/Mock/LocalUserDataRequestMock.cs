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
			
	    protected override IEnumerator HandleProgressTurn(WWW www, int playerIndex, int turn)
	    {
			this._isRequesting = false;
			yield return true;
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
			data.fightingUnitIndexes[CombatGroupInfo.Instance.GetPlayerGroupType(0)] = playerIndex;
			data.fightingUnitIndexes[CombatGroupInfo.Instance.GetPlayerGroupType(1)] = this.GetFirstAliveUnit(CombatGroupInfo.Instance.GetPlayerGroupType(1)).index;
			this._isRequesting = false;
			
	    }
	}
}
