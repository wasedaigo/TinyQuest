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
						CombatBattler playerBattlerData = new CombatBattler(CombatBattlerType.Player, 0, 10,  new int[]{});
						CombatBattler enemyBattlerData = new CombatBattler(CombatBattlerType.Monster, 0, 100, new int[]{});
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
				CombatBattler playerBattlerData = new CombatBattler(CombatBattlerType.Player, 100, 10, new int[]{});
				CombatBattler enemyBattlerData = new CombatBattler(CombatBattlerType.Monster, 100, 10, new int[]{});
				combatProgress = new CombatProgress(1, new CombatBattler[][]{
					new CombatBattler[]{playerBattlerData},
					new CombatBattler[]{enemyBattlerData}
				});
				CacheFactory.Instance.GetLocalUserDataCache().SetCombatProgress(combatProgress);
			}
			
			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			callback();
		}
		
		public virtual void UseSkill(BattlerEntity casterEntity, BattlerEntity targetEntity, System.Action<int> callback) {
			CombatProgress combatProgress = CacheFactory.Instance.GetLocalUserDataCache().GetCombatProgress();
			CombatBattler caster = combatProgress.battlers[(int)casterEntity.Group][casterEntity.No];
			CombatBattler target = combatProgress.battlers[(int)targetEntity.Group][targetEntity.No];
			
			int skillId = 1;
			
			//caster.tp -= 
			target.hp -= 1;
			targetEntity.SetHP(target.hp);
			
			SkillEntity skillEntity = SkillFactory.Instance.Build(skillId);

			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			callback(skillId);
		}
	}
}
