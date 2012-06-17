using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;
using TinyQuest.Data.Request;

namespace TinyQuest.Model {

	public class ZoneModel {
		public  enum PostCommandState {
			None,
			NextStep,
			ClearZone
		};
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

		private Dictionary<int, ZoneEventModel> events =  new Dictionary<int, ZoneEventModel>();
		private UserZone userZone;
		private BattlerModel playerBattler;
		
		public ZoneModel(UserZone userZone) {
			this.userZone = userZone;
		}
		
		public void SetEvent(int stepNo, ZoneEventModel zoneEvent) {
			this.events.Add(stepNo, zoneEvent);
		}
		
		public void SetPlayerBattler(BattlerModel playerBattler) {
			this.playerBattler = playerBattler;
		}
		
		public BattlerModel GetPlayerBattler() {
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
			req.GetExecutingCommand(this.OnCommandProgressed);
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
			Debug.Log("[StepProgressed] " + this.userZone.stepIndex);
			bool hasEvent = false;
			if (this.StepProgress != null) {
				int stepIndex = this.userZone.stepIndex;
				hasEvent = this.userZone.events.ContainsKey(stepIndex.ToString());
				this.StepProgress(stepIndex, hasEvent);
			}
			
			LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
			req.GetExecutingCommand(this.OnCommandProgressed);
		}
		
		// Event
		public void NextCommand() {
			LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
			req.ProgressCommand(this.OnCommandProgressed);
		}

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
	}
}
