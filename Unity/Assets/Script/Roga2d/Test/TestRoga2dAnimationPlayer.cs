using UnityEngine;

class TestRoga2dAnimationPlayer {
	
	public static void Test() {
		TestPlay();
	}
	
	private static int testCounter = 0;
    private static void AnimationFinished(Roga2dAnimation animation)
    {
		testCounter = 999;
    }

	public static void TestPlay() {
		GameObject gameObject = new GameObject();
		Roga2dAnimationPlayer player = new Roga2dAnimationPlayer();
		Roga2dWait interval = new Roga2dWait(3);
		Roga2dNode node = new Roga2dNode();
		Roga2dAnimation animation = Roga2dAnimation.Build(node, interval);
		player.Play(null, null, animation, AnimationFinished);
		
		Tester.Ok(!interval.IsDone());
        
        player.Update(1.0f);
		Tester.Ok(!interval.IsDone());
        
        player.Update(1.0f);
		Tester.Ok(!interval.IsDone());

        player.Update(1.0f);
		Tester.Ok(!interval.IsDone());
		
		Tester.Match(testCounter, 0);

        player.Update(1.0f);
		Tester.Ok(interval.IsDone());
		Tester.Match(testCounter, 999);
		
		node.Destroy();
		Object.Destroy(gameObject);
	}
}