using UnityEngine;

class TestRoga2dRoot {
	
	public static void Test() {
		TestConstructor();
	}
	
	public static void TestConstructor() {
		Roga2dNode target = new Roga2dNode();
		Roga2dAnimationPlayer player = new Roga2dAnimationPlayer();
		Roga2dRoot root = new Roga2dRoot(player);
		root.AddChild(target);
		Tester.Match(root.Player, player);
		Tester.Ok(root.Target == null);
		Tester.Ok(root.TargetOrigin == null);
		root.Target = target;
		Tester.Match(root.Target, target);
		Tester.Ok(root.TargetOrigin != null);
		root.Destroy();
		target.Destroy();
	}
}