using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;
using System.IO;
using TinyQuest.Data;
using TinyQuest.Scene.Model;
using TinyQuest.Data.Cache;

namespace TinyQuest.Data.Request {
	public class LocalUserDataRequest
	{	
		private static string APIDomain = "http://1.tiny-quest.appspot.com/api/";
		private class StartBattleResponse {
			public readonly int assignedGroupNo;
			public readonly int gameNo;
			public readonly CombatUnitGroup[] combatUnitGroups;
			public readonly int[] fightingUnitIndexes;
		}
		
		private class ProgressTurnResponse {
			public readonly bool valid;
			public readonly int opponentIndex;
		}
		
		public LocalUserDataRequest() {
			/*
			Hashtable hashtable = new Hashtable();
			hashtable["my_name"] = "Daigo";
			string jsonStringFromObj = JsonFx.Json.JsonWriter.Serialize(hashtable);
			byte[] postDataBytes = System.Text.Encoding.ASCII.GetBytes(jsonStringFromObj);
			*/
		}
		
		public virtual void StartBattle(MonoBehaviour monoBehaviour, System.Action callback) {
			WWW www = new WWW(APIDomain + "start_battle"); 
	        monoBehaviour.StartCoroutine(this.HandleStartBattle(www, callback));
		}
		
	    protected virtual IEnumerator HandleStartBattle(WWW www, System.Action callback)
	    {
	        yield return www;

	        // check for errors
	        if (www.error == null)
	        {
				string response = www.text;
	            Debug.Log("WWW Ok!: " + response);
				
				StartBattleResponse startBattleResponse = JsonReader.Deserialize<StartBattleResponse>(response);	
				CombatGroupInfo.Instance.SetPlayerGroupType(startBattleResponse.assignedGroupNo);
				
				CacheFactory.Instance.GetLocalUserDataCache().SetData(response);
				LocalUserDataCache cache = CacheFactory.Instance.GetLocalUserDataCache();
				LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
				data.fightingUnitIndexes = startBattleResponse.fightingUnitIndexes;

				callback();
	        } else {
	            Debug.Log("WWW Error: "+ www.error);
	        } 
			
			_isRequesting = false;
	    }
	
		public CombatUnit GetFirstAliveUnit(int groupType) {
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
			List<CombatUnit> combatUnitList = data.combatUnitGroups[groupType].combatUnits;
			foreach (CombatUnit combatUnit in combatUnitList) {
				if (combatUnit.hp > 0) {
					return combatUnit;
				}
			}
			
			return null;
		}
		
		
		protected bool _isRequesting;
		public bool IsRequesting {
			get {
				return _isRequesting;	
			}
		}

		public virtual void ProgressTurn(MonoBehaviour monoBehaviour, int playerIndex, int turn) {
			Debug.Log("ProgressTurn");
			WWWForm form = new WWWForm();
			form.AddField("playerGroupType", CombatGroupInfo.Instance.GetPlayerGroupType(0));
			form.AddField("playerIndex", playerIndex);
			form.AddField("turn", turn);
			WWW www = new WWW(APIDomain + "progress_turn", form); 
			
	        monoBehaviour.StartCoroutine(this.HandleProgressTurn(www, playerIndex, turn));
		}

		
	    protected virtual IEnumerator HandleProgressTurn(WWW www, int playerIndex, int turn)
	    {
			Debug.Log("HandleProgressTurnA");
	        yield return www;

	        // check for errors
	        if (www.error == null)
	        {
				string response = www.text;
	            Debug.Log("WWW Ok!: " + response);
				ProgressTurnResponse progressTurnResponse = JsonReader.Deserialize<ProgressTurnResponse>(response);

				while (!progressTurnResponse.valid) {
					yield return new WaitForSeconds(3.0f); 
					WWWForm form = new WWWForm();
					form.AddField("playerGroupType", CombatGroupInfo.Instance.GetPlayerGroupType(0));
					form.AddField("playerIndex", playerIndex);
					form.AddField("turn", turn);
					www = new WWW(APIDomain + "progress_turn", form); 

					yield return www;
					response = www.text;
	            	Debug.Log("WWW Ok!: " + response);
					progressTurnResponse = JsonReader.Deserialize<ProgressTurnResponse>(response);
					
					LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
					data.fightingUnitIndexes[CombatGroupInfo.Instance.GetPlayerGroupType(0)] = playerIndex;
					data.fightingUnitIndexes[CombatGroupInfo.Instance.GetPlayerGroupType(1)] = progressTurnResponse.opponentIndex;
				}
	        } else {
	            Debug.Log("WWW Error: "+ www.error);
	        }   
			
			_isRequesting = false;
	    }
	
		
		public virtual void FinishCombat(System.Action callback) {
			callback();
		}
	}
}
