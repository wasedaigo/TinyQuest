using UnityEngine;
using System.Collections.Generic;

class TestRoga2dSequence {
	
	public static void Test() {
		TestTween();
		TestMixTween();
		TestDeltaTime();
	}
	
	public static void TestTween() {
		Roga2dNode node = new Roga2dNode();

		Roga2dAlphaInterval interval1 = new Roga2dAlphaInterval(node, 0.1f, 1.0f, 3, true);
		Roga2dWait interval2 = new Roga2dWait(2);
		Roga2dAlphaInterval interval3 = new Roga2dAlphaInterval(node, 0.7f, 0.0f, 2, true);

		List<Roga2dBaseInterval> intervals = new List<Roga2dBaseInterval>();
		intervals.Add(interval1);
		intervals.Add(interval2);
		intervals.Add(interval3);
		
		Roga2dSequence sequence = new Roga2dSequence(intervals);
		
		sequence.Start();
		Tester.Match(node.LocalAlpha, 0.1f);
		Tester.Ok(!sequence.IsDone());
		
		sequence.Update(1.0f);
		Tester.Match(node.LocalAlpha, 0.4f);
		Tester.Ok(!sequence.IsDone());

		sequence.Update(1.0f);
		Tester.Match(node.LocalAlpha, 0.7f);
		Tester.Ok(!sequence.IsDone());
		
		sequence.Update(1.0f);
		Tester.Match(node.LocalAlpha, 1.0f);
		Tester.Ok(!sequence.IsDone());
		
		sequence.Update(1.0f);
		Tester.Match(node.LocalAlpha, 1.0f);
		Tester.Ok(!sequence.IsDone());
		
		sequence.Update(1.0f);
		Tester.Match(node.LocalAlpha, 1.0f);
		Tester.Ok(!sequence.IsDone());
		
		sequence.Update(1.0f);
		Tester.Match(node.LocalAlpha, 0.35f);
		Tester.Ok(!sequence.IsDone());
		
		
		sequence.Update(1.0f);
		Tester.Match(node.LocalAlpha, 0.0f);
		Tester.Ok(sequence.IsDone());
		
		sequence.Reset();
		Tester.Match(node.LocalAlpha, 0.1f);
		Tester.Ok(!sequence.IsDone());
		
		node.Destroy();
	}
	
	public static void TestMixTween() {
		Roga2dNode node = new Roga2dNode();

		Roga2dAlphaInterval interval1 = new Roga2dAlphaInterval(node, 1.0f, 0.5f, 1, false);
		Roga2dAlphaInterval interval2 = new Roga2dAlphaInterval(node, 0.5f, 0.3f, 1, false);
		Roga2dAlphaInterval interval3 = new Roga2dAlphaInterval(node, 0.3f, 0.1f, 1, true);
		Roga2dAlphaInterval interval4 = new Roga2dAlphaInterval(node, 0.1f, 0.1f, 1, true);

		List<Roga2dBaseInterval> intervals = new List<Roga2dBaseInterval>();
		intervals.Add(interval1);
		intervals.Add(interval2);
		intervals.Add(interval3);
		intervals.Add(interval4);
		
		Roga2dSequence sequence = new Roga2dSequence(intervals);
		
		sequence.Start();
		Tester.Match(node.LocalAlpha, 1.0f);
		Tester.Ok(!sequence.IsDone());
		
		sequence.Update(1.0f);
		Tester.Match(node.LocalAlpha, 0.5f);
		Tester.Ok(!sequence.IsDone());

		sequence.Update(1.0f);
		Tester.Match(node.LocalAlpha, 0.3f);
		Tester.Ok(!sequence.IsDone());
		
		sequence.Update(1.0f);
		Tester.Match(node.LocalAlpha, 0.1f);
		Tester.Ok(!sequence.IsDone());
		
		sequence.Update(1.0f);
		Tester.Match(node.LocalAlpha, 0.1f);
		Tester.Ok(sequence.IsDone());
		
		node.Destroy();
	}
	
	public static void TestDeltaTime() {
		Roga2dNode node = new Roga2dNode();

		Roga2dAlphaInterval interval1 = new Roga2dAlphaInterval(node, 1.0f, 0.0f, 1.0f, true);
		Roga2dAlphaInterval interval2 = new Roga2dAlphaInterval(node, 0.0f, 1.0f, 1.0f, true);

		List<Roga2dBaseInterval> intervals = new List<Roga2dBaseInterval>();
		intervals.Add(interval1);
		intervals.Add(interval2);
		
		Roga2dSequence sequence = new Roga2dSequence(intervals);
		
		sequence.Start();
		Tester.Match(node.LocalAlpha, 1.0f);
		Tester.Match(sequence.ExcessTime(), -1);
		Tester.Ok(!sequence.IsDone());
		
		sequence.Update(0.5f);
		Tester.Match(node.LocalAlpha, 0.5f);
		Tester.Match(sequence.ExcessTime(), -1);
		Tester.Ok(!sequence.IsDone());

		sequence.Update(0.5f);
		Tester.Match(node.LocalAlpha, 0.0f);
		Tester.Match(sequence.ExcessTime(), -1);
		Tester.Ok(!sequence.IsDone());
	
		sequence.Update(0.2f);
		Tester.Match(node.LocalAlpha, 0.2f);
		Tester.Match(sequence.ExcessTime(), -1);
		Tester.Ok(!sequence.IsDone());
		
		sequence.Update(0.2f);
		Tester.Match(node.LocalAlpha, 0.4f);
		Tester.Match(sequence.ExcessTime(), -1);
		Tester.Ok(!sequence.IsDone());
		
		sequence.Update(0.2f);
		Tester.Match(node.LocalAlpha, 0.6f);
		Tester.Match(sequence.ExcessTime(), -1);
		Tester.Ok(!sequence.IsDone());
		
		sequence.Update(0.2f);
		Tester.Match(node.LocalAlpha, 0.8f);
		Tester.Match(sequence.ExcessTime(), -1);
		Tester.Ok(!sequence.IsDone());
		
		sequence.Update(1.0f);
		Tester.Match(node.LocalAlpha, 1.0f);
		Tester.Match(sequence.ExcessTime(), 0.8f);
		Tester.Ok(sequence.IsDone());
		
		node.Destroy();
	}	
}