using UnityEngine;
using System.Collections;
using TinyQuest.Scene;

public class Main : MonoBehaviour {
	public GameObject roga2dRoot;
	private Roga2dNode root;

	
	// Use this for initialization
	void Start () {
		this.root = new DungeonScene();
		this.root.LocalScale = new Vector2(2.0f, 2.0f);
		Roga2dGameObjectState state = Roga2dUtils.stashState(this.root.Transform);
		this.root.Transform.parent = roga2dRoot.transform;
		Roga2dUtils.applyState(this.root.Transform, state);
	}

	// Update is called once per frame
	void Update () {
		this.root.Update();
	}
}
