using UnityEngine;
using System.Diagnostics;

public class Tester : MonoBehaviour {
	
	private static int count = 0;
	private static int failCount = 0;
	public static void setup() {
		count = 0;
		failCount = 0;
	}
	
	public static void outputResult() {
		if (failCount > 0) {
			UnityEngine.Debug.Log("Assertion Failed (" + failCount + "/" + count + ")");
		} else {
			UnityEngine.Debug.Log(count + " tests succeeded");
		}
	}
	
	public static void Ok(bool input) {
		count += 1;
		if (!input) {
			failCount += 1;
			StackFrame CallStack = new StackFrame(1, true);
			string[] temp = CallStack.GetFileName().Split('/');
			string filename = temp[temp.Length - 1];
			UnityEngine.Debug.Log("Failed at " + filename + ":" + CallStack.GetFileLineNumber() + " < " + CallStack.GetMethod() );
		}
	}
	
	public static void Match(float input, float expect) {
		input = TestRound(input);
		expect = TestRound(expect);
		count += 1;
		if (input != expect) {
			failCount += 1;
			StackFrame CallStack = new StackFrame(1, true);
			string[] temp = CallStack.GetFileName().Split('/');
			string filename = temp[temp.Length - 1];
			UnityEngine.Debug.Log("Failed at " + filename + ":" + CallStack.GetFileLineNumber() + " < " + CallStack.GetMethod() + "\ninput:" + input + " expected:" + expect );
		}	
	}
	
	public static void Match(Vector2 input, Vector2 expect) {
		input.x = TestRound(input.x);
		input.y = TestRound(input.y);
		expect.x = TestRound(expect.x);
		expect.y = TestRound(expect.y);
		count += 1;
		if (input != expect) {
			failCount += 1;
			StackFrame CallStack = new StackFrame(1, true);
			string[] temp = CallStack.GetFileName().Split('/');
			string filename = temp[temp.Length - 1];
			UnityEngine.Debug.Log("Failed at " + filename + ":" + CallStack.GetFileLineNumber() + " < " + CallStack.GetMethod() + "\ninput:" + input + " expected:" + expect );
		}	
	}

	public static void Match(Vector3 input, Vector3 expect) {
		input.x = TestRound(input.x);
		input.y = TestRound(input.y);
		input.z = TestRound(input.z);
		expect.x = TestRound(expect.x);
		expect.y = TestRound(expect.y);
		expect.z = TestRound(expect.z);
		count += 1;
		if (input != expect) {
			failCount += 1;
			StackFrame CallStack = new StackFrame(1, true);
			string[] temp = CallStack.GetFileName().Split('/');
			string filename = temp[temp.Length - 1];
			UnityEngine.Debug.Log("Failed at " + filename + ":" + CallStack.GetFileLineNumber() + " < " + CallStack.GetMethod() + "\ninput:" + input + " expected:" + expect );
		}	
	}
	
	public static void Match<T>(T input, T expect) {
		count += 1;
		if (!input.Equals(expect)) {
			failCount += 1;
			StackFrame CallStack = new StackFrame(1, true);
			string[] temp = CallStack.GetFileName().Split('/');
			string filename = temp[temp.Length - 1];
			UnityEngine.Debug.Log("Failed at " + filename + ":" + CallStack.GetFileLineNumber() + " < " + CallStack.GetMethod() + "\ninput:" + input + " expected:" + expect );
		}	
	}
	
	private static float TestRound(float value) {
		value *= 100;
		value = Mathf.Round(value);
		value /= 100;
		return value;
	}
	
}