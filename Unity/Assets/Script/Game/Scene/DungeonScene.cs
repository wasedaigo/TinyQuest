using TinyQuest.Component;
using UnityEngine;
namespace TinyQuest.Scene {
	public class DungeonScene : Roga2dNode {
		private Ally player;
		private bool isPressed;
		public DungeonScene()
		{
			// Player
			this.player = new Ally("lilia");
			this.player.LocalPriority = 0.1f;
			this.AddChild(player);
			
			// Stage
			Stage stage = new Stage();
			stage.LocalPriority = 0.0f;
			this.AddChild(stage);
		}
		
		public override void Update ()
		{
			base.Update ();
			if (Input.GetMouseButtonUp(0)) {
				isPressed = false;
			}
			if (Input.GetMouseButtonDown(0)) {
				isPressed = true;
			}
			if (isPressed) {
				Vector2 position = this.player.LocalPixelPosition;
				this.player.LocalPixelPosition = new Vector2(position.x - 1, position.y);
			}
		}
	}
}