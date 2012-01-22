using UnityEngine;

class TestRoga2dNode {
	
	public static void Test() {
	
		TestSetterGetter();
		TestAddRemove();
		TestAddRemoveAll();
		TestUpdate();
		TestVisibility();
	}
	
	public static void TestAddRemoveAll() {
		Roga2dNode node1 = new Roga2dNode();
		Roga2dNode node2 = new Roga2dNode();
		
		Tester.Match(node1.ChildrenCount, 0);
		
		node1.AddChild(node2);
		Tester.Match(node1.ChildrenCount, 1);
		Tester.Match(node1, node2.Parent);
		
		node1.RemoveAllChildren();
		Tester.Match(node1.ChildrenCount, 0);
	
		node1.Destroy();
	}
	
	public static void TestAddRemove() {
		Roga2dNode node1 = new Roga2dNode();
		Roga2dNode node2 = new Roga2dNode();
		
		node1.LocalPosition = new Vector2(5, 5);
		node1.LocalRotation = 50.0f;
		node1.LocalScale = new Vector2(3, 2);
		
		node2.LocalPosition = new Vector2(10, 10);
		node2.LocalRotation = 100.0f;
		node2.LocalScale = new Vector2(5, 4);
		
		Tester.Match(node1.ChildrenCount, 0);

		node1.AddChild(node2);
		Tester.Match(node1.ChildrenCount, 1);
		Tester.Match(node1, node2.Parent);
		
		// Check parent node transform is as expected
		Tester.Match(node1.LocalPosition, new Vector2(5, 5));
		Tester.Match(node1.LocalRotation, 50.0f);
		Tester.Match(node1.LocalScale, new Vector2(3, 2));
		
		// Check child node transform is as expected
		Tester.Match(node2.LocalPosition, new Vector2(10, 10));
		Tester.Match(node2.LocalRotation, 100.0f);
		Tester.Match(node2.LocalScale, new Vector2(5, 4));
		
		node1.RemoveChild(node2);
		Tester.Match(node1.ChildrenCount, 0);
		
		node1.Destroy();
	}
	
	public static void TestSetterGetter() {
		Roga2dNode node = new Roga2dNode();
	
		// Scale
		node.LocalScale = new Vector2(1.0f, 1.0f);
		Tester.Match(node.LocalScale, new Vector2(1.0f, 1.0f));
		
		// Position
		node.LocalPosition = new Vector2(1.0f, 1.0f);
		Tester.Match(node.LocalPosition, new Vector2(1.0f, 1.0f));
		Tester.Match(node.Position, new Vector2(1.0f, 1.0f));
		
		// Rotation
		node.LocalRotation = 30.0f;
		Tester.Match(node.LocalRotation, 30.0f);
		
		// Alpha
		node.LocalAlpha = 0.5f;
		Tester.Match(node.LocalAlpha, 0.5f);
		
		// Hue
		node.LocalHue = new Roga2dHue(100, 100, 100);
		Tester.Match(node.LocalHue, new Roga2dHue(100, 100, 100));
		
		// Priority
		node.LocalPriority = 0.5f;
		Tester.Match(node.LocalPriority, 0.5f);
		
		// BlendType
		node.BlendType = Roga2dBlendType.Add;
		Tester.Match(node.BlendType, Roga2dBlendType.Add);
		
		node.Destroy();
	}
	
	public static void TestUpdate() {
		Roga2dNode node = new Roga2dNode();
		Roga2dNode child = new Roga2dNode();
	
		node.LocalAlpha = 0.3f;
		node.LocalPriority = 0.4f;
		node.LocalPosition = new Vector2(1.0f, 2.0f);
		node.LocalRotation = 3.0f;
		node.LocalScale = new Vector2(-1.0f, -2.0f);
		
		child.LocalAlpha = 0.3f;
		child.LocalPriority = 0.4f;
		node.AddChild(child);
		
		// Before transform
		Tester.Match(node.Alpha, 1.0f);
		Tester.Match(node.Priority, 0.5f);
		Tester.Match(child.Alpha, 1.0f);
		Tester.Match(child.Priority, 0.5f);

		node.Update();
		// After transform
		Tester.Match(node.Transform.localPosition, new Vector3(1.0f, 2.0f, 0.4f));
		Tester.Match(node.Transform.localEulerAngles, new Vector3(0.0f, 0.0f, 3.0f));
		Tester.Match(node.Transform.localScale, new Vector3(-1.0f, -2.0f, 1.0f));
		Tester.Match(node.Transform, child.Transform.parent);
		Tester.Match(node.Alpha, 0.3f);
		Tester.Match(node.Priority, 0.4f);
		Tester.Match(child.Alpha, 0.09f);
		Tester.Match(child.Priority, 0.32f);
		
		node.Destroy();
	}
	
	public static void TestVisibility() {
		Roga2dNode node1 = new Roga2dNode();
		Roga2dNode node2 = new Roga2dNode();
		
		node1.Hide();
		Tester.Match(node1.IsVisible, false);
		
		node1.AddChild(node2);
		Tester.Match(node2.IsVisible, false);
		
		node1.Hide();
		Tester.Match(node1.IsVisible, false);
		Tester.Match(node2.IsVisible, false);
		
		node1.Show();
		Tester.Match(node1.IsVisible, true);
		Tester.Match(node2.IsVisible, true);
		
		node1.Destroy();
		node2.Destroy();
	}
}