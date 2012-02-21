using UnityEngine;
using System.Collections;
using TinyQuest.Cache;
using TinyQuest.Component;
using TinyQuest.Component.Window;
using TinyQuest.Model;

public class Main : MonoBehaviour {
	public GameObject AdventureWindow;
	public GameObject PanelWindow;
	public GameObject MainWindow;
	private PanelWindow panelWindow;
	private AdventureWindow adventureWindow;
	private Roga2dSprite mainWindow;
	
	// Use this for initialization
	void Start () {
		MapModel mapModel = MapCache.GetInstance().GetModel();

		this.panelWindow = new PanelWindow(mapModel);
		Roga2dGameObjectState state = Roga2dUtils.stashState(this.panelWindow.Transform);
		this.panelWindow.Transform.parent = PanelWindow.transform;
		Roga2dUtils.applyState(this.panelWindow.Transform, state);
		
		this.adventureWindow = new AdventureWindow(mapModel);
		state = Roga2dUtils.stashState(this.adventureWindow.Transform);
		this.adventureWindow.Transform.parent = AdventureWindow.transform;
		Roga2dUtils.applyState(this.adventureWindow.Transform, state);
		
		// Connect AdventureWindow and PanelWindow
		this.panelWindow.MessageEvent += this.panelWindow.ReceiveMessage;
		this.panelWindow.MessageEvent += this.adventureWindow.ReceiveMessage;

		this.adventureWindow.MessageEvent += this.panelWindow.ReceiveMessage;
		this.adventureWindow.MessageEvent += this.adventureWindow.ReceiveMessage;

		// BG
		Roga2dRenderObject renderObject = new Roga2dRenderObject("UI/frame", new Vector2(160, 240), new Vector2(0, 0), new Rect(0, 0, 160, 240));
		this.mainWindow = new Roga2dSprite(renderObject);
		state = Roga2dUtils.stashState(this.mainWindow.Transform);
		this.mainWindow.Transform.parent = MainWindow.transform;
		Roga2dUtils.applyState(this.mainWindow.Transform, state);
	}

	// Update is called once per frame
	void Update () {
		this.panelWindow.Update();
		this.adventureWindow.Update();
		this.mainWindow.Update();
	}
}
