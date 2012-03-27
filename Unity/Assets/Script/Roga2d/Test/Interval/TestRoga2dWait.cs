using UnityEngine;

class TestRoga2dWait {
	
	public static void Test() {
		TestTween();
		TestTweenDelta();
	}
	
	public static void TestTween () {
		Roga2dWait interval = new Roga2dWait(5);
		
		Tester.Ok(!interval.IsDone());
        
        interval.Update(1.0f);
		Tester.Ok(!interval.IsDone());
        
        interval.Update(1.0f);
		Tester.Ok(!interval.IsDone());
        
        interval.Update(1.0f);
		Tester.Ok(!interval.IsDone());
        
        interval.Update(1.0f);
		Tester.Ok(!interval.IsDone());

        interval.Update(1.0f);
		Tester.Ok(interval.IsDone());
        
        interval.Reset();
		Tester.Ok(!interval.IsDone());
	}

	public static void TestTweenDelta() {
		Roga2dWait interval = new Roga2dWait(2);
		
		Tester.Ok(!interval.IsDone());
        
        interval.Update(0.4f);
		Tester.Match(interval.ExcessTime(), -1.6f);
		Tester.Ok(!interval.IsDone());
        
        interval.Update(1.0f);
		Tester.Match(interval.ExcessTime(), -0.6f);
		Tester.Ok(!interval.IsDone());
        
        interval.Update(1.0f);
		Tester.Match(interval.ExcessTime(), 0.4f);
		Tester.Ok(interval.IsDone());
		
        interval.Reset();
		Tester.Match(interval.ExcessTime(), -2.0f);
		Tester.Ok(!interval.IsDone());
	}
}