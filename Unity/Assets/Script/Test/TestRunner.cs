using UnityEngine;

public class TestRunner : MonoBehaviour {
	void Start () {
		Tester.setup();
		TestRoga2dPositionInterval.Test();
		TestRoga2dRotationInterval.Test();
		TestRoga2dAlphaInterval.Test();
		TestRoga2dScaleInterval.Test();
		TestRoga2dHueInterval.Test();
		TestRoga2dWait.Test();
		TestRoga2dSequence.Test();
		TestRoga2dParallel.Test();
		TestRoga2dLoop.Test();
		TestRoga2dSourceInterval.Test();
		TestRoga2dUtils.Test();
		TestRoga2dNode.Test();
		TestRoga2dSprite.Test();
		TestRoga2dAnimationPlayer.Test();

		Tester.outputResult();
	}
}