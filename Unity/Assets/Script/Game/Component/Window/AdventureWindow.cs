using TinyQuest.Component;
using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Core;
using TinyQuest.Model;

namespace TinyQuest.Component.Window {
	public class AdventureWindow : Roga2dNode {
		public event WindowMessageEvent MessageEvent;
		
		private Roga2dNode root;
		private Monster monster;
		private Roga2dAnimationPlayer animationPlayer;
		private Roga2dBaseInterval targetInterval;
		private Stage stage;
		private MapModel mapModel;
		private List<BaseObject> battlers = new List<BaseObject>();
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
			battler.Sprite.LocalPriority = 0.45f;
			battler.LocalPixelPosition = new Vector2(x, y);
			//this.battlers.Add(battler);
			return battler;
		}
		
		// Use this for initialization
		public AdventureWindow(MapModel mapModel) {
			Shader.WarmupAllShaders() ;
	
			this.mapModel = mapModel;
			this.animationPlayer = new Roga2dAnimationPlayer();
			this.root = new Roga2dNode("Root");
			this.root.LocalScale = new Vector2(2.0f, 2.0f);
			this.AddChild(this.root);
			
			// animationPlayer
			this.player = spawnBattler("hose", 40, 30);
			this.root.AddChild(this.player);
			
			// Stage
			this.stage = new Stage();
			this.stage.LocalPriority = 0.0f;
			this.root.AddChild(stage);
			
			this.mapModel.StepMoved += this.onStepMoved;
			this.stage.ScrollFinished += this.onScrollFinished;
		}

		private void onStepMoved(float posX, float posY, float duration) {
			this.player.startWalkingAnimation();
			this.stage.Scroll(duration);
		}
	
		private void onScrollFinished() {
			this.player.stopWalkingAnimation();
		}
		
		static string[] ids = new string[] {
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
				"Battle/Skills/Fire/Skill_Flare",
				"Battle/Skills/Common/MagicCasting"
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
				this.finishCombat();
			}
		}
	
		private void finishCombat() {
			if (MessageEvent != null) {
				WindowMessage message = null;
				message = new WindowMessage(WindowMessageType.FinishCombat, null);
				if (message != null) {
					MessageEvent(message);;	
				}
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
				
				BaseObject baseObject = (BaseObject)settings.Target;
				baseObject.ApplyDamage(damageValue);
			}
		}
		
		private void playNextAnimation(int no) {
			BaseObject battler = this.battlers[0];
			if (battler.Sprite.IsVisible) {
				battler.Sprite.Hide();

				Dictionary<string, Roga2dSwapTextureDef> options = new Dictionary<string, Roga2dSwapTextureDef>() {
					{ "Battle/Skills/Battler_Base", new Roga2dSwapTextureDef() {TextureID = battler.TextureID, PixelSize = new Vector2(32, 32)}},
					{ "Battle/Skills/Monster_Base", new Roga2dSwapTextureDef() {TextureID = "death_wind", PixelSize = this.monster.PixelSize,  SrcRect = this.monster.SrcRect}}
				};

				Roga2dAnimationSettings settings = new Roga2dAnimationSettings(this.animationPlayer, this.root, battler, this.monster, CommandCalled);
				Roga2dAnimation animation = Roga2dUtils.LoadAnimation(ids[no], false, 1.0f, 0.5f, settings, options);
				this.animationPlayer.Play(battler, null, animation,  AnimationFinished);
			}	
		}

		
		public override void Update () {
			base.Update();
			Roga2dIntervalPlayer.GetInstance().Update();
	
			// Update animations
			this.animationPlayer.Update();
			this.root.Update();
			
			// Change Camera position
			// camera.position = new Vector3(this.monster.Position.x, this.monster.Position.y, camera.position.z);
		}
		
		public void ReceiveMessage(WindowMessage message) 
		{
			switch (message.Type) {
			case WindowMessageType.StartCombat:
				this.monster = spawnMonster("death_wind", -20, 0);
				this.root.AddChild(this.monster);
				break;
			case WindowMessageType.CombatCardTouched:
				int no = (int)message.Data;
				this.playNextAnimation(no);
				break;	
			}
		}
	}
}