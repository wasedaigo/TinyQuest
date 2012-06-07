
using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Core;
using TinyQuest.Object;

public class BaseStageController : MonoBehaviour {
	public  BaloonMessageBox baloonMessageBox;
	
	private Roga2dNode root;
	private Roga2dAnimationPlayer animationPlayer;
	private Roga2dIntervalPlayer intervalPlayer;
	private BaloonMessageBox visibleMessageBox;
	
		
	public Roga2dAnimationPlayer AnimationPlayer {
		get {return this.animationPlayer;}	
	}
	
	public Roga2dIntervalPlayer IntervalPlayer {
		get {return this.intervalPlayer;}	
	}
	
	private Stage stage;
	public Stage Stage {
		get {
			if (this.stage == null) {
				this.stage = this.GetComponent<Stage>();	
			}
			return this.stage;
		}	
	}
	
	protected Ally spawnBattler (string name, Ally.State state, float x, float y) {
		Ally battler = new Ally(name, state);
		battler.LocalPixelPosition = new Vector2(x, y);
		
		return battler;
	}

	protected Monster spawnMonster (string name, float x, float y) {
		Monster battler = new Monster(name);
		battler.LocalPriority = 0.45f;
		battler.LocalPixelPosition = new Vector2(x, y);
		battler.UpdatePriority();
		return battler;
	}
	
	// Use this for initialization
	protected virtual void Start() {
		Shader.WarmupAllShaders() ;
		this.animationPlayer = new Roga2dAnimationPlayer();
		this.intervalPlayer = new Roga2dIntervalPlayer();
		Application.targetFrameRate = 60;
	}
	
	public void ShowMessage(string message) {
		this.HideMessage();
		BaloonMessageBox box = (BaloonMessageBox)Instantiate(baloonMessageBox, new Vector3 (0, 0, 0), Quaternion.identity);
		box.transform.parent = this.Stage.gameObject.transform;
		box.transform.localScale = new Vector3(0.003f, 0.003f, 1);
		box.transform.localPosition = new Vector3(0, 0.45f, 0);
		box.ArrowFaceRight = true;
		box.Width = 256;
		box.Height = 64;
		box.Message = message;
		
		this.visibleMessageBox = box;
	}
	
	public void HideMessage() {
		if (this.visibleMessageBox != null) {
			this.visibleMessageBox.transform.parent = null;
			Destroy(this.visibleMessageBox.gameObject);
			this.visibleMessageBox = null;
		}
	}

	protected virtual void Update () {
		this.animationPlayer.Update(Time.deltaTime);
		this.intervalPlayer.Update();
	}
}