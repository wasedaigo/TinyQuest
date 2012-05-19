using UnityEngine;
using System.Collections;
using TinyQuest.Entity;
using TinyQuest.Factory.Entity;

public class ZoneSceneData : MonoBehaviour {
	
	private ZoneEntity zoneEntity;
	public ZoneEntity ZoneEntity {
		get { return this.zoneEntity;}
	}
	
	private BattlerEntity userBattlerEntity;
	public BattlerEntity UserBattlerEntity {
		get { return this.userBattlerEntity;}
	}
	
	void Awake () {
		this.zoneEntity = ZoneFactory.Instance.Build();
		this.userBattlerEntity = this.zoneEntity.GetPlayerBattler();
	}
}
