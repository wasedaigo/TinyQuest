using UnityEngine;


class TestRoga2dPositionInterval {
	public static void Test() {
		TestTween();
	}
	
	public static void TestTween () {
		Roga2dNode node = new Roga2dNode();

		Vector2 start = new Vector2(10.0f, 10.0f);
		Vector2 end = new Vector2(2.0f, 6.0f);
		Roga2dPositionIntervalOption option = Roga2dPositionIntervalOption.Build();
		Roga2dPositionInterval interval = new Roga2dPositionInterval(node, start, end, 4, true, option);
		
		Tester.Ok(!interval.IsDone());
		
		Tester.Match(node.LocalPosition, new Vector2(0.0f, 0.0f));
		Tester.Ok(!interval.IsDone());
        
        interval.Start();
		Tester.Match(node.LocalPosition, new Vector2(10.0f, 10.0f));
		Tester.Ok(!interval.IsDone());
        
        interval.Update();
		Tester.Match(node.LocalPosition, new Vector2(8.0f, 9.0f));
		Tester.Ok(!interval.IsDone());
        
        interval.Update();
		Tester.Match(node.LocalPosition, new Vector2(6.0f, 8.0f));
		Tester.Ok(!interval.IsDone());
        
        interval.Update();
		Tester.Match(node.LocalPosition, new Vector2(4.0f, 7.0f));
		Tester.Ok(!interval.IsDone());

        interval.Update();
		Tester.Match(node.LocalPosition, new Vector2(2.0f, 6.0f));
		Tester.Ok(interval.IsDone());
        
        interval.Reset();
		Tester.Match(node.LocalPosition, new Vector2(10.0f, 10.0f));
		Tester.Ok(!interval.IsDone());
		
		node.Destroy();
	}
}