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
			
			sPlayerGroupNo = 0;

			TextAsset txt = (TextAsset)Resources.Load("Data/CombatMock", typeof(TextAsset));
			Debug.Log("WWW Ok!: " + txt.text);
			CacheFactory.Instance.GetLocalUserDataCache().SetData(txt.text);
			
			LocalUserData data  = CacheFactory.Instance.GetLocalUserDataCache().Data;
			if (sPlayerGroupNo != 0) {
				CombatUnitGroup temp = data.combatUnitGroups[0];
				data.combatUnitGroups[0] = data.combatUnitGroups[1];
				data.combatUnitGroups[1] = temp;
			}
			
			for (int i = 0; i < data.combatUnitGroups.Length; i++) {
				for (int j = 0; j < data.combatUnitGroups[i].combatUnits.Count; j++) {
					CombatUnit combatUnit = data.combatUnitGroups[i].combatUnits[j];
					combatUnit.groupType = i;
				}
			}
			data.combatUnitGroups[0].fightingUnitIndex = -1;
			data.combatUnitGroups[1].fightingUnitIndex = 0;
			
			this._isRequesting = false;
			callback();
	    }
			
	    protected override IEnumerator HandleProgressTurn(WWW www, int playerIndex, int turn, System.Action callback)
	    {
			this._isRequesting = false;
			yield return true;
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
			
			data.combatUnitGroups[0].fightingUnitIndex = playerIndex;
			data.combatUnitGroups[1].fightingUnitIndex = this.GetFirstAliveUnit(1).index;
			
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
