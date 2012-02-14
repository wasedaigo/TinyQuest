using TinyQuest.Component;
using UnityEngine;
namespace TinyQuest.Scene {
	public class DungeonScene : Roga2dNode {
		public delegate void SymbolTouchEvent(object sender);
		public event SymbolTouchEvent SymbolTouched;
		
		private Ally player;
		private bool isPressed;
		private Collider pressedCollider;
		
		private void addSymbol(float x, float y) {
			// Symbols
			Roga2dButton button = new Roga2dButton();
			button.LocalPriority = 0.1f;
			button.UpRenderObject = new Roga2dRenderObject("Dungeon/symbols", new Vector2(24, 24), new Vector2(8, 8), new Rect(0, 0, 32, 32));
			button.DownRenderObject = new Roga2dRenderObject("Dungeon/symbols", new Vector2(24, 24), new Vector2(8, 8), new Rect(32, 0, 32, 32));
			button.OnTouched = this.OnTouched;
			button.LocalPixelPosition = new Vector2(x, y);
			this.AddChild(button);
		}
		
		public DungeonScene()
		{
			// Player
			//this.player = new Ally("lilia");
			//this.player.LocalPriority = 1.0f;
			//this.AddChild(player);
			
			// BG
			Roga2dRenderObject renderObject = new Roga2dRenderObject("Dungeon/sample_bg", new Vector2(144, 160), new Vector2(0, 0), new Rect(0, 0, 256, 256));
			Roga2dSprite sprite = new Roga2dSprite(renderObject);
			this.AddChild(sprite);
			
			// TileMap
			//RevealableTileMap tileMap = new RevealableTileMap();
			//tileMap.LocalPriority = 0.5f;
			//this.AddChild(tileMap);
			
			this.addSymbol(20, -20);
			this.addSymbol(-50, -30);
			this.addSymbol(20, 40);
		}
			
		public void OnTouched(Roga2dButton button) {
			if (SymbolTouched != null) {
				SymbolTouched(this);
			}
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
						object obj = hitInfo.point;
						this.pressedCollider.SendMessage("ReceiveTouchDown", obj);
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