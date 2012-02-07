using TinyQuest.Component;
using UnityEngine;
namespace TinyQuest.Scene {
	public class DungeonScene : Roga2dNode {
		private Ally player;
		private bool isPressed;
		private Collider pressedCollider;
		public DungeonScene()
		{
			// Player
			this.player = new Ally("lilia");
			this.player.LocalPriority = 1.0f;
			this.AddChild(player);

			// TileMap
			RevealableTileMap tileMap = new RevealableTileMap();
			this.AddChild(tileMap);
			
			Roga2dButton button = new Roga2dButton();
			button.UpRenderObject = new Roga2dRenderObject("Dungeon/sample", new Vector2(32, 32), new Vector2(0, 0), new Rect(0, 0, 64, 64));
			button.DownRenderObject = new Roga2dRenderObject("death_wind", new Vector2(32, 32), new Vector2(0, 0), new Rect(0, 0, 64, 64));
			button.OnTouched = this.OnTouched;
			this.AddChild(button);
		}
			
		public void OnTouched() {
			Vector2 position = this.player.LocalPixelPosition;
			this.player.LocalPixelPosition = new Vector2(position.x - 1, position.y);
		}
			
		public override void Update ()
		{
	
			base.Update ();
			
			// Construct a ray from the current mouse coordinates
			if (Input.GetMouseButtonDown(0)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hitInfo = new RaycastHit();
				if (Physics.Raycast(ray, out hitInfo)) {
					this.pressedCollider = hitInfo.collider;
					if (this.pressedCollider != null) {
						this.pressedCollider.SendMessage("ReceiveTouchDown");
					}
				}
			}
			
			if (Input.GetMouseButtonUp(0)) {
				if (this.pressedCollider != null) {
					this.pressedCollider.SendMessage("ReceiveTouchUp");
					this.pressedCollider = null;
				}
			}
		}
	}
}