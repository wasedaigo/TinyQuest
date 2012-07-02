using UnityEngine;
using System.Collections.Generic;

class TestRoga2dLoop {
	
	public static void Test() {
		Test2Loop();
		TestInfiniteLoop();
		TestLongFrameLoop();
	}
	
	public static void Test2Loop() {
		Roga2dNode node = new Roga2dNode();

		Roga2dAlphaInterval interval1 = new Roga2dAlphaInterval(node, 0.1f, 1.0f, 1, Roga2dTweenType.Linear);
		Roga2dWait interval2 = new Roga2dWait(1);
		Roga2dAlphaInterval interval3 = new Roga2dAlphaInterval(node, 0.7f, 0.0f, 1, Roga2dTweenType.Linear);

		List<Roga2dBaseInterval> intervals = new List<Roga2dBaseInterval>();
		intervals.Add(interval1);
		intervals.Add(interval2);
		intervals.Add(interval3);
		
		Roga2dSequence sequence = new Roga2dSequence(intervals);
		Roga2dLoop loop = new Roga2dLoop(sequence, 2);
		
		loop.Start();
		Tester.Match(node.LocalAlpha, 0.1f);
		Tester.Match(loop.ExcessTime(), -1);
		Tester.Ok(!loop.IsDone());
		
		loop.Update(1.5f);
		Tester.Match(node.LocalAlpha, 1.0f);
		Tester.Match(loop.ExcessTime(), -1);
		Tester.Ok(!loop.IsDone());

		loop.Update(0.5f);
		Tester.Match(node.LocalAlpha, 1.0f);
		Tester.Match(loop.ExcessTime(), -1);
		Tester.Ok(!loop.IsDone());
		
		loop.Update(1.0f);
		Tester.Match(node.LocalAlpha, 0.1f);
		Tester.Match(loop.ExcessTime(), -1);
		Tester.Ok(!loop.IsDone());
		
		loop.Update(1.0f);
		Tester.Match(node.LocalAlpha, 1.0f);
		Tester.Match(loop.ExcessTime(), -1);
		Tester.Ok(!loop.IsDone());
		
		loop.Update(1.0f);
		Tester.Match(node.LocalAlpha, 1.0f);
		Tester.Match(loop.ExcessTime(), -1);
		Tester.Ok(!loop.IsDone());
			
		loop.Update(1.0f);
		Tester.Match(node.LocalAlpha, 0.0f);
		Tester.Match(loop.ExcessTime(), 0);
		Tester.Ok(loop.IsDone());
		
		node.Destroy();
	}
	
	public static void TestInfiniteLoop() {
		Roga2dNode node = new Roga2dNode();

		Roga2dAlphaInterval interval1 = new Roga2dAlphaInterval(node, 0.1f, 1.0f, 1, Roga2dTweenType.Linear);
		Roga2dWait interval2 = new Roga2dWait(1);
		Roga2dAlphaInterval interval3 = new Roga2dAlphaInterval(node, 0.7f, 0.0f, 1, Roga2dTweenType.Linear);

		List<Roga2dBaseInterval> intervals = new List<Roga2dBaseInterval>();
		intervals.Add(interval1);
		intervals.Add(interval2);
		intervals.Add(interval3);
		
		Roga2dSequence sequence = new Roga2dSequence(intervals);
		Roga2dLoop loop = new Roga2dLoop(sequence, 0);
		
		loop.Start();
		Tester.Match(node.LocalAlpha, 0.1f);
		Tester.Ok(!loop.IsDone());
           
        for (int i = 0; i < 4; i++) {
            loop.Update(1.0f);
			Tester.Match(node.LocalAlpha, 1.0f);
			Tester.Match(loop.ExcessTime(), -1);
            Tester.Ok(!loop.IsDone());

            loop.Update(1.0f);
			Tester.Match(node.LocalAlpha, 1.0f);
			Tester.Match(loop.ExcessTime(), -1);
            Tester.Ok(!loop.IsDone());

            loop.Update(1.0f);
			Tester.Match(node.LocalAlpha, 0.1f);
			Tester.Match(loop.ExcessTime(), -1);
            Tester.Ok(!loop.IsDone());
        }   
	
		node.Destroy();
	}
	
	public static void TestLongFrameLoop() {
		Roga2dNode node = new Roga2dNode();

		Roga2dAlphaInterval interval1 = new Roga2dAlphaInterval(node, 0.1f, 1.0f, 1, Roga2dTweenType.Linear);
		Roga2dWait interval2 = new Roga2dWait(1);
		Roga2dAlphaInterval interval3 = new Roga2dAlphaInterval(node, 0.7f, 0.0f, 1, Roga2dTweenType.Linear);

		List<Roga2dBaseInterval> intervals = new List<Roga2dBaseInterval>();
		intervals.Add(interval1);
		intervals.Add(interval2);
		intervals.Add(interval3);
		
		Roga2dSequence sequence = new Roga2dSequence(intervals);
		Roga2dLoop loop = new Roga2dLoop(sequence, 2);
		
		loop.Start();
		Tester.Match(node.LocalAlpha, 0.1f);
		Tester.Match(loop.ExcessTime(), -1);
		Tester.Ok(!loop.IsDone());
		
		loop.Update(3.0f);
		Tester.Match(node.LocalAlpha, 0.1f);
		Tester.Match(loop.ExcessTime(), -1);
		Tester.Ok(!loop.IsDone());
		
		loop.Update(2.0f);
		Tester.Match(node.LocalAlpha, 1.0f);
		Tester.Match(loop.ExcessTime(), -1);
		Tester.Ok(!loop.IsDone());
			
		loop.Update(1.0f);
		Tester.Match(node.LocalAlpha, 0.0f);
		Tester.Match(loop.ExcessTime(), 0);
		Tester.Ok(loop.IsDone());
		
		node.Destroy();
	}
}