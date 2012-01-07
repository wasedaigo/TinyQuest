using UnityEngine;

class TestRoga2dRenderObject {
	
	public static void Test() {
		TestConstructor();
	}
	
	public static void TestConstructor() {
		Roga2dRenderObject renderObject = new Roga2dRenderObject(null, new Vector2(64, 64), new Vector2(32, 16), new Rect(0, 0, 1, 1));
		
		Tester.Match(renderObject.PixelSize, new Vector2(64, 64));
		Tester.Match(renderObject.PixelCenter, new Vector2(32, 16));
		Tester.Match(renderObject.SrcRect, new Rect(0, 0, 1, 1));

		renderObject.Destroy();
	}
}