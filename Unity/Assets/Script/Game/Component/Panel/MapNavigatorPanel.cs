using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Core;
using TinyQuest.Model;

namespace TinyQuest.Component.Panel {
	public class MapNavigatorPanel : BasePanel {
		public delegate void StepTouchEventHandler(int stepIndex);
		public event StepTouchEventHandler StepTouched;
		private Roga2dNode floor;
		private Vector2 mapSize;
		private Vector2 panelSize;
		private Vector2 scrollVelocity;
		
		private void addStep(int no, float x, float y) {
			// Steps
			Roga2dButton button = new Roga2dButton();
			button.Tag = no;
			button.LocalPriority = 0.1f;
			button.SetUpSprite("Dungeon/symbols", new Vector2(24, 24), new Vector2(12, 12), new Rect(0, 0, 32, 32));
			button.SetDownSprite("Dungeon/symbols", new Vector2(24, 24), new Vector2(12, 12), new Rect(32, 0, 32, 32));
			button.OnTouched = this.onTouched;
			button.LocalPixelPosition = new Vector2(x, y);
			this.floor.AddChild(button);
		}
		
		// Use this for initialization
		public MapNavigatorPanel() {
			this.mapSize = new Vector2(512, 512);

			// BG
			this.floor = new Roga2dNode();
			Roga2dSprite sprite = new Roga2dSprite("bg/map", new Vector2(512, 512), new Vector2(0, 0), new Rect(0, 0, 512, 512));
			sprite.LocalPixelPosition = new Vector2(256, 256);
			this.floor.AddChild(sprite);
			this.AddChild(this.floor);
	
			this.addStep(0, 300, 50);
			this.addStep(1, 150, 400);
			this.addStep(2, 100, 300);
			this.addStep(3, 50, 400);

			sprite = new Roga2dSprite("Dungeon/piece", new Vector2(32, 32), new Vector2(16, 32), new Rect(0, 0, 64, 64));
			sprite.LocalPixelPosition = new Vector2(316, 66);
			sprite.LocalPriority = 0.2f;
			this.floor.AddChild(sprite);
			
			this.floor.LocalPixelPosition = new Vector2(-sprite.LocalPixelPosition.x + Config.PanelWidth / 2 + 16, sprite.LocalPixelPosition.y + Config.PanelHeight / 2);
		}
		
		private void onTouched(Roga2dButton button) {
			if (StepTouched != null) {
				StepTouched((int)button.Tag);	
			}
		}
		
		private void scrollFloor(Vector2 delta) {
			Vector2 pos = this.floor.LocalPixelPosition + delta;
			if (pos.x > 0) { pos.x = 0; }
			if (pos.y > 0) { pos.y = 0; }
			if (pos.x < -this.mapSize.x + Config.PanelWidth) { pos.x = -this.mapSize.x + Config.PanelWidth; }
			if (pos.y < -this.mapSize.y + Config.PanelHeight) { pos.y = -this.mapSize.y + Config.PanelHeight; }

			this.floor.LocalPixelPosition = pos;
		}
		
		public override void OnTouchMoved(Vector2 delta) {
			this.scrollVelocity = delta;
		}
		
		public override void Update() {
			base.Update();
			this.scrollFloor(this.scrollVelocity);
			this.scrollVelocity *= 0.9f;
			if (Mathf.Abs(this.scrollVelocity.x) < 0.1f) {this.scrollVelocity.x = 0.0f;}
			if (Mathf.Abs(this.scrollVelocity.y) < 0.1f) {this.scrollVelocity.y = 0.0f;}
		}
	}
}