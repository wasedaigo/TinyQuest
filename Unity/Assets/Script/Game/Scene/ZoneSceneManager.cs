using UnityEngine;
using System.Collections;

public class ZoneSceneManager : MonoBehaviour {
	public GameObject UICombatPanel;
	public GameObject UIProgressPanel;
	
			// Use this for initialization
	void Start () {
		//this.gameObject.AddComponent<ZoneEventController>();
		//this.UIProgressPanel.SetActiveRecursively(true);
		
		this.gameObject.AddComponent<CombatController>();
		this.UICombatPanel.SetActiveRecursively(true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
