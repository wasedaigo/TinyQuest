using TinyQuest.Entity;
using TinyQuest.Core;
using UnityEngine;
namespace TinyQuest.Scene {
	public delegate void PanelWindowMessageEvent(PanelWindowMessage message);
	public class PanelWindow : Roga2dNode {
		public event PanelWindowMessageEvent MessageEvent;
		private Ally player;
		private bool isPressed;
		private Collider pressedCollider;

		
		public PanelWindow()
		{
			// Player
			//this.player = new Ally("lilia");
			//this.player.LocalPriority = 1.0f;
			//this.AddChild(player);

			
			CombatControlPanel combatControlPanel = new CombatControlPanel(this.OnTouched);
			this.AddChild(combatControlPanel);
			combatControlPanel.LocalPriority = -0.2f;
			combatControlPanel.LocalPixelPosition = new Vector2(-80, 20);
			
			/*
			MapNavigator mapNavigator = new MapNavigator(this.OnTouched);
			mapNavigator.LocalPriority = -0.1f;
			mapNavigator.LocalPixelPosition = new Vector2(-80, 20);
			this.AddChild(mapNavigator);
			 */
			// TileMap
			//RevealableTileMap tileMap = new RevealableTileMap();
			//tileMap.LocalPriority = 0.5f;
			//this.AddChild(tileMap);
		}
			
		public void OnTouched(Roga2dButton button) {
			if (MessageEvent != null) {
				// PanelWindowMessage message = new PanelWindowMessage(PanelWindowMessageType.FloorSymbolTouched, button.Tag);
				PanelWindowMessage message = new PanelWindowMessage(PanelWindowMessageType.CombatCardTouched, button.Tag);
				MessageEvent(message);
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