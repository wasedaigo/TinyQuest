using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using TinyQuest.Data.Request;

namespace TinyQuest.Scene.Model {

	public class ZoneModel {
		public  enum PostCommandState {
			None,
			NextStep,
			ClearZone
		};
		public System.Action<float> PlayerMove;
		public System.Action<ZoneCommand, object> CommandExecute;
		public System.Action GotoNextStep;
		public System.Action ClearZone;

		private static readonly int StepDistance = 150;
		private static readonly int Speed = 100;
		private float moveDistance;
		public float MoveDistance {
			get{return this.moveDistance;}
		}

		private UserZone userZone;
		private UserUnit playerBattler;
		
		public ZoneModel() {
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
			this.userZone =  data.zone;
		}
		
		public void SetPlayerBattler(UserUnit playerBattler) {
			this.playerBattler = playerBattler;
		}
		
		public UserUnit GetPlayerBattler() {
			return this.playerBattler;
		}
		
		public bool IsAtStart() {
			int stepIndex = this.userZone.stepIndex;
			int commandIndex = this.userZone.commandIndex;
			
			return stepIndex == 0 && commandIndex == 0;
		}
		
		// Step
		public void StartAdventure() {
			LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
			req.ExecuteCommand(this.OnCommandProgressed);
		}

		public void MoveForward() {
			float delta = Speed * Time.deltaTime;
			this.moveDistance += delta;
			if (this.moveDistance >= StepDistance) {
				delta = this.moveDistance - StepDistance;
				this.moveDistance = 0;

				LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
				req.ProgressStep(this.OnCommandProgressed);
			}
			
			if (this.PlayerMove != null) {
				this.PlayerMove(delta);
			}
		}
		
		// Check whether it is executing event or not
		public bool IsCommandExecuting() {
			// Get UserZone
			UserZone userZone = CacheFactory.Instance.GetLocalUserDataCache().GetUserZone();
			int stepIndex = userZone.stepIndex;
			int commandIndex = userZone.commandIndex;
			
			// Get current zoneEvent
			ZoneEvent zoneEvent = userZone.events[stepIndex.ToString()];
			if (zoneEvent == null) {return false;}
			
			// Check current command index
			ZoneCommand[] commands = zoneEvent.commands;
			return commandIndex < commands.Length;
		}
		
		// Invoke next command
		public void NextCommand() {
			LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
			req.ProgressCommand(this.OnCommandProgressed);
		}

		// Handle command result
		private void OnCommandProgressed(ZoneModel.PostCommandState postCommandState, ZoneCommand command, object zoneCommandState) {
			switch (postCommandState) {
			case PostCommandState.None:
				this.CommandExecute(command, zoneCommandState);
				break;
			case PostCommandState.ClearZone:
				this.ClearZone();
				break;
			case PostCommandState.NextStep:
				this.GotoNextStep();
				break;
			}
		}
		
		public CombatUnit GetWalkingUnit() {
			CombatUnitGroup[] combatUnitGroups = this.GetCombatUnits();
			return combatUnitGroups[0].combatUnits[combatUnitGroups[0].activeIndex];
		}
		
		public CombatUnitGroup[] GetCombatUnits() {
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
			return data.combatUnitGroups;
		}

		public CombatUnit GetCombatUnit(int groupType, int index) {
			CombatUnitGroup[] combatUnitGroups = this.GetCombatUnits();
			return combatUnitGroups[groupType].combatUnits[index];
		}
	}
}
