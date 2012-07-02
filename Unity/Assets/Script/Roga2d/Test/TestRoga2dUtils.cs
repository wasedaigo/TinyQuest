using UnityEngine;
using JsonFx.Json;

class TestRoga2dUtils{
	
	public static void Test() {
	
		TestCompletementFloat();
		TestCompletementVector2();
		TestParsePositionIntervalData();
		TestParseRotationIntervalData();
		TestParseScaleIntervalData();
		TestParseAlphaIntervalData();
		TestParseImageSourceIntervalData();
		TestParseAnimationSourceIntervalData();
		TestParseAnimationData();
		TestCoordinationConversion();
	}
	
	public static void TestCompletementFloat() {
		float value = Roga2dUtils.Completement(0.0f, 1.0f, 0.4f);
		Tester.Match(value, 0.4f);
	}
	
	public static void TestCompletementVector2() {
		Vector2 value = Roga2dUtils.Completement(new Vector2(0.0f, 1.0f), new Vector2(1.0f, 0.0f), 0.4f);
		Tester.Match(value, new Vector2(0.4f, 0.6f));
	}
	
	public static void TestParsePositionIntervalData() {
		Tester.Match(JsonReader.Deserialize<Roga2dPositionIntervalData>("{}").duration, 0);
		
		string jsonText = @"{
	          ""duration"": 5,
	          ""endPositionType"": ""None"",
	          ""startPositionAnchor"": [
	            1,
	            2
	          ],
	          ""endValue"": [
	            1,
	            2
	          ],
	          ""tween"": true,
	          ""startValue"": [
	            0,
	            -52
	          ],
	          ""endPositionAnchor"": [
	            -2,
	            -5
	          ],
	          ""frameNo"": 0,
	          ""startPositionType"": ""RelativeToTarget""
	        }
		";
		Roga2dPositionIntervalData intervaldata = JsonReader.Deserialize<Roga2dPositionIntervalData>(jsonText);
		Tester.Match(intervaldata.duration, 5);
		Tester.Match(intervaldata.startPositionType, Roga2dPositionType.RelativeToTarget);
		Tester.Match(intervaldata.endPositionType, Roga2dPositionType.None);
		Tester.Match(intervaldata.startPositionAnchor[0], 1);
		Tester.Match(intervaldata.startPositionAnchor[1], 2);
		Tester.Match(intervaldata.endPositionAnchor[0], -2);
		Tester.Match(intervaldata.endPositionAnchor[1], -5);
		Tester.Match(intervaldata.startValue[0], 0);
		Tester.Match(intervaldata.startValue[1], -52);
		Tester.Match(intervaldata.endValue[0], 1);
		Tester.Match(intervaldata.endValue[1], 2);
		Tester.Match(intervaldata.tweenType, Roga2dTweenType.Linear);
		Tester.Match(intervaldata.frameNo, 0);
		Tester.Match(intervaldata.wait, false);
	}
	
	public static void TestParseRotationIntervalData() {
		Tester.Match(JsonReader.Deserialize<Roga2dRotationIntervalData>("{}").duration, 0);
		
		string jsonText = @"{
          ""duration"": 7,
          ""endValue"": 315,
          ""tween"": true,
          ""startValue"": 0,
          ""facingOption"": ""FaceToMov"",
          ""frameNo"": 0
        }";
		
		Roga2dRotationIntervalData intervaldata = JsonReader.Deserialize<Roga2dRotationIntervalData>(jsonText);
		Tester.Match(intervaldata.duration, 7);
		Tester.Match(intervaldata.facingOption, Roga2dFacingType.FaceToMov);
		Tester.Match(intervaldata.startValue, 0);
		Tester.Match(intervaldata.endValue, 315);
		Tester.Match(intervaldata.tweenType, Roga2dTweenType.Linear);
		Tester.Match(intervaldata.frameNo, 0);
		Tester.Match(intervaldata.wait, false);
	}
	
	public static void TestParseScaleIntervalData() {
		Tester.Match(JsonReader.Deserialize<Roga2dScaleIntervalData>("{}").duration, 0);
		
		string jsonText = @"{
          ""duration"": 7,
          ""endValue"": [
            0.100000001490116,
            0.100000001490116
          ],
          ""tween"": true,
          ""startValue"": [
            1,
            1
          ],
          ""frameNo"": 0
        }";
		
		Roga2dScaleIntervalData intervaldata = JsonReader.Deserialize<Roga2dScaleIntervalData>(jsonText);
		Tester.Match(intervaldata.startValue[0], 1);
		Tester.Match(intervaldata.startValue[1], 1);
		Tester.Match(intervaldata.endValue[0], 0.100000001490116f);
		Tester.Match(intervaldata.endValue[1], 0.100000001490116f);
		Tester.Match(intervaldata.duration, 7);
		Tester.Match(intervaldata.frameNo, 0);
		Tester.Match(intervaldata.wait, false);
	}
	
	public static void TestParseAlphaIntervalData() {
		Tester.Match(JsonReader.Deserialize<Roga2dAlphaIntervalData>("{}").duration, 0);
		
		string jsonText = @"{
          ""duration"": 4,
          ""endValue"": 1.0,
          ""wait"": true,
          ""frameNo"": 3
        }";
		
		Roga2dAlphaIntervalData intervaldata = JsonReader.Deserialize<Roga2dAlphaIntervalData>(jsonText);
		Tester.Match(intervaldata.startValue, 0.0f);
		Tester.Match(intervaldata.endValue, 1.0f);
		Tester.Match(intervaldata.duration, 4);
		Tester.Match(intervaldata.frameNo, 3);
		Tester.Match(intervaldata.wait, true);
	}
	
	public static void TestParseImageSourceIntervalData() {
		Tester.Match(JsonReader.Deserialize<Roga2dSourceIntervalData>("{}").duration, 0);
		
		string jsonText = @"{
          ""duration"": 10,
          ""center"": [
            0,
            0
          ],
          ""priority"": 0.5,
          ""id"": ""Test/Test/Test"",
          ""type"": ""Image"",
          ""frameNo"": 0,
          ""rect"": [
            646,
            1216,
            44,
            98
          ]
        }";
		
		Roga2dSourceIntervalData intervaldata = JsonReader.Deserialize<Roga2dSourceIntervalData>(jsonText);
		Tester.Match(intervaldata.center[0], 0);
		Tester.Match(intervaldata.center[1], 0);
		Tester.Match(intervaldata.priority, 0.5f);
		Tester.Match(intervaldata.id, "Test/Test/Test");
		Tester.Match(intervaldata.type, Roga2dAnimationKeyFrameType.Image);
		Tester.Match(intervaldata.frameNo, 0);
		Tester.Match(intervaldata.rect[0], 646);
		Tester.Match(intervaldata.rect[1], 1216);
		Tester.Match(intervaldata.rect[2], 44);
		Tester.Match(intervaldata.rect[3], 98);
	}
	
	public static void TestParseAnimationSourceIntervalData() {
		string jsonText = @"{
          ""duration"": 1,
          ""emitter"": true,
          ""minEmitSpeed"": 2.0,
          ""priority"": 0.5,
          ""minEmitAngle"": 0,
          ""id"": ""Test/Test/Test"",
          ""maxEmitAngle"": 180,
          ""type"": ""Animation"",
          ""maxEmitSpeed"": 5.0,
          ""frameNo"": 0
        }";
		
		Roga2dSourceIntervalData intervaldata = JsonReader.Deserialize<Roga2dSourceIntervalData>(jsonText);
		Tester.Match(intervaldata.priority, 0.5f);
		Tester.Match(intervaldata.id, "Test/Test/Test");
		Tester.Match(intervaldata.type, Roga2dAnimationKeyFrameType.Animation);
		Tester.Match(intervaldata.frameNo, 0);
		Tester.Match(intervaldata.emitter, true);
		Tester.Match(intervaldata.minEmitAngle, 0);
		Tester.Match(intervaldata.maxEmitAngle, 180);
		Tester.Match(intervaldata.minEmitSpeed, 2.0f);
		Tester.Match(intervaldata.maxEmitSpeed, 5.0f);
	}
	
	public static void TestParseAnimationData() {
		string jsonText = @"
		{
			""dependencies"": {
				""animations"": [
				  ""Animation/Test""
				],
				""images"": [
				  ""Image/Test""
				]
			},
			""timelines"": [
				{
		          ""position"": [],
				  ""rotation"": [],
		          ""alpha"": [],
		          ""scale"": [],
				  ""source"": []
				}
			]
		}";
		
		Roga2dAnimationData animationData = JsonReader.Deserialize<Roga2dAnimationData>(jsonText);
		Tester.Match(animationData.dependencies.animations[0], "Animation/Test");
		Tester.Match(animationData.dependencies.images[0], "Image/Test");
		Tester.Match(animationData.timelines[0].position.Length, 0);
		Tester.Match(animationData.timelines[0].rotation.Length, 0);
		Tester.Match(animationData.timelines[0].scale.Length, 0);
		Tester.Match(animationData.timelines[0].alpha.Length, 0);
		Tester.Match(animationData.timelines[0].source.Length, 0);
	}
	
	public static void TestCoordinationConversion() {
		Tester.Match(Roga2dUtils.pixelToLocal(new Vector2(96.0f, 64.0f)), new Vector2(3.0f, 2.0f));
		Tester.Match(Roga2dUtils.localToPixel(new Vector2(3.0f, 2.0f)), new Vector2(96.0f, 64.0f));
	}
}