using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using System.IO;
using TinyQuest.Data;
using TinyQuest.Scene.Model;
using TinyQuest.Data.Cache;

namespace TinyQuest.Data.Request {
	public class LocalUserDataRequest
	{
		const int MaxHandCount = 3;
		public virtual void Get(System.Action<string> callback) {
		}
		
		public virtual void ProgressStep(System.Action callback) {
			UserZone userZone = CacheFactory.Instance.GetLocalUserDataCache().GetUserZone();
			userZone.stepIndex += 1;
			userZone.commandIndex = 0;
			
			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			callback();
		}
		
		public virtual void GetExecutingCommand(System.Action<ZoneModel.PostCommandState, ZoneCommand, object> callback) {
			UserZone userZone = CacheFactory.Instance.GetLocalUserDataCache().GetUserZone();
			int stepIndex = userZone.stepIndex;
			int commandIndex = userZone.commandIndex;
			
			ZoneModel.PostCommandState postCommandState = ZoneModel.PostCommandState.None;
			ZoneCommand command = null;
			object zoneCommandState = null;
			
			bool allCommandFinished = true;
			string key = stepIndex.ToString();
			if (userZone.events.ContainsKey(key)) {
				ZoneEvent zoneEvent = userZone.events[stepIndex.ToString()];
				ZoneCommand[] commands = zoneEvent.commands;
				if (commandIndex < commands.Length) {
					command = commands[commandIndex];
					zoneCommandState = userZone.commandState;
					allCommandFinished = false;
				}
			}
			
			if (allCommandFinished) {
				if (userZone.stepIndex >= userZone.lastStepIndex) {
					postCommandState = ZoneModel.PostCommandState.ClearZone;
				} else {
					postCommandState = ZoneModel.PostCommandState.NextStep;
				}
			} else {
				switch((ZoneCommand.Type)command.type) {
				case ZoneCommand.Type.Battle:
					/*
					CombatProgress combatProgress = CacheFactory.Instance.GetLocalUserDataCache().GetCombatProgress();
					if (combatProgress == null) {
						CombatUnit playerBattlerData = new CombatUnit(1, 0, 0, 100, new int[]{});
						CombatUnit enemyBattlerData = new CombatUnit(2, 1, 0, 100, new int[]{});
						combatProgress = new CombatProgress(1, new CombatUnit[][]{
							new CombatUnit[]{playerBattlerData},
							new CombatUnit[]{enemyBattlerData}
						});
						CacheFactory.Instance.GetLocalUserDataCache().SetCombatProgress(combatProgress);
					}
					*/
					break;
				}
			}
			
			callback(postCommandState, command, zoneCommandState);
		}

		public virtual void StartBattle(System.Action<List<CombatUnit>> callback) {
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
			CombatProgress combatProgress = data.combatProgress;
			if (combatProgress == null) {
				List<CombatUnit> combatUnits = new List<CombatUnit>();
				for (int i = 0; i < data.ownUnits.Count; i++) {
					UserUnit playerUnit = data.ownUnits[i];
					combatUnits.Add(new CombatUnit(playerUnit.id, 0, i, 100, new int[]{})); // Player	
				}
				
				UserUnit enemyUnit = data.zoneUnits[0];
				combatUnits.Add(new CombatUnit(enemyUnit.id, 1, 0, 200, new int[]{})); // Enemy
				
				data.combatProgress = new CombatProgress(combatUnits);
			}

			CacheFactory.Instance.GetLocalUserDataCache().Commit();			
			
			callback(data.combatProgress.combatUnits);
		}
		
		public virtual void ProgressCommand(System.Action<ZoneModel.PostCommandState, ZoneCommand, object> callback) {
			UserZone userZone = CacheFactory.Instance.GetLocalUserDataCache().GetUserZone();
			userZone.commandIndex += 1;

			CacheFactory.Instance.GetLocalUserDataCache().Commit();			
			
			this.GetExecutingCommand(callback);
		}
		
		public virtual void UseSkill(CombatUnit casterUnit, CombatUnit targetUnit, System.Action<MasterSkill, CombatUnit, CombatUnit> callback) {
			CombatProgress combatProgress = CacheFactory.Instance.GetLocalUserDataCache().GetCombatProgress();

			int skillId = 1;

			//caster.tp -= 
			//targetUnit.hp -= 1;

			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			
			MasterSkill masterSkill = CacheFactory.Instance.GetMasterDataCache().GetSkillByID(skillId);
			callback(masterSkill, casterUnit, targetUnit);
		}
	}
}
