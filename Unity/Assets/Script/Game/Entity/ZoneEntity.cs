using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;
using TinyQuest.Data.Request;

namespace TinyQuest.Entity {

	public class ZoneEntity {
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
		
		public void MoveForward() {
			this.moveDistance += Speed;
			if (this.moveDistance >= StepDistance) {
				this.moveDistance = 0;
				LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
				req.ProgressStep(this.stepProgressed);
				// Increment step
			}
		}
		
		private void stepProgressed() {
			Debug.Log(this.userZoneProgress.progressStep);
		}

		public void SetEvent(int stepNo, ZoneEventEntity zoneEvent) {
			this.events.Add(stepNo, zoneEvent);
		}
	}
}
