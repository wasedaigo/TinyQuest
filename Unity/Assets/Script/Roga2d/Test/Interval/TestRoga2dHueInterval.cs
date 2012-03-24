using UnityEngine;

class TestRoga2dHueInterval {
	
	public static void Test() {
		TestTween();
	}
	
	public static void TestTween () {
		Roga2dNode node = new Roga2dNode();

		Roga2dHue start = new Roga2dHue(10, 10, 10);
		Roga2dHue end = new Roga2dHue(2, 6, -10);
		Roga2dHueInterval interval = new Roga2dHueInterval(node, start, end, 4, true);
		
		Tester.Ok(!interval.IsDone());
		
		Tester.Match(node.LocalHue, new Roga2dHue(0, 0, 0));
		Tester.Ok(!interval.IsDone());
        
        interval.Start();
		Tester.Match(node.LocalHue, new Roga2dHue(10, 10, 10));
		Tester.Ok(!interval.IsDone());

        interval.Update(1.0f);
		Tester.Match(node.LocalHue, new Roga2dHue(8, 9, 5));
		Tester.Ok(!interval.IsDone());
        
        interval.Update(1.0f);
		Tester.Match(node.LocalHue, new Roga2dHue(6, 8, 0));
		Tester.Ok(!interval.IsDone());
        
        interval.Update(1.0f);
		Tester.Match(node.LocalHue, new Roga2dHue(4, 7, -5));
		Tester.Ok(!interval.IsDone());

        interval.Update(1.0f);
		Tester.Match(node.LocalHue, new Roga2dHue(2, 6, -10));
		Tester.Ok(interval.IsDone());
        
        interval.Reset();
		Tester.Match(node.LocalHue, new Roga2dHue(10, 10, 10));
		Tester.Ok(!interval.IsDone());
		
		node.Destroy();
	}
}