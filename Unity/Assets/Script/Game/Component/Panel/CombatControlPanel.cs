using UnityEngine;

namespace TinyQuest.Component.Panel {
	public class CombatControlPanel : BasePanel {
		public delegate void CardSelectEventHandler(int cardIndex);
		public event CardSelectEventHandler CardSelected;
		
		private void AddCard(int no, float x, float y) {
			// Symbols
			Roga2dButton button = new Roga2dButton();
			button.LocalPriority = 0.1f;
			button.SetUpSprite("UI/buttons", new Vector2(32, 32), new Vector2(16, 16), new Rect(0, 0, 32, 32));
			button.SetDownSprite("UI/buttons", new Vector2(32, 32), new Vector2(16, 16), new Rect(32, 0, 32, 32));
			button.OnTouched = this.onTouched;
			button.LocalPixelPosition = new Vector2(x, y);
			button.Tag = no;
			
			int tx = no % 3;
			int ty = no / 3;
			
			Roga2dSprite sprite = new Roga2dSprite("UI/weapon", new Vector2(32, 32), new Vector2(16, 16), new Rect(64 * tx, 56 * ty, 64, 56));
			sprite.LocalPriority = 0.1f;
			sprite.LocalPixelPosition = new Vector2(16, 16);
			button.AddChild(sprite);
			this.AddChild(button);
		}

		// Use this for initialization
		public CombatControlPanel() {
			// BG
			Roga2dSprite sprite = new Roga2dSprite("UI/panel_bg", new Vector2(160, 100), new Vector2(-80, -50), new Rect(0, 0, 160, 100));
			this.AddChild(sprite);
			
			this.AddCard(0, 30, 25);
			this.AddCard(1, 80, 25);
			this.AddCard(2, 130, 25);
			this.AddCard(3, 30, 65);
			this.AddCard(4, 80, 65);
			this.AddCard(5, 130, 65);
		}
		
		private void onTouched(Roga2dButton button) {
			if (CardSelected != null) {
				CardSelected((int)button.Tag);	
			}
		}
	}
}