using UnityEngine;

namespace TinyQuest.Component {
public class RevealableTileMap : Roga2dNode {
	private Roga2dTiledSprite map;
	public RevealableTileMap () {
		this.map = new Roga2dTiledSprite("Dungeon/sample", 5, 5);
		this.map.LocalPriority = 0.05f;
		this.AddChild(this.map);
		
		// Setup Collider
		BoxCollider collider = this.GameObject.AddComponent("BoxCollider") as BoxCollider;
		collider.size = map.LocalScale;
			
		Roga2dTouchReceiver touchReceiver = this.GameObject.AddComponent("Roga2dTouchReceiver") as Roga2dTouchReceiver;
		touchReceiver.OnTouchDown = this.OnTouchDown;
	}
		
	public override void Destroy () {
		this.map.Destroy();
		base.Destroy();
	}
		
	private void OnTouchDown(Vector2 pos) {
		// Calculate local pixel position
		pos += this.map.LocalScale / 2;
		pos = Roga2dUtils.localToPixel(pos);
			
		// Get touched grid position
		int gridX = (int)(pos.x / this.map.GridWidth);
		int gridY = (int)(pos.y / this.map.GridHeight);
		this.map.SetTile(gridX, gridY, -1);	
	}
}
}
