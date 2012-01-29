using UnityEngine;


class TestRoga2dRotationInterval {
	
	public static void Test() {
		TestTween();
	}
	
	public static void TestTween () {
		Roga2dNode node = new Roga2dNode();
		Roga2dRotationIntervalOption option = new Roga2dRotationIntervalOption();
		Roga2dRotationInterval interval = new Roga2dRotationInterval(node, 360.0f, 0.0f, 3, true, option);
		
		Tester.Ok(!interval.IsDone());
		
		Tester.Match(node.LocalRotation, 0.0f);
		Tester.Ok(!interval.IsDone());
        
        interval.Start();
		Tester.Match(node.LocalRotation, 0.0f); // 360.0f = 0.0f
		Tester.Ok(!interval.IsDone());
        
        interval.Update();
		Tester.Match(node.LocalRotation, 240.0f);
		Tester.Ok(!interval.IsDone());
        
        interval.Update();
		Tester.Match(node.LocalRotation, 120.0f);
		Tester.Ok(!interval.IsDone());
        
        interval.Update();
		Tester.Match(node.LocalRotation, 0.0f);
		Tester.Ok(interval.IsDone());

        interval.Reset();
		Tester.Match(node.LocalRotation, 0.0f); // 360.0f = 0.0f
		Tester.Ok(!interval.IsDone());
		
		node.Destroy();
	}
}