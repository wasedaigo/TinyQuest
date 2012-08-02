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
		private static int turn = 0;
		private class StartBattleResponse {
			public readonly int assignedGroupNo;
			public readonly int gameNo;
			public readonly CombatUnitGroup[] combatUnitGroups;
			
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
		
		public virtual void StartBattle(MonoBehaviour monoBehaviour, System.Action<CombatUnitGroup[]> callback) {
			WWW www = new WWW(APIDomain + "start_battle"); 
	        monoBehaviour.StartCoroutine(this.HandleStartBattle(www, callback));
		}
		
	    private IEnumerator<WWW> HandleStartBattle(WWW www, System.Action<CombatUnitGroup[]> callback)
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
				LocalUserData data = cache.Data;
				callback(data.combatUnitGroups);
				turn = 1;
	        } else {
	            Debug.Log("WWW Error: "+ www.error);
	        }    
	    }
	
		public static CombatUnit GetFirstAliveUnit(int groupType) {
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
			List<CombatUnit> combatUnitList = data.combatUnitGroups[groupType].combatUnits;
			foreach (CombatUnit combatUnit in combatUnitList) {
				if (combatUnit.hp > 0) {
					return combatUnit;
				}
			}
			
			return null;
		}
		
		private struct CombatProcessBlock {
			public CombatUnit caster;
			public CombatUnit target;
			
			public CombatProcessBlock(CombatUnit caster, CombatUnit target) {
				this.caster = caster;
				this.target = target;
			}
			
			public CombatAction Execute() {
				if (this.caster.IsDead || this.target.IsDead) { return null; }
				MasterSkill skill = CacheFactory.Instance.GetMasterDataCache().GetSkillByID(this.caster.GetUserUnit().Unit.normalAttack);
				
				int effect = this.caster.GetUserUnit().Power * skill.multiplier / 100;
				this.target.hp -= effect;
				if (this.target.hp < 0) {
					this.target.hp = 0;	
				}
				
				CombatActionResult targetResult = new CombatActionResult();
				targetResult.life = this.target.hp;
				targetResult.maxLife = this.target.GetUserUnit().MaxHP;
				targetResult.effect = effect;
				targetResult.combatUnit = this.target;
				
				if (targetResult.life == 0) {
					targetResult.swapUnit = LocalUserDataRequest.GetFirstAliveUnit(this.target.groupType);
				}
				
				LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
				data.combatProgress = new CombatProgress();
				
				return new CombatAction(this.caster, this.target, skill, null, targetResult);
			}
		}

		public virtual void ProgressTurn(MonoBehaviour monoBehaviour, int playerIndex, System.Action<CombatUnit, CombatUnit, List<CombatAction>> callback) {
			WWWForm form = new WWWForm();
			form.AddField("playerGroupType", CombatGroupInfo.Instance.GetPlayerGroupType(0));
			form.AddField("playerIndex", playerIndex);
			form.AddField("turn", turn);
			WWW www = new WWW(APIDomain + "progress_turn", form); 
	        monoBehaviour.StartCoroutine(this.HandleProgressTurn(www, playerIndex, callback));
		}

		private bool IsPlayerFirst(CombatUnit playerUnit, CombatUnit enemyUnit) {
			if (playerUnit.GetUserUnit().Speed > enemyUnit.GetUserUnit().Speed) {
				return true;
			}
			
			if (playerUnit.GetUserUnit().Speed == enemyUnit.GetUserUnit().Speed) {
				return playerUnit.groupType > enemyUnit.groupType;
			}

			return false;
		}
		
	    private IEnumerator HandleProgressTurn(WWW www, int playerIndex, System.Action<CombatUnit, CombatUnit, List<CombatAction>> callback)
	    {
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
				}


				int opponentIndex = progressTurnResponse.opponentIndex;
				
				LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
	
				CombatUnit playerUnit = data.combatUnitGroups[CombatGroupInfo.Instance.GetPlayerGroupType(0)].combatUnits[playerIndex];
				CombatUnit enemyUnit = data.combatUnitGroups[CombatGroupInfo.Instance.GetPlayerGroupType(1)].combatUnits[opponentIndex];
				Debug.Log("playerIndex:"+playerIndex+" opponentIndex:"+opponentIndex);
				// Add actions
				List<CombatAction> combatActions = new List<CombatAction>();
				
				CombatProcessBlock[] blocks = new CombatProcessBlock[CombatGroupInfo.Instance.GetGroupCount()];
				if (this.IsPlayerFirst(playerUnit, enemyUnit)) {
					blocks[0] = new CombatProcessBlock(playerUnit, enemyUnit);
					blocks[1] = new CombatProcessBlock(enemyUnit, playerUnit);
				} else {
					blocks[0] = new CombatProcessBlock(enemyUnit, playerUnit);
					blocks[1] = new CombatProcessBlock(playerUnit, enemyUnit);
				}
	
				for (int i = 0; i < blocks.Length; i++) {
					CombatAction action = blocks[i].Execute();
					if (action != null) {
						combatActions.Add(action);
					}
				}
				
				data.combatProgress.turnCount += 1;
				CacheFactory.Instance.GetLocalUserDataCache().Commit();
				
				turn++;
				callback(playerUnit, enemyUnit, combatActions);

	        } else {
	            Debug.Log("WWW Error: "+ www.error);
	        }    
	    }
	
		
		public virtual void FinishCombat(System.Action callback) {
			callback();
		}
	}
}
