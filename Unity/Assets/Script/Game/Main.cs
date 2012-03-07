using UnityEngine;
using System.Collections;
using TinyQuest.Cache;
using TinyQuest.Component;
using TinyQuest.Component.Window;
using TinyQuest.Model;

public class Main : MonoBehaviour {
	public GameObject PreviewWindow;
	public GameObject ControlWindow;
	public GameObject MainWindow;
	private ControlWindow controlWindow;
	private AdventureWindow adventureWindow;
	private Roga2dSprite mainWindow;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;
		MapModel mapModel = MapCache.GetInstance().GetModel();

		this.controlWindow = new ControlWindow(mapModel);
		Roga2dGameObjectState state = Roga2dUtils.stashState(this.controlWindow.Transform);
		this.controlWindow.Transform.parent = ControlWindow.transform;
		Roga2dUtils.applyState(this.controlWindow.Transform, state);
		
		this.adventureWindow = new AdventureWindow(mapModel);
		state = Roga2dUtils.stashState(this.adventureWindow.Transform);
		this.adventureWindow.Transform.parent = PreviewWindow.transform;
		Roga2dUtils.applyState(this.adventureWindow.Transform, state);
		
		// Connect PreviewWindow and ControlWindow
		this.controlWindow.MessageEvent += this.controlWindow.ReceiveMessage;
		this.controlWindow.MessageEvent += this.adventureWindow.ReceiveMessage;

		this.adventureWindow.MessageEvent += this.controlWindow.ReceiveMessage;
		this.adventureWindow.MessageEvent += this.adventureWindow.ReceiveMessage;

		// BG
		this.mainWindow = new Roga2dSprite("UI/frame", new Vector2(160, 240), new Vector2(0, 0), new Rect(0, 0, 160, 240));
		state = Roga2dUtils.stashState(this.mainWindow.Transform);
		this.mainWindow.Transform.parent = MainWindow.transform;
		Roga2dUtils.applyState(this.mainWindow.Transform, state);
	}

	// Update is called once per frame
	void Update () {
		this.controlWindow.Update();
		this.adventureWindow.Update();
		this.mainWindow.Update();
	}
}
