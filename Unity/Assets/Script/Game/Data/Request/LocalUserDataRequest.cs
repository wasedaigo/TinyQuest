using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;
using System.IO;
using TinyQuest.Data;
using TinyQuest.Scene.Model;
using TinyQuest.Data.Cache;
using TinyQuest.Data.Skills;

namespace TinyQuest.Data.Request {
	public class LocalUserDataRequest
	{	
		protected static int sPlayerGroupNo;

		private static string APIDomain = "http://1.tiny-quest.appspot.com/api/";
		private class StartBattleResponse {
			public readonly int assignedGroupNo;
			public readonly int gameNo;
			public readonly CombatUnitGroup[] combatUnitGroups;
			public readonly int[] fightingUnitIndexes;
		}
		
		public class TurnCommand {
			public int skillIndex;
			public bool forceSwap;
		}
		
		private class ProgressTurnResponse {
			public readonly bool valid;
			public readonly int opponentIndex;
			public readonly TurnCommand[] turnCommands;
		}
		
		public LocalUserDataRequest() {
		}
		
		private struct CombatProcessBlock {
			public CombatUnit caster;
			public CombatUnit target;
			
			public CombatProcessBlock(CombatUnit caster, CombatUnit target) {
				this.caster = caster;
				this.target = target;
			}

			public CombatAction Execute(int skillIndex) {
				if (this.caster.IsDead || this.target.IsDead) { return null; }

				BaseSkill skill = SkillFactory.Instance.GetSkill(this.caster.GetUserUnit().Unit.skills[skillIndex]);
				
				BaseSkill.SkillResult result = skill.Calculate(this.caster);

				int effect = result.damage;
				this.target.hp -= effect;
				if (this.target.hp < 0) {
					this.target.hp = 0;	
				}
				
				CombatActionResult targetResult = new CombatActionResult();
				targetResult.life = this.target.hp;
				targetResult.maxLife = this.target.GetUserUnit().MaxHP;
				targetResult.effect = effect;
				targetResult.combatUnit = this.target;

				return new CombatAction(this.caster, this.target, result, null, targetResult);
			}
		}
		
		
		public struct ActionResult {
			public List<CombatAction> combatActions;
			public CombatUnit activePlayerUnit;
			public CombatUnit activeOpponentUnit;
		}
		
		public void ProcessActions(TurnCommand[] turnCommands, System.Action<ActionResult> callback) {
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
			
			ActionResult actionResult = new ActionResult();
			actionResult.combatActions = new List<CombatAction>();

			// Add actions
			
			List<CombatProcessBlock> blocks = new List<CombatProcessBlock>();
			
			
			for (int i = 0; i < Constant.GroupCount; i++) {
				if (turnCommands[i].forceSwap) {
					CombatUnitGroup group = data.combatUnitGroups[i];
					if (group.fightingUnitIndex != group.standByUnitIndex) {
						group.fightingUnitIndex = group.standByUnitIndex;
						if (group.fightingUnitIndex >= 0) {
							group.combatUnits[group.fightingUnitIndex].revealed = true;
						}
						group.standByUnitIndex = RequestFactory.Instance.GetLocalUserRequest().GetFirstAliveStandByUnitIndex(i);
					}
				}
			}
			
			actionResult.activePlayerUnit = data.combatUnitGroups[0].combatUnits[data.combatUnitGroups[0].fightingUnitIndex];
			actionResult.activeOpponentUnit = data.combatUnitGroups[1].combatUnits[data.combatUnitGroups[1].fightingUnitIndex];
			
			
			if (!turnCommands[0].forceSwap) {
				blocks.Add(new CombatProcessBlock(actionResult.activePlayerUnit, actionResult.activeOpponentUnit));
			}
			
			if (!turnCommands[1].forceSwap) {
				blocks.Add(new CombatProcessBlock(actionResult.activeOpponentUnit, actionResult.activePlayerUnit));
			}

			// Calculations
			for (int i = 0; i < blocks.Count; i++) {
				CombatAction action = blocks[i].Execute(turnCommands[i].skillIndex);
				if (action != null) {
					actionResult.combatActions.Add(action);
				}
			}

			for (int i = 0; i < Constant.GroupCount; i++) {
				CombatUnitGroup group = data.combatUnitGroups[i];
				CombatUnit fightingUnit = group.combatUnits[group.fightingUnitIndex];
				if (fightingUnit.IsDead) {
					if (group.fightingUnitIndex != group.standByUnitIndex) {
						group.fightingUnitIndex = group.standByUnitIndex;
						if (group.fightingUnitIndex >= 0) {
							group.combatUnits[group.fightingUnitIndex].revealed = true;
						}
						group.standByUnitIndex = RequestFactory.Instance.GetLocalUserRequest().GetFirstAliveStandByUnitIndex(i);
					}
				}
			}

			data.turnCount += 1;
			CacheFactory.Instance.GetLocalUserDataCache().Commit();

			callback(actionResult);
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
				
				sPlayerGroupNo = startBattleResponse.assignedGroupNo;
				
				CacheFactory.Instance.GetLocalUserDataCache().SetData(response);
				LocalUserDataCache cache = CacheFactory.Instance.GetLocalUserDataCache();
				LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
				data.combatUnitGroups[0].fightingUnitIndex = startBattleResponse.fightingUnitIndexes[0];
				data.combatUnitGroups[1].fightingUnitIndex = startBattleResponse.fightingUnitIndexes[1];
				
				callback();
	        } else {
	            Debug.Log("WWW Error: "+ www.error);
	        } 
			
			_isRequesting = false;
	    }
	
		public int GetFirstAliveStandByUnitIndex(int groupType) {
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
			CombatUnitGroup combatUnitGroup = data.combatUnitGroups[groupType];
			foreach (CombatUnit combatUnit in combatUnitGroup.combatUnits) {
				if (combatUnit.hp > 0 && combatUnit.index != combatUnitGroup.fightingUnitIndex) {
					return combatUnit.index;
				}
			}
			
			return -1;
		}
		
		
		protected bool _isRequesting;
		public bool IsRequesting {
			get {
				return _isRequesting;	
			}
		}

		public virtual void SendTurnInput(MonoBehaviour monoBehaviour, int standByUnitIndex, bool forceSwap, System.Action<ActionResult> callback) {
			Debug.Log("SendTurnInput");
			WWWForm form = new WWWForm();
			form.AddField("playerGroupNo", sPlayerGroupNo);
			form.AddField("standByUnitIndex", standByUnitIndex);
			WWW www = new WWW(APIDomain + "progress_turn", form); 
			
	        monoBehaviour.StartCoroutine(this.HandleSendTurnInput(www, standByUnitIndex, forceSwap, callback));
		}

		
	    protected virtual IEnumerator HandleSendTurnInput(WWW www, int standByUnitIndex, bool forceSwap, System.Action<ActionResult> callback)
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
					form.AddField("playerGroupType", 0);
					form.AddField("standByUnitIndex", standByUnitIndex);
					www = new WWW(APIDomain + "progress_turn", form); 

					yield return www;
					response = www.text;
	            	Debug.Log("WWW Ok!: " + response);
					progressTurnResponse = JsonReader.Deserialize<ProgressTurnResponse>(response);
					
					LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
					data.combatUnitGroups[0].standByUnitIndex = standByUnitIndex;
					data.combatUnitGroups[1].standByUnitIndex = progressTurnResponse.opponentIndex;
					TurnCommand[] turnCommands = progressTurnResponse.turnCommands;
					
					this.ProcessActions(turnCommands, (ActionResult actionResult)=>{
						this._isRequesting = false;
						callback(actionResult);
					});
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
