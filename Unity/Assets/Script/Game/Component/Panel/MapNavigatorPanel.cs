using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Core;
using TinyQuest.Entity;
using TinyQuest.Model;
using TinyQuest.Cache;

namespace TinyQuest.Component.Panel {
	public class MapNavigatorPanel : BasePanel {
		public delegate void StepTouchEventHandler(int stepIndex);
		public event StepTouchEventHandler StepTouched;
		
		public event WindowMessageEventHandler WindowMessageInvoked;
		
		private Roga2dNode floor;
		private Roga2dNode camera;
		private Roga2dSprite playerPiece;
		private Vector2 mapSize;
		private Vector2 panelSize;
		private Vector2 scrollVelocity;
		private MapModel mapModel;
		
		// Use this for initialization
		public MapNavigatorPanel(MapModel mapModel) {
			this.mapModel = mapModel;
			this.mapSize = new Vector2(256, 256);

			// BG
			this.floor = new Roga2dNode();
			Roga2dSprite sprite = new Roga2dSprite("bg/map", new Vector2(256, 256), new Vector2(0, 0), new Rect(0, 0, 512, 512));
			sprite.LocalPixelPosition = new Vector2(128, 128);
			this.floor.AddChild(sprite);
			this.AddChild(this.floor);
			
			// Camera
			this.camera = new Roga2dNode();
			this.AddChild(this.camera);

			this.playerPiece = new Roga2dSprite("Dungeon/piece", new Vector2(16, 16), new Vector2(8, 16), new Rect(0, 0, 64, 64));
			this.playerPiece.LocalPriority = 0.2f;
			this.playerPiece.LocalPixelPosition = new Vector2(32, 32);
			this.floor.AddChild(this.playerPiece);
			
			this.camera.LocalPixelPosition = new Vector2(68, 64);
		}
		
		public override void Init() {
		}

		private void setScreenCenter(float posX, float posY) {
			this.floor.LocalPixelPosition = new Vector2(-posX + Config.PanelWidth / 2, -posY + Config.PanelHeight / 2);
		}
		
		
		public void OnStepMoved(StepData step, float duration) {
			Vector2 piecePixelPos = this.playerPiece.LocalPixelPosition;
			Vector2 piecePos = Roga2dUtils.pixelToLocal(new Vector2(piecePixelPos.x, piecePixelPos.y));
			Vector2 movePos = Roga2dUtils.pixelToLocal(new Vector2(step.PosX + 16, step.PosY + 16));
			Vector2 cameraPos = this.camera.LocalPosition;
			Vector2 cameraFocusPos = Roga2dUtils.pixelToLocal(new Vector2(piecePixelPos.x - 16, piecePixelPos.y - 16));
			Vector2 cameraMovePos = Roga2dUtils.pixelToLocal(new Vector2(step.PosX , step.PosY));
			
			Roga2dFunc func = new Roga2dFunc(this.onPieceMoved);
			
			// Move camera and piece at the same time
			Roga2dParallel parallel = new Roga2dParallel(new List<Roga2dBaseInterval> {
				new Roga2dPositionInterval(this.playerPiece, piecePos, movePos, duration, true, null),
				new Roga2dPositionInterval(this.camera, cameraFocusPos, cameraMovePos, duration, true, null)
			});
			
			// At first move back to the piece position
			float distance = Vector2.Distance(cameraPos, piecePos);
			float cameraFocusDuration = distance * 0.1f;
			Roga2dSequence sequence = new Roga2dSequence(new List<Roga2dBaseInterval> {
				new Roga2dPositionInterval(this.camera, cameraPos, cameraFocusPos, cameraFocusDuration, true, null),
				parallel,
				func
			});
			
			Roga2dIntervalPlayer.GetInstance().Play(sequence);
		}
		
		// After whole movement, invoke the next state action
		private void onPieceMoved() {
			if (this.WindowMessageInvoked != null) {
				WindowMessage message = new WindowMessage(WindowMessageType.StartCombat, null);
				this.WindowMessageInvoked(message);	
			}
		}
		
		private void onStepTouched(Roga2dButton button) {
			int stepId = (int)button.Tag;
			this.mapModel.MoveTo(stepId);
			if (StepTouched != null) {
				StepTouched(stepId);	
			}
		}
		
		public override void OnTouchMoved(Vector2 delta) {
			this.scrollVelocity = new Vector2(0, delta.y);
		}

		private void scrollFloor(Vector2 delta) {
			Vector2 pos = this.camera.LocalPixelPosition - delta;
			if (pos.y < 52) { pos.y = 52; }
			if (pos.y > 220) { pos.y = 220; }

			this.camera.LocalPixelPosition = pos;
		}
		
		public override void Update() {
			base.Update();
			this.scrollFloor(this.scrollVelocity);
			this.scrollVelocity *= 0.9f;
			if (Mathf.Abs(this.scrollVelocity.x) < 0.1f) {this.scrollVelocity.x = 0.0f;}
			if (Mathf.Abs(this.scrollVelocity.y) < 0.1f) {this.scrollVelocity.y = 0.0f;}
			
			this.setScreenCenter(this.camera.LocalPixelPosition.x, this.camera.LocalPixelPosition.y);
		}
	}
}