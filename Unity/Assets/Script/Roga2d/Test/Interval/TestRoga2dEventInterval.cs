using UnityEngine;
using System.Collections.Generic;

class TestRoga2dEventInterval {
	private static int counter = 0;
	public static void Test() {
		TestUpdate();
	}
	
	void CommandCalled(string command) 
	{
		if (counter == 0) {
			Tester.Match(command, "test1");
		} else if(counter == 2) {
			Tester.Match(command, "test2");
		} else if(counter == 3) {
			Tester.Match(command, "test3");
		}
		counter += 1;
	}
	
	public static void TestUpdate() {
		Dictionary<int, string[]> events = new Dictionary<int, string[]>();
		
		string[] test1 = {"test1", "test2"};
		string[] test2 = {"test3"};
		events.Add(1, test1);
		events.Add(5, test2);

		Roga2dBaseInterval interval = new Roga2dEventInterval(events, this.CommandCalled);
		
		interval.Start();
		interval.Update();
		interval.Update();
		interval.Update();
		interval.Update();
		Tester.Ok(!interval.IsDone());
		
		interval.Update();
		Tester.Ok(interval.IsDone());
	}
}