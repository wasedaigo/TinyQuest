using TinyQuest.Core;
using UnityEngine;
namespace TinyQuest.Entity {
	public enum PanelType {
		Undefined,
		MapNavigation,
		Combat
	}
	
	public class PanelWindow : Roga2dNode {
		public event WindowMessageEvent MessageEvent;
		private Ally player;
		private bool isPressed;
		private Collider pressedCollider;
		private PanelType selectedPanelType;
		private Roga2dNode selectedNode;

		public PanelWindow()
		{
			this.setPanel(PanelType.MapNavigation);
		}
		
		private void setPanel(PanelType panelType) {
			// Don't do anything if it is trying to switch to the same panel
			if (this.selectedPanelType == panelType) {
				return;	
			}
			
			// Remove selected panel in order to switch to a new panel
			if (selectedNode != null) {
				this.RemoveChild(selectedNode);
				selectedNode.Destroy();
			}

			// Construct new panel component and add it to the scene
			Roga2dNode node = null;
			switch (panelType) {
			case PanelType.Combat:
				node = new CombatControlPanel(this.OnTouched);
				break;
			case PanelType.MapNavigation:
				node = new MapNavigator(this.OnTouched);
				break;
			}
			this.AddChild(node);
			// Adjugst priority and positions
			node.LocalPriority = -0.2f;
			node.LocalPixelPosition = new Vector2(-80, 20);
			this.selectedNode = node;
			this.selectedPanelType = panelType;
		}
		
		public void OnTouched(Roga2dButton button) {
			if (MessageEvent != null) {
				PanelWindowMessage message = null;
				switch (this.selectedPanelType) {
				case PanelType.Combat:
					message = new PanelWindowMessage(WindowMessageType.CombatCardTouched, button.Tag);
					break;
				case PanelType.MapNavigation:
					//message = new PanelWindowMessage(WindowMessageType.FloorSymbolTouched, button.Tag);
					message = new PanelWindowMessage(WindowMessageType.StartCombat, button.Tag);
					break;
				}
				
				if (message != null) {
					MessageEvent(message);;	
				}
			}
		}
		
		public void ReceiveMessage(PanelWindowMessage message) 
		{
			switch (message.Type) {
			case WindowMessageType.StartCombat:
				this.setPanel(PanelType.Combat);
				break;
			case WindowMessageType.CombatCardTouched:
				break;	
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