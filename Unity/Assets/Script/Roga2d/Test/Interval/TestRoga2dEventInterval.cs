using UnityEngine;
using System.Collections.Generic;

class TestRoga2dEventInterval {
	private static int counter = 0;
	public static void Test() {
		TestUpdate();
	}
	
	private static void CommandCalled(Roga2dAnimationSettings settings, string command) 
	{
		if (counter == 0) {
			Tester.Match(command, "test1");
		} else if(counter == 1) {
			Tester.Match(command, "test2");
		} else if(counter == 2) {
			Tester.Match(command, "test3");
		}
		counter += 1;
	}
	
	private static void TestUpdate() {
		Dictionary<int, string[]> events = new Dictionary<int, string[]>();
		
		string[] test1 = {"test1", "test2"};
		string[] test2 = {"test3"};
		events.Add(1, test1);
		events.Add(5, test2);
		
		Roga2dAnimationSettings settings = new Roga2dAnimationSettings(null, null, null, null, CommandCalled);
		Roga2dBaseInterval interval = new Roga2dEventInterval(events, settings);
		
		interval.Start();
		interval.Update(1.0f);
		interval.Update(1.0f);
		interval.Update(1.0f);
		interval.Update(1.0f);
		interval.Update(1.0f);
		Tester.Ok(!interval.IsDone());
		
		interval.Update(1.0f);
		Tester.Ok(interval.IsDone());
		
		Tester.Match(counter, 3);
	}
}