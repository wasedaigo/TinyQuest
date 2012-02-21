using TinyQuest.Core;
using TinyQuest.Component.Panel;
using TinyQuest.Model;
using UnityEngine;

namespace TinyQuest.Component {
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
		private MapModel mapModel;

		public PanelWindow(MapModel mapModel)
		{
			this.mapModel = mapModel;
			this.mapModel.StepMoveStart += this.onStepMoveStart;
			this.setPanel(PanelType.MapNavigation);
		}
		
		private void onStepMoveStart() {
			Debug.Log("onStepMoveStart B");
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
				CombatControlPanel combatControlPanel = new CombatControlPanel();
				combatControlPanel.CardSelected += this.onCardSelected;
				node = combatControlPanel;
				break;
			case PanelType.MapNavigation:
				MapNavigatorPanel mapNavigator = new MapNavigatorPanel();
				mapNavigator.StepTouched += this.onStepTouched;
				node = mapNavigator;
				break;
			}
			this.AddChild(node);
			// Adjugst priority and positions
			node.LocalPriority = -0.2f;
			node.LocalPixelPosition = new Vector2(-80, 20);
			this.selectedNode = node;
			this.selectedPanelType = panelType;
		}
		
		public void onStepTouched(int stepIndex) {
			this.mapModel.moveTo(stepIndex);
			if (stepIndex == 0) {
				WindowMessage message = new WindowMessage(WindowMessageType.StartCombat, stepIndex);
				MessageEvent(message);
			}
		}
		
		private void onCardSelected(int cardIndex) {
				WindowMessage message = new WindowMessage(WindowMessageType.CombatCardTouched, cardIndex);
				MessageEvent(message);
		}
		
		public void ReceiveMessage(WindowMessage message) 
		{
			switch (message.Type) {
			case WindowMessageType.StartCombat:
				this.setPanel(PanelType.Combat);
				break;
			case WindowMessageType.FinishCombat:
				this.setPanel(PanelType.MapNavigation);
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