using UnityEngine;
using TinyQuest.Entity;

namespace TinyQuest.Model {
	public class MapModel {
		public delegate void StepMovedEventHandler(float x, float y);
		public event StepMovedEventHandler StepMoved;
		
		private StepData[] steps; 
		
		public MapModel(StepData[] steps)
		{
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
			StepData step = this.getStepById(stepId);
			if (step != null && this.StepMoved != null) {
				this.StepMoved(step.PosX, step.PosY);	
			}
		}
	}
}