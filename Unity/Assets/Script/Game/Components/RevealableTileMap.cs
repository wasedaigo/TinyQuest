using UnityEngine;

namespace TinyQuest.Component {
public class RevealableTileMap : Roga2dNode {

	public RevealableTileMap () {
		Roga2dTiledSprite map = new Roga2dTiledSprite("Dungeon/sample", 5, 5);
		map.LocalPriority = 0.05f;
		this.AddChild(map);
		
		// Setup Collider
		BoxCollider collider = this.GameObject.AddComponent("BoxCollider") as BoxCollider;
		collider.size = map.LocalScale;
			
		Roga2dTouchReceiver touchReceiver = this.GameObject.AddComponent("Roga2dTouchReceiver") as Roga2dTouchReceiver;
		touchReceiver.OnTouchDown = this.OnTouchDown;
	}
		
	private void OnTouchDown() {
		Debug.Log("RevealableTileMap touched");
	}
}
}
