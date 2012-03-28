using UnityEngine;
	using System.Collections;
using TinyQuest.Cache;
using TinyQuest;
using TinyQuest.Core;
using TinyQuest.Component;
using TinyQuest.Component.Window;
using TinyQuest.Model;

public class DualScreen : Roga2dNode{
	private Vector2 InvalidTouchPosition = new Vector2(-1000, -1000);
	private Rect TopPanelRect = new Rect(0, 100, 160, 140);
	private Rect BottomPanelRect = new Rect(0, 0, 160, 100);
	
	private bool isPressed;
	private Vector2 lastTouchedPosition;
	private Collider pressedCollider;
	
	private BaseComponent topWindow;
	private BaseComponent bottomWindow;
	private Camera BottomScreenCamera;
	private Camera TopScreenCamera;

	// Use this for initialization
	public DualScreen() {
		MapModel mapModel = MapCache.GetInstance().GetModel();
		
		this.topWindow = new TopWindow(mapModel);
		this.topWindow.LocalPosition = new Vector2(0, -1.5f);
		
		this.bottomWindow = new BottomWindow(mapModel);
		
		Roga2dNode frame = new Roga2dNode();
		this.AddChild(frame);
		
		// Set up Frame
		Roga2dSprite mainWindow = new Roga2dSprite("UI/frame", new Vector2(160, 240), new Vector2(0, 0), new Rect(0, 0, 160, 240));
		frame.AddChild(mainWindow);

		// Connect PreviewWindow and ControlWindow
		this.bottomWindow.MessageEvent += this.bottomWindow.ReceiveMessage;
		this.bottomWindow.MessageEvent += this.topWindow.ReceiveMessage;

		this.topWindow.MessageEvent += this.bottomWindow.ReceiveMessage;
		this.topWindow.MessageEvent += this.topWindow.ReceiveMessage;
		
		this.lastTouchedPosition = InvalidTouchPosition;
	}
	
	public void SetTopScreen(GameObject topScreen, Camera topScreenCamera) {
		Roga2dGameObjectState state = Roga2dUtils.stashState(this.topWindow.Transform);
		this.topWindow.Transform.parent = topScreen.transform;
		Roga2dUtils.applyState(this.topWindow.Transform, state);
		this.TopScreenCamera = topScreenCamera;
	}

	public void SetBottomScreen(GameObject bottomScreen, Camera bottomScreenCamera) {
		Roga2dGameObjectState state = Roga2dUtils.stashState(this.bottomWindow.Transform);
		this.bottomWindow.Transform.parent = bottomScreen.transform;
		Roga2dUtils.applyState(this.bottomWindow.Transform, state);
		this.BottomScreenCamera = bottomScreenCamera;
	}
	
	public override void Update ()
	{
		base.Update();
		this.bottomWindow.Update();
		this.topWindow.Update();
		
		// Construct a ray from the current mouse coordinates
		if (Input.GetMouseButtonDown(0)) {
			this.isPressed = true;
			Ray ray = this.BottomScreenCamera.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitInfo = new RaycastHit();
			if (Physics.Raycast(ray, out hitInfo)) {
				this.pressedCollider = hitInfo.collider;

				if (this.pressedCollider != null) {
					object obj = hitInfo.point;
					this.pressedCollider.SendMessage("ReceiveTouchDown", obj);
				}
			}
		}
		
		if (this.isPressed) {
			float logicalInputX = Input.mousePosition.x * Config.ActualLogicalRatio;
			float logicalInputY = Input.mousePosition.y * Config.ActualLogicalRatio;
			
			if (this.lastTouchedPosition == InvalidTouchPosition) {
				this.lastTouchedPosition = new Vector2(logicalInputX, logicalInputY);	
			}
			
			Vector2 delta = new Vector2(
					logicalInputX - this.lastTouchedPosition.x,
					-logicalInputY + this.lastTouchedPosition.y
			);
			
			if (this.TopPanelRect.Contains(this.lastTouchedPosition)) {
				this.topWindow.OnTouchMoved(delta);
			}

			if (this.BottomPanelRect.Contains(this.lastTouchedPosition)) {
				this.bottomWindow.OnTouchMoved(delta);
			}
			this.lastTouchedPosition.x = logicalInputX;
			this.lastTouchedPosition.y = logicalInputY;
		} else {
			this.lastTouchedPosition = InvalidTouchPosition;
		}
		
		if (Input.GetMouseButtonUp(0)) {	
			this.isPressed = false;
			if (this.pressedCollider != null) {
				this.pressedCollider.SendMessage("ReceiveTouchUp");
				this.pressedCollider = null;
			}
		}
	}
}
