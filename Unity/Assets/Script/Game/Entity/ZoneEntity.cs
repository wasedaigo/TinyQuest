using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;
using TinyQuest.Data.Request;

namespace TinyQuest.Entity {

	public class ZoneEntity {
		public System.Action<float> PlayerMove;
		public System.Action<int, bool> StepProgress;
		public System.Action<ZoneCommand, object> CommandExecute;
		public System.Action GotoNextStep;
		public System.Action ClearZone;
		
		private static readonly int StepDistance = 100;
		private static readonly int Speed = 100;
		private float moveDistance;
		public float MoveDistance {
			get{return this.moveDistance;}
		}

		private Dictionary<int, ZoneEventEntity> events =  new Dictionary<int, ZoneEventEntity>();
		private UserZoneProgress userZoneProgress;
		private UserZone userZone;
		
		public ZoneEntity(UserZoneProgress userZoneProgress, UserZone userZone) {
			this.userZoneProgress = userZoneProgress;
			this.userZone = userZone;
		}
		
		public void SetEvent(int stepNo, ZoneEventEntity zoneEvent) {
			this.events.Add(stepNo, zoneEvent);
		}
		
		public bool IsAtStart() {
			int stepIndex = this.userZoneProgress.stepIndex;
			int commandIndex = this.userZoneProgress.commandIndex;
			
			return stepIndex == 0 && commandIndex == 0;
		}
		
		// Step
		public void StartAdventure() {
			this.OnCommandProgressed();
		}

		public void MoveForward() {
			float delta = Speed * Time.deltaTime;
			this.moveDistance += delta;
			
			if (this.moveDistance >= StepDistance) {
				delta = this.moveDistance - StepDistance;
				this.moveDistance = 0;

				LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
				req.ProgressStep(this.OnStepProgressed);
			}
			
			if (this.PlayerMove != null) {
				this.PlayerMove(delta);
			}
		}
		
		private void OnStepProgressed() {
			Debug.Log("[StepProgressed] " + this.userZoneProgress.stepIndex);
			bool hasEvent = false;
			if (this.StepProgress != null) {
				int stepIndex = this.userZoneProgress.stepIndex;
				hasEvent = this.userZone.events.ContainsKey(stepIndex.ToString());
				this.StepProgress(stepIndex, hasEvent);
			}
			
			this.OnCommandProgressed();
		}
		
		// Event

		public void NextCommand() {
			LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
			req.ProgressCommand(this.OnCommandProgressed);
		}
		
		private void OnCommandProgressed() {
			int stepIndex = this.userZoneProgress.stepIndex;
			int commandIndex = this.userZoneProgress.commandIndex;

			bool allCommandFinished = true;
			string key = stepIndex.ToString();
			if (this.userZone.events.ContainsKey(key)) {
				ZoneEvent zoneEvent = this.userZone.events[stepIndex.ToString()];
				ZoneCommand[] commands = zoneEvent.commands;
				if (commandIndex < commands.Length) {
					ZoneCommand command = commands[commandIndex];
					this.CommandExecute(command, null);
					allCommandFinished = false;
				}
			}
			
			if (allCommandFinished) {
				if (this.userZoneProgress.stepIndex >= this.userZone.lastStepIndex) {
					this.ClearZone();
				} else {
					this.GotoNextStep();
				}
			}
		}
	}
}
