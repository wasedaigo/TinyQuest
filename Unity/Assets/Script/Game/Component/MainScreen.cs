using TinyQuest.Component;
using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Core;
using TinyQuest.Model;

namespace TinyQuest.Component {
	public class MainScreen : Roga2dNode {
		
		private Roga2dNode root;
		private Monster monster;
		private Roga2dAnimationPlayer animationPlayer;
		private Roga2dBaseInterval targetInterval;
		private Stage stage;
		private List<AdventureObject> battlers = new List<AdventureObject>();
		private Ally player;
		
		Ally spawnBattler (string name, float x, float y) {
			Ally battler = new Ally(name);
			battler.Sprite.LocalPriority = 0.1f;
			battler.LocalPixelPosition = new Vector2(x, y);
			this.battlers.Add(battler);
			
			return battler;
		}
		
		Monster spawnMonster (string name, float x, float y) {
			Monster battler = new Monster(name);
			battler.LocalPriority = 0.45f;
			battler.LocalPixelPosition = new Vector2(x, y);
			battler.UpdatePriority();
			//this.battlers.Add(battler);
			return battler;
		}
		
		// Use this for initialization
		public MainScreen() {
			Shader.WarmupAllShaders() ;
	
			this.animationPlayer = new Roga2dAnimationPlayer();
			this.root = new Roga2dNode("Root");
			this.AddChild(this.root);
			
			// Stage
			this.stage = new Stage();
			this.stage.LocalPriority = 0.0f;
			this.root.AddChild(stage);

			// animationPlayer
			this.player = spawnBattler("fighter", 20, 0);
			this.stage.GetCharacterLayer().AddChild(this.player);
			
			this.stage.ScrollFinished += this.onScrollFinished;
			
			ActionWheel actionWheel = GameObject.Find("ActionWheel").GetComponent("ActionWheel") as ActionWheel;
			actionWheel.onSlotChanged = this.onSlotChanged;
			actionWheel.onSubButtonClicked = this.onSubButtonClicked;
			this.monster = spawnMonster("death_wind", -40, 0);
			this.stage.GetCharacterLayer().AddChild(this.monster);
		}
	
		private void onSlotChanged(int slotNo) {
			this.playNextAnimation(slotNo);
		}

		private void onSubButtonClicked() {
		}
		
		private void onScrollFinished() {
			this.player.stopWalkingAnimation();
		}
		
		static string[] ids = new string[] {
				"combat/sword_slash01",
				"combat/sword_slash01",
				"combat/sword_slash01",
				"combat/sword_slash01",
				"combat/sword_slash01",
				"combat/sword_slash01",
				"combat/sword_slash01",
				"combat/sword_slash01"
				//"Battle/Skills/Bow/Shoot",
				//"Battle/Skills/Sword/LeaveDance",
				//"Battle/Skills/Spear/SpearAirraid",
				//"Battle/Skills/Axe/Bash",
				//"Battle/Skills/Common/MagicCasting",
				//"Battle/Skills/Laser/Skill_Laser01",
				//"Battle/Skills/Bow/bow_bomb",
				//"Battle/Skills/Axe/CycloneAxe",
				//"Battle/Skills/Axe/Slash",
				//"Battle/Skills/Axe/ArmorBreaker",
				//"Battle/Skills/Fire/Skill_Flare",
				//"Battle/Skills/Monster/DeadlyBite",
		};
		
		static Rect[] weapons = new Rect[] {
				new Rect(32, 0, 32, 32),
				new Rect(32, 0, 32, 32),
				new Rect(32, 0, 32, 32),
				new Rect(32, 0, 32, 32),
				new Rect(0, 0, 32, 32),
				new Rect(0, 0, 32, 32),
				new Rect(0, 0, 32, 32),
				new Rect(0, 0, 32, 32),
				//"Battle/Skills/Bow/Shoot",
				//"Battle/Skills/Sword/LeaveDance",
				//"Battle/Skills/Spear/SpearAirraid",
				//"Battle/Skills/Axe/Bash",
				//"Battle/Skills/Common/MagicCasting",
				//"Battle/Skills/Laser/Skill_Laser01",
				//"Battle/Skills/Bow/bow_bomb",
				//"Battle/Skills/Axe/CycloneAxe",
				//"Battle/Skills/Axe/Slash",
				//"Battle/Skills/Axe/ArmorBreaker",
				//"Battle/Skills/Fire/Skill_Flare",
				//"Battle/Skills/Monster/DeadlyBite",
		};
		
	    private void AnimationFinished(Roga2dAnimation animation)
	    {
			animation.settings.Origin.Show();
			animation.settings.Destroy();
			animation.settings = null;
			
			this.checkDead();
	    }
		
		private void checkDead() {
			if (this.monster.IsDead()) {
				this.RemoveChild(this.monster);
				this.monster.Destroy();
			}
		}
		
		private void CommandCalled(Roga2dAnimationSettings settings, string command) 
		{
			string[] commandData = command.Split(':');
			if (commandData[0] == "damage") {
				uint damageValue = 2750;
				// Flash effect
				Roga2dBaseInterval interval = EffectBuilder.GetInstance().BuildDamageInterval(settings.Target);
				Roga2dIntervalPlayer.GetInstance().Play(interval);
				
				// Damage pop
				Roga2dAnimation animation = EffectBuilder.GetInstance().BuildDamagePopAnimation(settings.Target.LocalPixelPosition, damageValue);
				this.animationPlayer.Play(settings.Root, null, animation, null);
				
				//AdventureObject obj = (AdventureObject)settings.Target;
				//obj.ApplyDamage(damageValue);
			}
		}
		
		private void playNextAnimation(int no) {
			AdventureObject battler = this.battlers[0];
			if (battler.Sprite.IsVisible) {
				battler.Sprite.Hide();

				Dictionary<string, Roga2dSwapTextureDef> options = new Dictionary<string, Roga2dSwapTextureDef>() {
					{ "combat/battler_base", new Roga2dSwapTextureDef() {TextureID = battler.TextureID, PixelSize = new Vector2(32, 32)}},
					{ "Battle/Skills/Monster_Base", new Roga2dSwapTextureDef() {TextureID = "death_wind", PixelSize = this.monster.PixelSize,  SrcRect = this.monster.SrcRect}},
					{ "combat/weapon_sword_base", new Roga2dSwapTextureDef() {TextureID = "combat/weapon_sword_base", PixelSize = new Vector2(32, 32),  SrcRect = weapons[no]}}
				};

				Roga2dAnimationSettings settings = new Roga2dAnimationSettings(this.animationPlayer, this.stage.GetCharacterLayer(), battler, this.monster, CommandCalled);
				Roga2dAnimation animation = Roga2dUtils.LoadAnimation(ids[no], false, 1.0f, 0.5f, settings, options);
				this.animationPlayer.Play(battler, null, animation,  AnimationFinished);
			}	
		}

		
		public override void Update () {
			Roga2dIntervalPlayer.GetInstance().Update();
	
			// Update animations
			this.animationPlayer.Update(Time.deltaTime);
			base.Update();
			
			// Change Camera position
			// camera.position = new Vector3(this.monster.Position.x, this.monster.Position.y, camera.position.z);
		}
	}
}