using UnityEngine;
using System.Collections.Generic;

class TestRoga2dParallel {
	
	public static void Test() {
		TestTween();
	}

	public static void TestTween () {
		Roga2dNode node = new Roga2dNode();

		Roga2dAlphaInterval interval1 = new Roga2dAlphaInterval(node, 0.1f, 1.0f, 3, true);
		Roga2dRotationIntervalOption option = new Roga2dRotationIntervalOption();
		Roga2dRotationInterval interval2 = new Roga2dRotationInterval(node, 0.0f, 180.0f, 5, true,  option);
		
		List<Roga2dBaseInterval> intervals = new List<Roga2dBaseInterval>();
		intervals.Add(interval1);
		intervals.Add(interval2);
		Roga2dParallel parallel = new Roga2dParallel(intervals);	
        
        parallel.Start();
		Tester.Match(node.LocalAlpha, 0.1f);
		Tester.Match(node.LocalRotation, 0.0f);
		Tester.Ok(!parallel.IsDone());
		
        parallel.Update();
		Tester.Match(node.LocalAlpha, 0.4f);
		Tester.Match(node.LocalRotation, 36.0f);
		Tester.Ok(!parallel.IsDone());
		
        parallel.Update();
		Tester.Match(node.LocalAlpha, 0.7f);
		Tester.Match(node.LocalRotation, 72.0f);
		Tester.Ok(!parallel.IsDone());
		
        parallel.Update();
		Tester.Match(node.LocalAlpha, 1.0f);
		Tester.Match(node.LocalRotation, 108.0f);
		Tester.Ok(!parallel.IsDone());
        
        parallel.Update();
		Tester.Match(node.LocalAlpha, 1.0f);
		Tester.Match(node.LocalRotation, 144.0f);
		Tester.Ok(!parallel.IsDone());

        parallel.Update();
		Tester.Match(node.LocalAlpha, 1.0f);
		Tester.Match(node.LocalRotation, 180.0f);
		Tester.Ok(parallel.IsDone());
        
        parallel.Update();
		Tester.Match(node.LocalAlpha, 1.0f);
		Tester.Match(node.LocalRotation, 180.0f);
		Tester.Ok(parallel.IsDone());
        
        parallel.Reset();
		Tester.Match(node.LocalAlpha, 0.1f);
		Tester.Match(node.LocalRotation, 0.0f);
		Tester.Ok(!parallel.IsDone());
		
		node.Destroy();
	}
}