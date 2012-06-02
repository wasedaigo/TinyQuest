using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using System.IO;
using TinyQuest.Data;
using TinyQuest.Entity;
using TinyQuest.Factory.Entity;
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
		
		public virtual void GetExecutingCommand(System.Action<ZoneEntity.PostCommandState, ZoneCommand, object> callback) {
			UserZone userZone = CacheFactory.Instance.GetLocalUserDataCache().GetUserZone();
			int stepIndex = userZone.stepIndex;
			int commandIndex = userZone.commandIndex;
			
			ZoneEntity.PostCommandState postCommandState = ZoneEntity.PostCommandState.None;
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
					postCommandState = ZoneEntity.PostCommandState.ClearZone;
				} else {
					postCommandState = ZoneEntity.PostCommandState.NextStep;
				}
			} else {
				switch((ZoneCommand.Type)command.type) {
				case ZoneCommand.Type.Battle:
					CombatProgress combatProgress = CacheFactory.Instance.GetLocalUserDataCache().GetCombatProgress();
					if (combatProgress == null) {
						CombatBattler playerBattlerData = new CombatBattler(0, (int)BattlerEntity.GroupType.Player, 100, new int[]{});
						CombatBattler enemyBattlerData = new CombatBattler(1, (int)BattlerEntity.GroupType.Enemy, 100, new int[]{});
						combatProgress = new CombatProgress(1, new CombatBattler[][]{
							new CombatBattler[]{playerBattlerData},
							new CombatBattler[]{enemyBattlerData}
						});
						CacheFactory.Instance.GetLocalUserDataCache().SetCombatProgress(combatProgress);
					}
					break;
				}
			}
			
			callback(postCommandState, command, zoneCommandState);
		}
		
		public virtual void ProgressCommand(System.Action<ZoneEntity.PostCommandState, ZoneCommand, object> callback) {
			UserZone userZone = CacheFactory.Instance.GetLocalUserDataCache().GetUserZone();
			userZone.commandIndex += 1;

			CacheFactory.Instance.GetLocalUserDataCache().Commit();			
			
			this.GetExecutingCommand(callback);
		}
		
		public virtual void StartCombat(System.Action<int[]> callback) {
			int[] drawnSkillIndexes = new int[MaxHandCount]{-1, -1, -1};
			
			CombatProgress combatProgress = CacheFactory.Instance.GetLocalUserDataCache().GetCombatProgress();
			if (combatProgress != null) {
				CombatBattler battler = combatProgress.battlers[(int)BattlerEntity.GroupType.Player][0];

				for (int i = 0; i < MaxHandCount; i++) {
					if (battler.handSkillIndexes[i] >= 0) {
						drawnSkillIndexes[i] = battler.handSkillIndexes[i];
					}
				}
			}
			
			callback(drawnSkillIndexes);
		}
		
		public virtual void ProcessCombat(BattlerEntity caster, BattlerEntity target, System.Action callback) {
			CombatProgress combatProgress = CacheFactory.Instance.GetLocalUserDataCache().GetCombatProgress();
			if (combatProgress == null) {
				CombatBattler playerBattlerData = new CombatBattler(0, (int)BattlerEntity.GroupType.Player, 100, new int[]{});
				CombatBattler enemyBattlerData = new CombatBattler(1, (int)BattlerEntity.GroupType.Enemy, 100, new int[]{});
				combatProgress = new CombatProgress(1, new CombatBattler[][]{
					new CombatBattler[]{playerBattlerData},
					new CombatBattler[]{enemyBattlerData}
				});
				CacheFactory.Instance.GetLocalUserDataCache().SetCombatProgress(combatProgress);
			}
			
			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			callback();
		}
		
		public virtual void UseSkill(int handIndex, BattlerEntity.GroupType groupType, int battlerIndex, System.Action<int, int> callback) {
			CombatProgress combatProgress = CacheFactory.Instance.GetLocalUserDataCache().GetCombatProgress();
			CombatBattler battler = combatProgress.battlers[(int)groupType][battlerIndex];
			int skillIndex = battler.handSkillIndexes[handIndex];
			battler.handSkillIndexes[handIndex] = -1;

			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			callback(handIndex, skillIndex);
		}
		
		public virtual void UseCompositeSkill(int id, System.Action callback) {
			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			callback();
		}
		
		private List<int> GetLibrarySkillList(CombatBattler battler, Dictionary<int, SkillEntity> skillIndexMap) {
			List<int> librarySkillIndexList = new List<int>();
			for (int i = 0; i < skillIndexMap.Count; i++) {
				librarySkillIndexList.Add(i);
			}

			for (int i = 0; i < MaxHandCount; i++) {
				if (battler.handSkillIndexes[i] != -1) {
					librarySkillIndexList.Remove(battler.handSkillIndexes[i]);
				}
			}
			
			return librarySkillIndexList;
		}

		public virtual void DrawSkills(BattlerEntity.GroupType groupType, int battlerIndex, Dictionary<int, SkillEntity> skillIndexMap, System.Action<SkillEntity[]> callback) {
			CombatProgress combatProgress = CacheFactory.Instance.GetLocalUserDataCache().GetCombatProgress();
			CombatBattler battler = combatProgress.battlers[(int)groupType][battlerIndex];
			List<int> librarySkillIndexList = this.GetLibrarySkillList(battler, skillIndexMap);

			SkillEntity[] drawnSkills = new SkillEntity[MaxHandCount]{null, null, null};
			for (int i = 0; i < MaxHandCount; i++) {
				if (battler.handSkillIndexes[i] == -1) {
					int index = Random.Range(0, librarySkillIndexList.Count - 1);
					int chosenSkillIndex = librarySkillIndexList[index];
					battler.handSkillIndexes[i] = chosenSkillIndex;
					drawnSkills[i] = skillIndexMap[chosenSkillIndex];
				} else {
					drawnSkills[i] = skillIndexMap[battler.handSkillIndexes[i]];
				}
			}
			
			SkillEntity skill1 = skillIndexMap[battler.handSkillIndexes[0]];
			SkillEntity skill2 = skillIndexMap[battler.handSkillIndexes[1]];
			SkillEntity skill3 = skillIndexMap[battler.handSkillIndexes[2]];
			
			CompositeData data = CacheFactory.Instance.GetMasterDataCache().GetCompositeData(
				skill1 != null ? skill1.MasterSkill.id : 0, 
				skill2 != null ? skill2.MasterSkill.id : 0, 
				skill3 != null ? skill3.MasterSkill.id : 0
			);
			
			if (data != null) {
					
			}
			Debug.Log(data);	
			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			callback(drawnSkills);
		}
	}
}
