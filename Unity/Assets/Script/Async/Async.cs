using System;

namespace Async {
	public class Async {
		public static readonly Async Instance = new Async();
		private Async(){}
		private int loadCount;
		private int loadedCount;
		private System.Action callback;
		public void Parallel(IAsync[] objects, System.Action callback) {
			this.loadCount = objects.Length;
			this.loadedCount = 0;
			this.callback = callback;
			for (int i = 0; i < objects.Length; i++) {
				objects[i].Load(this.LoadFinished);	
			}
		}
		
		private void LoadFinished() {
			this.loadedCount++;
			if (this.loadedCount == this.loadCount) {
				this.callback();
			}
		}
	}
}