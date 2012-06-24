using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Object;
using TinyQuest.Core;

public class AnimationSpawner : MonoBehaviour {
	public GameObject roga2dRoot;
	
	private Roga2dNode root;
	private Actor monster;
	private Roga2dAnimationPlayer player;
	private Roga2dIntervalPlayer intervalPlayer;
	private Roga2dBaseInterval targetInterval;

	private List<Actor> battlers = new List<Actor>();
	
	void Awake () {
		Application.targetFrameRate = 30;
		Time.captureFramerate = 30;
	}
	
	Actor spawnBattler (string name, float x, float y) {
		Actor battler = new PuppetActor(name, PuppetActor.State.Stand);
		battler.Sprite.LocalPriority = 0.1f;
		battler.LocalPixelPosition = new Vector2(x, y);
		this.battlers.Add(battler);
		return battler;
	}
	
	Actor spawnMonster (string name, float x, float y) {
		Actor battler = new MonsterActor(name);
		battler.Sprite.LocalPriority = 0.45f;
		battler.LocalPixelPosition = new Vector2(x, y);
		//this.battlers.Add(battler);
		return battler;
	}
	
	// Use this for initialization
	void Start () {
		Shader.WarmupAllShaders() ;

		this.player = new Roga2dAnimationPlayer();
		this.intervalPlayer = new Roga2dIntervalPlayer();
		this.root = new Roga2dNode("Root");
		this.root.LocalScale = new Vector2(2.0f, 2.0f);
		Roga2dGameObjectState state = Roga2dUtils.stashState(this.root.Transform);
		this.root.Transform.parent = roga2dRoot.transform;
		Roga2dUtils.applyState(this.root.Transform, state);

		// Player
		this.root.AddChild(spawnBattler("pierre", 40, -20));
		this.root.AddChild(spawnBattler("amon", 40, 20));
		this.root.AddChild(spawnBattler("draco", 80, -40));
		this.root.AddChild(spawnBattler("lilia", 80, 0));
		this.root.AddChild(spawnBattler("hose", 80, 40));
		
		// Monster
		this.monster = spawnMonster("death_wind", -40, 0);
		this.root.AddChild(this.monster);
	}
	
	
	static string[] ids = new string[] {
			"Battle/Skills/Fire/Skill_Flare",
			"Battle/Skills/Monster/DeadlyBite",
			"Battle/Skills/Bow/Shoot",
			"Battle/Skills/Sword/LeaveDance",
			
			"Battle/Skills/Spear/SpearAirraid",
			"Battle/Skills/Axe/Bash",
			"Battle/Skills/Laser/Skill_Laser01",
			"Battle/Skills/Bow/bow_bomb",
			"Battle/Skills/Axe/CycloneAxe",
			"Battle/Skills/Axe/Slash",
			"Battle/Skills/Axe/ArmorBreaker",
			
			"Battle/Skills/Common/MagicCasting"
	};
	
	static int no = 0;
    void AnimationFinished(Roga2dAnimation animation)
    {
		animation.settings.Origin.Show();
		animation.settings.Destroy();
		animation.settings = null;
    }
	
	void CommandCalled(Roga2dAnimationSettings settings, string command) 
	{
		string[] commandData = command.Split(':');
		if (commandData[0] == "damage") {
			// Flash effect
			Roga2dBaseInterval interval = EffectBuilder.GetInstance().BuildDamageInterval(settings.Target);
			this.intervalPlayer.Play(interval);
			
			// Damage pop
			Roga2dAnimation animation = EffectBuilder.GetInstance().BuildDamagePopAnimation(settings.Target.LocalPixelPosition, 2750);
			this.player.Play(settings.Root, null, animation, null);
		}
	}
	
	void Update () {
		
		this.intervalPlayer.Update();

		// Update animations
		this.player.Update(Time.deltaTime);
		this.root.Update();

		if (Input.GetMouseButtonDown(0)) {
			Actor battler = this.battlers[no % 5];
			if (battler.Sprite.IsVisible) {
				battler.Sprite.Hide();

				Dictionary<string, Roga2dSwapTextureDef> options = new Dictionary<string, Roga2dSwapTextureDef>() {
					{ "Battle/Skills/Battler_Base", new Roga2dSwapTextureDef() {TextureID = battler.TextureID, PixelSize = new Vector2(32, 32)}},
					{ "Battle/Skills/Monster_Base", new Roga2dSwapTextureDef() {TextureID = "death_wind", PixelSize = this.monster.PixelSize,  SrcRect = this.monster.SrcRect}}
				};

				Roga2dAnimationSettings settings = new Roga2dAnimationSettings(this.player, true, this.root, battler, this.monster, CommandCalled);
				Roga2dAnimation animation = Roga2dUtils.LoadAnimation(ids[no], false, battler.Sprite, settings, options);
				animation.Node.LocalPriority = 10.0f;
				this.player.Play(battler, null, animation,  AnimationFinished);
				no +=1;
				if (no >= ids.Length) {
					no = 0;	
				}
			}
		}
		
		// Change Camera position
		// camera.position = new Vector3(this.monster.Position.x, this.monster.Position.y, camera.position.z);
	}
}
