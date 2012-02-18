using TinyQuest.Component;
using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Core;
using TinyQuest.Scene;

namespace TinyQuest.Component {
	public class MapNavigator : Roga2dNode {

		private void addSymbol(float x, float y, Roga2dButton.OnTouchDelegate symbolTouched) {
			// Symbols
			Roga2dButton button = new Roga2dButton();
			button.LocalPriority = 0.1f;
			button.UpRenderObject = new Roga2dRenderObject("Dungeon/symbols", new Vector2(24, 24), new Vector2(8, 8), new Rect(0, 0, 32, 32));
			button.DownRenderObject = new Roga2dRenderObject("Dungeon/symbols", new Vector2(24, 24), new Vector2(8, 8), new Rect(32, 0, 32, 32));
			button.OnTouched = symbolTouched;
			button.LocalPixelPosition = new Vector2(x, y);
			this.AddChild(button);
		}
		
		// Use this for initialization
		public MapNavigator(Roga2dButton.OnTouchDelegate symbolTouched) {
			// BG
			Roga2dRenderObject renderObject = new Roga2dRenderObject("bg/map", new Vector2(160, 100), new Vector2(-80, -50), new Rect(20, 300, 160, 100));
			Roga2dSprite sprite = new Roga2dSprite(renderObject);
			this.AddChild(sprite);
	
			this.addSymbol(50, 50, symbolTouched);
		}
		
	}
}