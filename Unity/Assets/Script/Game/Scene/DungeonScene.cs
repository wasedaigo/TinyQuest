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
			//Roga2dTiledSprite map = new Roga2dTiledSprite("Dungeon/sample", 5, 5);
			//map.LocalPriority = 0.05f;
			//this.AddChild(map);
			
			Roga2dRenderObject renderObject = new Roga2dRenderObject("Dungeon/sample", new Vector2(32, 32), new Vector2(0, 0), new Rect(0, 0, 64, 64));
			Roga2dButton button = new Roga2dButton(renderObject);
			this.AddChild(button);
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
				
			// Construct a ray from the current mouse coordinates
			if (Input.GetMouseButtonDown(0)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hitInfo = new RaycastHit();
				if (Physics.Raycast(ray, out hitInfo)) {
					hitInfo.collider.SendMessage("ReceiveTouch");
				}
			}
		}
	}
}