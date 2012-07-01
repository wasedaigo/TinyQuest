using System;
using System.Collections;
using System.Collections.Generic;

namespace Async {
	public class Async {
		public static readonly Async Instance = new Async();
		private Async(){}
		
		public void Parallel(List<System.Action<System.Action>> actions, System.Action callback) {
			int loadCount = actions.Count;
			if (loadCount == 0) {
				callback();
			} else {
				int loadedCount = 0;
				for (int i = 0; i < actions.Count; i++) {
				actions[i](
					()=>{ this.parallelCallback(ref loadedCount, loadCount, callback); }
				);
				}
			}
		}
		
		private void parallelCallback(ref int loadedCount, int loadCount, System.Action callback) {
			loadedCount++;
			if (loadedCount == loadCount) {
				callback();
			}
		}
		
		public void Waterfall(List<System.Action<System.Action>> actions, System.Action callback) {
			if (actions.Count == 0) {
				callback();
			} else {
				actions[0](
					()=>{ this.waterfallCallback(0, actions, callback); }
				);
			}
		}
		
		private void waterfallCallback(int loadedCount, List<System.Action<System.Action>> actions, System.Action callback) {
			loadedCount++;
			if (loadedCount == actions.Count) {
				callback();
			} else {
				actions[loadedCount](
					()=>{ this.waterfallCallback(loadedCount, actions, callback); }
				);
			}
		}
		

	}
}