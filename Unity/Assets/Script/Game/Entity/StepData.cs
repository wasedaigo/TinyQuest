using UnityEngine;
using System.Collections.Generic;

namespace TinyQuest.Entity {
	public class StepData {
		private int stepId;
		public int StepId { get { return this.stepId; } }
		
		private float posX;
		public float PosX { get { return this.posX; } }
		private float posY;
		public float PosY { get { return this.posY; } }
		
		public StepData(int stepId, float posX, float posY) 
		{
			this.stepId = stepId;
			this.posX = posX;
			this.posY = posY;
		}
	}
}