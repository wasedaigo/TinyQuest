using UnityEngine;

using TinyQuest.Data;
using TinyQuest.Core;
using TinyQuest.Object;

public class SkillButtonView : MonoBehaviour {
	public UILabel nameLabel;
	public GameObject lifeBarImage;
	public GameObject faceIcon;
	public GameObject infoPanel;
	public GameObject deadSign;
	public UISprite background;
	
	private Roga2dNode faceIconNode;
	private bool initialized = false;
	private int life;
	
	public void Start() {
		this.deadSign.SetActiveRecursively(false);
	}

	public void SetFaceIcon(int puppetId, bool isOpponent) {
		
		if (this.faceIconNode != null) {
			this.faceIconNode.Destroy();
			this.faceIconNode = null;
		}
		
		FaceIcon actor = new FaceIcon(puppetId);
		actor.Transform.parent = faceIcon.transform;
		actor.Transform.localPosition = new Vector3(0, 0, -0.1f);
		actor.Transform.localEulerAngles = new Vector3(0, 0, 180);
		
		if (isOpponent) {
			actor.Transform.localScale = new Vector3(24, 24, 0);
		} else {
			actor.Transform.localScale = new Vector3(-24, 24, 0);
		}
		
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
		UISprite lifeBarImageSprite = lifeBarImage.GetComponent<UISprite>();
		
		if (life < this.life) {
			iTween.ShakePosition(this.gameObject, iTween.Hash("x", 0.02f, "y", 0.02f,"time", 0.2f));
		}
		this.life = life;
		
		// change color depends on remains
		float ratio = life / (float)maxLife;
		if (ratio > 0.5f) {
			lifeBarImageSprite.color = new Color(0.6f, 0.8f, 0.0f, 1.0f);
		} else {
			if (ratio > 0.25f) {
				lifeBarImageSprite.color = new Color(1.0f, 0.8f, 0.2f, 1.0f);
			} else {
				lifeBarImageSprite.color = new Color(1.0f, 0.2f, 0.2f, 1.0f);	
			}
		}
		
		lifeBarImage.transform.localScale = new Vector3(life / (float)maxLife, 1, 1);
		deadSign.SetActiveRecursively(IsDead());
	}
	
	public void UpdateStatus(CombatUnit combatUnit) {
		this.SetLife(combatUnit.hp, combatUnit.GetUserUnit().MaxHP);
		if (!initialized) {
			this.SetFaceIcon(combatUnit.GetUserUnit().Unit.id, combatUnit.groupNo == 1);
			this.initialized = true;
		}
	}
}