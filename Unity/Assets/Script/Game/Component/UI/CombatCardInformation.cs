using UnityEngine;
using System.Collections;
using TinyQuest.Scene.Model;
using TinyQuest.Data.Skills;

public class CombatCardInformation : MonoBehaviour {
	public GameObject Panel;
	public UILabel Skill1Name;
	public UILabel Skill2Name;
	public UILabel Skill1Chance;
	public UILabel Skill2Chance;
	public UILabel FeatureName;
	private Vector3 origin;
	
	public void Start() {
		this.origin = this.Panel.transform.position;
		HideCardInfoPanel();
	}
	
	public struct ShowPanelParams {
		public readonly BaseSkill skill1;
		public readonly BaseSkill skill2;
		public readonly int feature;
		public ShowPanelParams(BaseSkill skill1, BaseSkill skill2, int feature) {
			this.skill1 = skill1;
			this.skill2 = skill2;
			this.feature = feature;
		}
	}
	
	private CombatModel combatModel;
	
	public void ShowCardInfoPanel(ShowPanelParams param) {
		this.Panel.transform.position = this.origin;
		Skill1Name.text = "";
		Skill2Name.text = "";
		Skill1Chance.text = "";
		Skill2Chance.text = "";
		FeatureName.text = "";

		if (param.skill1 != null) {
			Skill1Name.text = param.skill1.GetName();
			Skill1Chance.text = param.skill1.GetChance().ToString() + "%";
		}
		
		if (param.skill2 != null) {
			Skill2Name.text = param.skill1.GetName();
			Skill2Chance.text = param.skill1.GetChance().ToString() + "%";
		}

	}
	
	public void HideCardInfoPanel() {
		this.Panel.transform.position = new Vector3(1000, 1000, 1000);
	}
}
