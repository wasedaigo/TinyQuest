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
			
			sPlayerGroupNo = 1;

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
					combatUnit.groupNo = i;
				}
			}

			data.combatUnitGroups[0].fightingUnitIndex = 0;
			data.combatUnitGroups[1].fightingUnitIndex = 0;
			data.combatUnitGroups[0].standByUnitIndex = 1;
			data.combatUnitGroups[1].standByUnitIndex = 1;
			
			this._isRequesting = false;
			callback();
	    }

	    protected override IEnumerator HandleSendTurnInput(WWW www, int standByUnitIndex, bool forceSwap, System.Action<ActionResult> callback)
	    {
			this._isRequesting = true;
			yield return new WaitForSeconds(0.5f);
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
			
			data.combatUnitGroups[0].standByUnitIndex = standByUnitIndex;
			data.combatUnitGroups[1].standByUnitIndex = this.GetFirstAliveStandByUnitIndex(1);
			
			TurnCommand[] turnCommands = new TurnCommand[2];
			for (int i = 0; i < turnCommands.Length; i++) {
				turnCommands[i] = new TurnCommand();
				turnCommands[i].skillIndex = 0;
				turnCommands[i].forceSwap = false;
			}
			turnCommands[0].forceSwap = forceSwap;
			
			this.ProcessActions(turnCommands, (ActionResult actionResult)=>{
				this._isRequesting = false;
				callback(actionResult);
			});
	    }
	}
}
