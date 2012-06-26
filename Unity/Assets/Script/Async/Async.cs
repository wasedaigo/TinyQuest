using System;
using System.Collections;
using System.Collections.Generic;

namespace Async {
	public class Async {
		public static readonly Async Instance = new Async();
		private Async(){}
		private int loadCount;
		private int loadedCount;
		private System.Action callback;
		private List<System.Action<System.Action>> actions;
		
		public void Parallel(List<System.Action<System.Action>> actions, System.Action callback) {
			this.actions = actions;
			this.loadCount = actions.Count;
			if (this.loadCount == 0) {
				callback();
			} else {
				this.loadedCount = 0;
				this.callback = callback;
				for (int i = 0; i < actions.Count; i++) {
					actions[i](this.parallelCallback);
				}
			}
		}
		
		private void parallelCallback() {
			this.loadedCount++;
			if (this.loadedCount == this.loadCount) {
				this.callback();
			}
		}
		
		public void Waterfall(List<System.Action<System.Action>> actions, System.Action callback) {
			this.actions = actions;
			this.loadCount = actions.Count;
			if (this.loadCount == 0) {
				callback();
			} else {
				this.loadedCount = 0;
				this.callback = callback;
				
				this.actions[0](this.waterfallCallback);
			}
		}
		
		private void waterfallCallback() {
			this.loadedCount++;
			if (this.loadedCount == this.loadCount) {
				this.callback();
			} else {
				this.actions[this.loadedCount](this.waterfallCallback);
			}
		}
		

	}
}