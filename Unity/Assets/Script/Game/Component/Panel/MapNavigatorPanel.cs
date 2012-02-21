using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Core;
using TinyQuest.Model;

namespace TinyQuest.Component.Panel {
	public class MapNavigatorPanel : Roga2dNode {
		public delegate void StepTouchEventHandler(int stepIndex);
		public event StepTouchEventHandler StepTouched;
		
		private void addStep(int no, float x, float y) {
			// Steps
			Roga2dButton button = new Roga2dButton();
			button.Tag = no;
			button.LocalPriority = 0.1f;
			button.UpRenderObject = new Roga2dRenderObject("Dungeon/symbols", new Vector2(24, 24), new Vector2(8, 8), new Rect(0, 0, 32, 32));
			button.DownRenderObject = new Roga2dRenderObject("Dungeon/symbols", new Vector2(24, 24), new Vector2(8, 8), new Rect(32, 0, 32, 32));
			button.OnTouched = this.onTouched;
			button.LocalPixelPosition = new Vector2(x, y);
			this.AddChild(button);
		}
		
		// Use this for initialization
		public MapNavigatorPanel() {
			// BG
			Roga2dRenderObject renderObject = new Roga2dRenderObject("bg/map", new Vector2(160, 100), new Vector2(-80, -50), new Rect(20, 300, 160, 100));
			Roga2dSprite sprite = new Roga2dSprite(renderObject);
			this.AddChild(sprite);
	
			this.addStep(0, 50, 50);
			this.addStep(1, 100, 50);
		}
		
		private void onTouched(Roga2dButton button) {
			if (StepTouched != null) {
				StepTouched((int)button.Tag);	
			}
		}
	}
}