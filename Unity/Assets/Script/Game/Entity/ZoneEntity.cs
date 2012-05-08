using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;
using TinyQuest.Data.Request;

namespace TinyQuest.Entity {

	public class ZoneEntity {
		public System.Action<float> PlayerMove;
		public System.Action<int> StepProgress;
		
		private static readonly int StepDistance = 300;
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
		
		public void MoveForward() {
			float delta = Speed * Time.deltaTime;
			this.moveDistance += delta;
			
			if (this.moveDistance >= StepDistance) {
				delta = this.moveDistance - StepDistance;
				this.moveDistance = 0;
				LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
				req.ProgressStep(this.stepProgressed);
			}
			
			if (this.PlayerMove != null) {
				this.PlayerMove(delta);
			}
		}

		private void stepProgressed() {
			Debug.Log(this.userZoneProgress.progressStep);
			if (this.StepProgress != null) {
				this.StepProgress(this.userZoneProgress.progressStep);
			}
		}

		public void SetEvent(int stepNo, ZoneEventEntity zoneEvent) {
			this.events.Add(stepNo, zoneEvent);
		}
	}
}
