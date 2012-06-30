using UnityEngine;

using TinyQuest.Data;
using TinyQuest.Core;

public class SkillButtonView : MonoBehaviour {
	public UILabel nameLabel;
	public UILabel lifeLabel;
	public GameObject faceIcon;
		
	private UITexture faceIconTexture;
	private bool initialized = false;
	
	public void SetFaceIcon(int puppetId) {
		string textureId = "UI/Icon/puppet/" + puppetId.ToString();
		
		if (this.faceIconTexture != null) {
			NGUITools.Destroy(this.faceIconTexture);
		}

		UITexture ut = NGUITools.AddWidget<UITexture>(faceIcon);
		Material material = Roga2dResourceManager.getSharedMaterial(textureId, Roga2dBlendType.Unlit);
	    ut.material = material;
		ut.MarkAsChanged();
		ut.MakePixelPerfect();
		ut.transform.localScale = new Vector3(1, 1, 1);
		ut.transform.localPosition = Vector3.zero;
		this.faceIconTexture = ut;
	}

	public void SetName(string name) {
		nameLabel.text = name;
	}
	
	public void SetLife(int life, int maxLife) {
		float ratio = life / (float)maxLife;
		ActorHealthState state = Utils.GetHealthState(ratio);
		switch (state) {
		case ActorHealthState.Full:
			lifeLabel.color = Color.blue;
			break;
		case ActorHealthState.Ok:
			lifeLabel.color = Color.white;
			break;
		case ActorHealthState.Dying:
			lifeLabel.color = Color.yellow;
			break;
		case ActorHealthState.Dead:
			lifeLabel.color = Color.red;
			break;
		}
		
		BoxCollider collider = this.gameObject.GetComponent<BoxCollider>();
		if (life <= 0) {
			collider.enabled = false;
		} else {
			collider.enabled = true;
		}
		lifeLabel.text = life.ToString();
	}
	
	public void UpdateStatus(CombatUnit combatUnit) {
		this.SetLife(combatUnit.GetUserUnit().hp, combatUnit.GetUserUnit().MaxHP);
		if (!initialized) {
			this.SetFaceIcon(combatUnit.GetUserUnit().Unit.id);
			this.initialized = true;
		}
	}
}