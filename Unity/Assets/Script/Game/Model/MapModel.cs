using UnityEngine;
using TinyQuest.Entity;

namespace TinyQuest.Model {
	public class MapModel {
		public delegate void StepMovedEventHandler(float x, float y, float duration);
		public event StepMovedEventHandler StepMoved;
		
		private int currentStepId;
		private StepData[] steps; 
		
		public MapModel(StepData[] steps)
		{
			this.currentStepId = 0;
			this.steps = steps;
		}
		
		public StepData[] GetSteps() {
			return this.steps;
		}
		
		private StepData getStepById(int stepId) {
			for (int i = 0; i < this.steps.Length; i++) {
				if (this.steps[i].StepId == stepId) {
					return this.steps[i];	
				}
			}
			
			return null;
		}

		public void MoveTo(int stepId) {
			this.currentStepId = stepId;
			StepData step = this.getStepById(stepId);
			if (step != null && this.StepMoved != null) {
				this.StepMoved(step.PosX, step.PosY, 90);	
			}
		}
	}
}