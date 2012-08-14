using UnityEngine;
using System.Collections;
using TinyQuest.Object;

public class PuppetButton : MonoBehaviour {
	
	public int puppetID;
	
	// Use this for initialization
	void Start () {
		PuppetActor actor = new PuppetActor(puppetID.ToString(), Actor.PoseType.Attack);
		actor.Transform.parent = this.gameObject.transform;

		actor.Transform.localPosition = new Vector3(0, 0, 0);
		actor.Transform.localEulerAngles = new Vector3(0, 0, -180);
		actor.Transform.localScale = new Vector3(-32, 32, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
