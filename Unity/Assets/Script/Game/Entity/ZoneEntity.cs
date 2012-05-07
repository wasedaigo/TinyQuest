using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;
using TinyQuest.Data.Request;

namespace TinyQuest.Entity {

	public class ZoneEntity {
		public System.Action<float> playerMove;
		public System.Action<int> stepProgress;
		
		private static readonly int StepDistance = 1000;
		private static readonly int Speed = 75;
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
		
		public void MoveForward() {
			float delta = Speed * Time.deltaTime;
			this.moveDistance += delta;
			
			if (this.moveDistance >= StepDistance) {
				delta = this.moveDistance - StepDistance;
				this.moveDistance = 0;
				LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
				req.ProgressStep(this.stepProgressed);
			}
			
			if (this.playerMove != null) {
				this.playerMove(delta);
			}
		}

		private void stepProgressed() {
			Debug.Log(this.userZoneProgress.progressStep);
			if (this.stepProgress != null) {
				this.stepProgress(this.userZoneProgress.progressStep);
			}
		}

		public void SetEvent(int stepNo, ZoneEventEntity zoneEvent) {
			this.events.Add(stepNo, zoneEvent);
		}
	}
}
