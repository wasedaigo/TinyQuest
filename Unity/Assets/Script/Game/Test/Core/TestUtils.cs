using UnityEngine;
using TinyQuest.Core;
using System.Collections.Generic;

namespace TinyQuest.Test.Core {
	class TestUtils{
		
		public static void Test() {
			TestGetDigits();
		}
		
		public static void TestGetDigits() {
			List<uint> digits = Utils.getDigits(0);
			Tester.Match(digits.Count, 1);
			Tester.Match(digits[0], 0);
			
			digits = Utils.getDigits(12345);
			Tester.Match(digits.Count, 5);
			Tester.Match(digits[0], 1);
			Tester.Match(digits[1], 2);
			Tester.Match(digits[2], 3);
			Tester.Match(digits[3], 4);
			Tester.Match(digits[4], 5);
		}
	
	}
}