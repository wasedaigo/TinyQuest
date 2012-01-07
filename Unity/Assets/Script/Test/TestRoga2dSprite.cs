using UnityEngine;

class TestRoga2dSprite {
	
	public static void Test() {
		TestConstructor();
		TestConstructorNoRenderObject();
		TestAddRemove();
		TestPositionType();
	}
	
	public static void TestConstructor() {
		Roga2dRenderObject renderObject = new Roga2dRenderObject(null, new Vector2(64, 64), new Vector2(32, 16), new Rect(0, 0, 1, 1));
		Roga2dSprite sprite = new Roga2dSprite(renderObject);
		
		Tester.Match(renderObject.PixelSize, new Vector2(64, 64));
		Tester.Match(renderObject.PixelCenter, new Vector2(32, 16));
		Tester.Match(renderObject.SrcRect, new Rect(0, 0, 1, 1));
		
		sprite.LocalPosition = new Vector2(1.0f, 2.0f);
		sprite.LocalRotation = 3.0f;
		sprite.LocalScale = new Vector2(-1.0f, -2.0f);

		// Check TextureObject
		Tester.Match(renderObject.GameObject.transform.localPosition, new Vector3(1.0f, 0.5f, 0.0f));
		
		// Check Sprite Root
		sprite.Update();
		Tester.Match(sprite.GameObject.transform.localPosition, new Vector3(1.0f, 2.0f, 0.5f));
		Tester.Match(sprite.GameObject.transform.localEulerAngles, new Vector3(0.0f, 0.0f, 3.0f));
		Tester.Match(sprite.GameObject.transform.localScale, new Vector3(-1.0f, -2.0f, 1.0f));
		
		// Check TextureObject
		Tester.Match(renderObject.GameObject.transform.localPosition, new Vector3(1.0f, 0.5f, 0.0f));
		
		sprite.Destroy();
	}
	
	public static void TestConstructorNoRenderObject() {
		Roga2dSprite sprite = new Roga2dSprite(null);
		
		sprite.Update();
		Tester.Match(sprite.GetOffsetByPositionAnchor(0, 0), new Vector2(0, 0));

		sprite.Destroy();
	}
	
	public static void TestAddRemove() {
		Roga2dNode node = new Roga2dNode();
		Roga2dRenderObject renderObject = new Roga2dRenderObject(null, new Vector2(64, 64), new Vector2(32, 16), new Rect(0, 0, 1, 1));
		Roga2dSprite sprite = new Roga2dSprite(renderObject);
		
		Tester.Match(node.ChildrenCount, 0);
		
		node.AddChild(sprite);
		Tester.Match(node.ChildrenCount, 1);
		node.Update();

		node.RemoveAllChildren();
		Tester.Match(node.ChildrenCount, 0);

		node.Destroy();
	}
	
	public static void TestPositionType() {
		Roga2dRenderObject renderObject = new Roga2dRenderObject(null, new Vector2(30, 50), new Vector2(-5, 5), new Rect(5, 14, 25, 30));
		Roga2dSprite sprite = new Roga2dSprite(renderObject);

        sprite.LocalPosition = new Vector2(0, 10);
        sprite.LocalScale = new Vector2(2, 3);
		
		Tester.Match(sprite.GetOffsetByPositionAnchor(-1, 1), new Vector2(-40, 15));
		Tester.Match(sprite.GetOffsetByPositionAnchor(0, 1), new Vector2(-10, 15));
		Tester.Match(sprite.GetOffsetByPositionAnchor(1, 1), new Vector2(20, 15));
		Tester.Match(sprite.GetOffsetByPositionAnchor(-1, 0), new Vector2(-40, -60));
		Tester.Match(sprite.GetOffsetByPositionAnchor(0, 0), new Vector2(-10, -60));
		Tester.Match(sprite.GetOffsetByPositionAnchor(1, 0), new Vector2(20, -60));
		Tester.Match(sprite.GetOffsetByPositionAnchor(-1, -1), new Vector2(-40, -135));
		Tester.Match(sprite.GetOffsetByPositionAnchor(0, -1), new Vector2(-10, -135));
		Tester.Match(sprite.GetOffsetByPositionAnchor(1, -1), new Vector2(20, -135));

		sprite.Destroy();
	}
}