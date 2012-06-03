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
			int skillId = battler.handSkills[handIndex];
			battler.handSkills[handIndex] = 0;
			
			MasterCompositeSkill masterCompositeSkill = CacheFactory.Instance.GetMasterDataCache().GetCompositeSkillById(skillId);
			if (masterCompositeSkill == null) {
				battler.librarySkills.Add(skillId);
			} else {
				if (masterCompositeSkill.baseSkill1 > 0) {
					battler.librarySkills.Add(masterCompositeSkill.baseSkill1);
				}
				if (masterCompositeSkill.baseSkill2 > 0) {
					battler.librarySkills.Add(masterCompositeSkill.baseSkill2);
				}
				if (masterCompositeSkill.baseSkill3 > 0) {
					battler.librarySkills.Add(masterCompositeSkill.baseSkill3);
				}
			}

			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			callback(handIndex, skillId);
		}
		
		public virtual void UseCompositeSkill(int id, System.Action callback) {
			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			callback();
		}

		public virtual void DrawSkills(BattlerEntity.GroupType groupType, int battlerIndex, bool initialDraw, System.Action<SkillEntity[], CompositeData> callback) {
			CombatProgress combatProgress = CacheFactory.Instance.GetLocalUserDataCache().GetCombatProgress();
			CombatBattler battler = combatProgress.battlers[(int)groupType][battlerIndex];

			SkillEntity[] drawnSkills = new SkillEntity[MaxHandCount]{null, null, null};
			for (int i = 0; i < MaxHandCount; i++) {
				int handSkill = battler.handSkills[i];
				if (initialDraw) {
					if (handSkill > 0) {
						drawnSkills[i] = SkillFactory.Instance.Build(handSkill);
					}
				} else {
					if (handSkill == 0) {
						int index = Random.Range(0, battler.librarySkills.Count - 1);
						int chosenSkillId = battler.librarySkills[index];
						battler.librarySkills.RemoveAt(index);
						battler.handSkills[i] = chosenSkillId;
						drawnSkills[i] = SkillFactory.Instance.Build(chosenSkillId);
					}
				}
			}
			
			CompositeData compositeData = CacheFactory.Instance.GetMasterDataCache().GetCompositeData(
				battler.handSkills[0], 
				battler.handSkills[1], 
				battler.handSkills[2]
			);
			
			
			// Set composite skill data
			if (compositeData != null) {
				bool isFirstSkillSet = false;
				for (int i = 0; i < compositeData.BaseSkills.Length; i++) {
					if (compositeData.BaseSkills[i] > 0) {
						
						if (isFirstSkillSet) {
							battler.handSkills[i] = 0;
						} else {
							battler.handSkills[i] = compositeData.Skill;
							isFirstSkillSet = true;
						}
					}
				}
				
				
			}

			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			callback(drawnSkills, compositeData);
		}
	}
}
