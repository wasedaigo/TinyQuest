using UnityEngine;
using System.Collections;
using TinyQuest.Model;
using TinyQuest.Factory.Model;

public class ZoneSceneData : MonoBehaviour {
	
	private ZoneModel zoneModel;
	public ZoneModel ZoneModel {
		get { return this.zoneModel;}
	}
	
	private BattlerModel userBattlerModel;
	public BattlerModel UserBattlerModel {
		get { return this.userBattlerModel;}
	}
	
	void Awake () {
		this.zoneModel = ZoneFactory.Instance.Build();
		this.userBattlerModel = this.zoneModel.GetPlayerBattler();
	}
}
