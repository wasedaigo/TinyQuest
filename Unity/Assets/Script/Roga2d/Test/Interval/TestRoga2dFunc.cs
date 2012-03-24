using UnityEngine;
using System.Collections.Generic;

class TestRoga2dFunc {
	
	public static void Test() {
		TestFunc();
		TestFuncSequence();
	}
	
	private static void callback() {
		i += 1;
	}
	
	static int i = 0;
	public static void TestFunc () {
		Roga2dFunc interval = new Roga2dFunc(callback);
		
		i = 0;
		Tester.Ok(!interval.IsDone());
        
        interval.Update(1.0f);
		Tester.Match(i, 1);
		Tester.Ok(interval.IsDone());
        
        interval.Update(1.0f);
		Tester.Match(i, 1);
		Tester.Ok(interval.IsDone());
	}
	
	public static void TestFuncSequence () {
		i = 0;
		List<Roga2dBaseInterval> intervals = new List<Roga2dBaseInterval>() {
			new Roga2dFunc(callback),
			new Roga2dFunc(callback),
			new Roga2dFunc(callback)
		};

		Roga2dSequence interval = new Roga2dSequence(intervals);
		
        interval.Update(1.0f);
		Tester.Match(i, 3);
		Tester.Ok(interval.IsDone());
	}
}