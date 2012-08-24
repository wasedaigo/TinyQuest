using UnityEngine;

using TinyQuest.Data;
using TinyQuest.Core;
using TinyQuest.Object;

public class SkillButtonView : MonoBehaviour {
	public UILabel nameLabel;
	public UISprite lifeBarImage;
	public GameObject faceIcon;
	public GameObject infoPanel;
	public UISprite background;
	
	private Roga2dNode faceIconNode;
	private int life;
	private int revealedId;
	private bool isRevealed;
	
	public void Start() {
		this.revealedId = -1;
	}

	public void SetFaceIcon(int puppetId) {
		if (this.revealedId == puppetId) {
			return;	
		}
		
		this.revealedId = puppetId;
		if (this.faceIconNode != null) {
			this.faceIconNode.Destroy();
			this.faceIconNode = null;
		}

		FaceIcon actor = new FaceIcon(puppetId);
		actor.Transform.parent = faceIcon.transform;
		actor.LocalPosition = new Vector2(0, 0);
		actor.LocalRotation = 180.0f;
		actor.LocalScale = new Vector2(-24, 24);
		
		Utils.SetLayerRecursively(actor.Transform, 5);
		this.faceIconNode = actor;
	}

	public void SetName(string name) {
		nameLabel.text = name;
	}
	
	public void SetTouchEnabled(bool enabled) {
		if (IsDead()) {return;}
		collider.enabled = enabled;
	}
	
	private bool IsDead() {
		return this.life <= 0;	
	}
	
	public void SetLife(int life, int maxLife) {
		
		if (life < this.life) {
			iTween.ShakePosition(this.gameObject, iTween.Hash("x", 0.02f, "y", 0.02f,"time", 0.2f));
			if (this.faceIconNode != null) {
				Roga2dBaseInterval interval = EffectBuilder.GetInstance().BuildDamageInterval(this.faceIconNode);
				Roga2dIntervalPlayer.Instance.Play(interval);
			}
		}
		this.life = life;
		
		float ratio = life / (float)maxLife;
		
		
		lifeBarImage.transform.localScale = new Vector3(ratio, 1, 1);
		if (ratio > 0.5f) {
			lifeBarImage.color = new Color(0.4f, 0.7f, 0.3f);
		} else {
			if (ratio > 0.25f) {
				lifeBarImage.color = new Color(0.8f, 0.6f, 0.2f);
			} else {
				lifeBarImage.color = new Color(0.8f, 0.2f, 0.2f);
			}
		}
		
		this.SetDefaultColor();
	}
	
	public void SetDefaultColor() {
		if (this.IsDead()) {
			this.faceIconNode.LocalHue = new Roga2dHue(80, -80, -80);
		} else {
			if (this.isRevealed) {
				this.faceIconNode.LocalHue = new Roga2dHue(0, 0, 0);
			} else {
				this.faceIconNode.LocalHue = new Roga2dHue(-75, -75, -75);
			}
		}
	}
	
	public void Select() {
		this.isRevealed = true;
		this.SetDefaultColor();
	}
	
	public void Update() {
		if (this.faceIconNode != null) {
			this.faceIconNode.Update();	
		}
	}
	
	public void UpdateStatus(CombatUnit combatUnit) {
		bool show = (combatUnit.groupNo == 0 || combatUnit.revealed);
		if (show) {
			this.SetFaceIcon(combatUnit.GetUserUnit().Unit.id);
		} else {
			this.SetFaceIcon(0);
		}

		this.SetLife(combatUnit.hp, combatUnit.GetUserUnit().MaxHP);
	}
}