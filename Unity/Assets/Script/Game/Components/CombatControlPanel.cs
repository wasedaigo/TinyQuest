using UnityEngine;

namespace TinyQuest.Component {
	public class CombatControlPanel : Roga2dNode {
		
		// Use this for initialization
		public CombatControlPanel() {
			// BG
			Roga2dRenderObject renderObject = new Roga2dRenderObject("bg/map", new Vector2(160, 100), new Vector2(-80, -50), new Rect(20, 300, 160, 100));
			Roga2dSprite sprite = new Roga2dSprite(renderObject);
			this.AddChild(sprite);
		}
		
	}
}