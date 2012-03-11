using UnityEngine;

namespace TinyQuest {
	public class Config {
		public static int PanelWidth = 160;
		public static int PanelHeight = 120;
		public static int LogicalWidth = 160;
		public static int LogicalHeight = 240;
		
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
 