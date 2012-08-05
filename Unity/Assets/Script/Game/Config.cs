using UnityEngine;

namespace TinyQuest {
	public class Config {
		public static readonly bool IsMockEnabled = true;
		public static readonly int LogicalWidth = 320;
		public static readonly int LogicalHeight = 480;
		
		private static float actualLogicalRatio = 0.0f;
		public static float ActualLogicalRatio {
			get {
				if (actualLogicalRatio == 0.0f) {
					actualLogicalRatio = Config.LogicalWidth / (float)Screen.width;
				}
				return actualLogicalRatio;
			}
		}
	}
}
 