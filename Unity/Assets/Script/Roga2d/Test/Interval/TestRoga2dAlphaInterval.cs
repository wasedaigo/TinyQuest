using UnityEngine;


class TestRoga2dAlphaInterval {
	
	public static void Test() {
		TestTween();
	}
	
	public static void TestTween () {
		Roga2dNode node = new Roga2dNode();
		Roga2dAlphaInterval interval = new Roga2dAlphaInterval(node, 0.0f, 1.0f, 5, Roga2dTweenType.Linear);
		
		Tester.Ok(!interval.IsDone());
		
		interval.Start();
		Tester.Match(node.LocalAlpha, 0.0f);
		Tester.Ok(!interval.IsDone());
        
		interval.Update(1.0f);
		Tester.Match(node.LocalAlpha, 0.2f);
		Tester.Ok(!interval.IsDone());
        
        interval.Update(1.0f);
		Tester.Match(node.LocalAlpha, 0.4f);
		Tester.Ok(!interval.IsDone());
        
        interval.Update(1.0f);
		Tester.Match(node.LocalAlpha, 0.6f);
		Tester.Ok(!interval.IsDone());
        
        interval.Update(1.0f);
		Tester.Match(node.LocalAlpha, 0.8f);
		Tester.Ok(!interval.IsDone());
        
        interval.Update(1.0f);
		Tester.Match(node.LocalAlpha, 1.0f);
		Tester.Ok(interval.IsDone());
	
        interval.Update(1.0f);
		Tester.Match(node.LocalAlpha, 1.0f);
		Tester.Ok(interval.IsDone());
		
        interval.Reset();
		Tester.Match(node.LocalAlpha, 0.0f);
		Tester.Ok(!interval.IsDone());
		
		node.Destroy();
	}
}