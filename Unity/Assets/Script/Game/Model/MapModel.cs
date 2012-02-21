namespace TinyQuest.Model {
	public class MapModel {
		public delegate void StepMoveStartEventHandler();
		public event StepMoveStartEventHandler StepMoveStart;

		
		public MapModel()
		{
			//this.steps = steps;
		}

		public void moveTo(int stepIndex) {
			if (this.StepMoveStart != null) {
				this.StepMoveStart();	
			}
		}
	}
}