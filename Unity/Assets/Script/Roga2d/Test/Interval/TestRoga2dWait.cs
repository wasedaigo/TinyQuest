using UnityEngine;

class TestRoga2dWait {
	
	public static void Test() {
		TestTween();
	}
	
	public static void TestTween () {
		Roga2dWait interval = new Roga2dWait(5);
		
		Tester.Ok(!interval.IsDone());
        
        interval.Update();
		Tester.Ok(!interval.IsDone());
        
        interval.Update();
		Tester.Ok(!interval.IsDone());
        
        interval.Update();
		Tester.Ok(!interval.IsDone());
        
        interval.Update();
		Tester.Ok(!interval.IsDone());

        interval.Update();
		Tester.Ok(interval.IsDone());
        
        interval.Reset();
		Tester.Ok(!interval.IsDone());
	}
}