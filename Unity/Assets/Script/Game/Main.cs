using UnityEngine;
using System.Collections;
using TinyQuest.Cache;
using TinyQuest.Component;
using TinyQuest.Component.Window;
using TinyQuest.Model;

public class Main : MonoBehaviour {
	public GameObject Root;
	private DualScreen dualScreen;
	
	void Awake () {
		Application.targetFrameRate = 26;
	}
	
	// Use this for initialization
	void Start () {
		this.dualScreen = new DualScreen();
		Roga2dGameObjectState state = Roga2dUtils.stashState(this.dualScreen.Transform);
		this.dualScreen.Transform.parent = this.Root.transform;
		Roga2dUtils.applyState(this.dualScreen.Transform, state);
	}

	// Update is called once per frame
	public void Update () {
		this.dualScreen.Update();
	}
}
