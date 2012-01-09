using UnityEngine;

class TestRoga2dAnimationPlayer {
	
	public static void Test() {
		TestPlay();
	}
	
	public static void TestPlay() {
		GameObject gameObject = new GameObject();
		gameObject.AddComponent("Roga2dAnimationPlayer");
		Roga2dAnimationPlayer player = gameObject.GetComponent("Roga2dAnimationPlayer") as Roga2dAnimationPlayer;
		
		Roga2dWait interval = new Roga2dWait(3);
		Roga2dNode node = new Roga2dNode();
		Roga2dAnimation animation = Roga2dAnimation.Build(node, interval);
		player.Start();
		player.Play(player.transform, animation);
		
		Tester.Ok(!interval.IsDone());
        
        player.Update();
		Tester.Ok(!interval.IsDone());
        
        player.Update();
		Tester.Ok(!interval.IsDone());

        player.Update();
		Tester.Ok(!interval.IsDone());
		
        player.Update();
		Tester.Ok(interval.IsDone());
		
		node.Destroy();
		Object.Destroy(gameObject);
	}
}