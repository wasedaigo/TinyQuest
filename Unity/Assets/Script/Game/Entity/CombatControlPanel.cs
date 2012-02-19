using UnityEngine;

namespace TinyQuest.Entity {
	public class CombatControlPanel : Roga2dNode {
		
		private void AddCard(int no, float x, float y, Roga2dButton.OnTouchDelegate symbolTouched) {
			// Symbols
			Roga2dButton button = new Roga2dButton();
			button.LocalPriority = 0.1f;
			button.UpRenderObject = new Roga2dRenderObject("UI/buttons", new Vector2(32, 32), new Vector2(16, 16), new Rect(0, 0, 32, 32));
			button.DownRenderObject = new Roga2dRenderObject("UI/buttons", new Vector2(32, 32), new Vector2(16, 16), new Rect(32, 0, 32, 32));
			button.OnTouched = symbolTouched;
			button.LocalPixelPosition = new Vector2(x, y);
			button.Tag = no;
			this.AddChild(button);
		}

		// Use this for initialization
		public CombatControlPanel(Roga2dButton.OnTouchDelegate symbolTouched) {
			// BG
			Roga2dRenderObject renderObject = new Roga2dRenderObject("UI/panel_bg", new Vector2(160, 100), new Vector2(-80, -50), new Rect(0, 0, 160, 100));
			Roga2dSprite sprite = new Roga2dSprite(renderObject);
			this.AddChild(sprite);
			
			this.AddCard(0, 30, 25, symbolTouched);
			this.AddCard(1, 80, 25, symbolTouched);
			this.AddCard(2, 130, 25, symbolTouched);
			this.AddCard(3, 30, 65, symbolTouched);
			this.AddCard(4, 80, 65, symbolTouched);
			this.AddCard(5, 130, 65, symbolTouched);
		}
		
	}
}