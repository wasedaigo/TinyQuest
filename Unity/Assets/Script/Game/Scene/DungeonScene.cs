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

			// TileMap
			Roga2dTiledSprite map = new Roga2dTiledSprite("Dungeon/sample", 5, 5);
			map.LocalPriority = 0.05f;
			this.AddChild(map);
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