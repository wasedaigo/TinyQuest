using UnityEngine;
using System.Collections.Generic;

namespace TinyQuest.Component {
	public class Stage : Roga2dNode {
		public delegate void StageScrollFinishEventHandler();
		public event StageScrollFinishEventHandler ScrollFinished;
		
		public const int LayerNodeWidth = 160;
		public const int LayerNodeHeight = 160;
		public const int LayerNum = 6;
		public const int LayerNodeNum = 3;
		public const float DeviceMoveErrorThreshold = 0.05f;
		public const float DeviceParallaxMoveAmount = 16.0f;
		private Roga2dNode root;
		private Roga2dRenderObject renderObject;
		private Roga2dNode[,] parallaxLayerNodes = new Roga2dNode[LayerNum, LayerNodeNum];
		private Roga2dNode[] parallaxLayers = new Roga2dNode[LayerNum];
		private string bgFilePath;
		private float lastDeviceMoveX;
		private float lastDeviceMoveY;
		private Roga2dBaseInterval parallaxInterval;
	
		public struct ParallaxLayerInfo {
			public float moveInfluenceRatio;
			public float autoScrollSpeed;
			public float priority;
			public ParallaxLayerInfo(float moveInfluenceRatio, float autoScrollSpeed, float priority) {
				this.moveInfluenceRatio = moveInfluenceRatio;
				this.autoScrollSpeed = autoScrollSpeed;
				this.priority = priority;
			}
		};

		public static Vector2[] initialLayerPos = new Vector2[LayerNodeNum] {
			new Vector2(LayerNodeWidth, 0),
			new Vector2(0, 0),
			new Vector2(-LayerNodeWidth, 0)
		};
		
		public static ParallaxLayerInfo[] layerInfoList = new ParallaxLayerInfo[LayerNum] {
			new ParallaxLayerInfo(0.1f, 0, 0.01f),
			new ParallaxLayerInfo(0.3f, 0, 0.02f),
			new ParallaxLayerInfo(0.5f, 0, 0.03f),
			new ParallaxLayerInfo(1.0f, 0, 0.04f),
			new ParallaxLayerInfo(2.0f, 0, 2),
			new ParallaxLayerInfo(0.0f, 30, 0.3f)
		};

		public Stage() 
		{
			this.bgFilePath = "bg/bg0002";
			this.root = new Roga2dNode();
			this.AddChild(this.root);
			
			this.setupStage();
		}
		
		private void setupStage() {
			for (int i = 0; i < LayerNum; i++) {
				Roga2dNode node = new Roga2dNode();
				for (int j = 0; j < LayerNodeNum; j++) {
					ParallaxLayerInfo layerInfo = layerInfoList[i];
		
					Roga2dSprite sprite = new Roga2dSprite(this.bgFilePath, new Vector2(LayerNodeWidth, LayerNodeHeight), new Vector2(0, 0), new Rect(0, i * LayerNodeWidth, LayerNodeWidth, LayerNodeHeight));
					sprite.LocalPixelPosition = initialLayerPos[j];
					this.parallaxLayerNodes[i, j] = sprite;
					sprite.LocalPriority = layerInfo.priority;

					node.AddChild(sprite);
				}
				this.root.AddChild(node);
				this.parallaxLayers[i] = node;
			}
		}
		
		public Roga2dNode GetCharacterLayer() {
			return this.parallaxLayers[3];
		}
		
		public void Scroll(float duration) {
			Roga2dPositionInterval posInterval = new Roga2dPositionInterval(this, new Vector2(0, 0), Roga2dUtils.pixelToLocal(new Vector2(320, 0)), duration, true, null);
			Roga2dFunc func = new Roga2dFunc(this.onScrolled);
			
			Roga2dBaseInterval interval = new Roga2dSequence(new List<Roga2dBaseInterval> {posInterval, func});
			Roga2dIntervalPlayer.GetInstance().Play(interval);
		}
		
		private void onScrolled() {
			if (this.ScrollFinished != null) {
				this.ScrollFinished();	
			}
		}
	
		private void UpdateScroll()
		{
			for (int i = 0; i < LayerNum; i++) {
				ParallaxLayerInfo layerInfo = layerInfoList[i];
				float deltaX = layerInfo.autoScrollSpeed * Time.deltaTime;
				float deltaY = 0;

				for (int j = 0; j < LayerNodeNum; j++) {
					Roga2dNode layer = this.parallaxLayerNodes[i, j];
					layer.LocalPixelPosition = new Vector2(layer.LocalPixelPosition.x + deltaX, layer.LocalPixelPosition.y + deltaY);
				}
				if (this.parallaxLayerNodes[i, 0].LocalPixelPosition.x > LayerNodeWidth) {
					this.root.RemoveChild(this.parallaxLayerNodes[i, 0]);
					this.parallaxLayerNodes[i, 0] = this.parallaxLayerNodes[i, 1];
					this.parallaxLayerNodes[i, 1] = this.parallaxLayerNodes[i, 2];

					Roga2dSprite sprite = new Roga2dSprite(this.bgFilePath, new Vector2(LayerNodeWidth, LayerNodeHeight), new Vector2(0, 0), new Rect(0, i * LayerNodeHeight, LayerNodeWidth, LayerNodeHeight));
					sprite.LocalPixelPosition = new Vector2(this.parallaxLayerNodes[i, 1].LocalPixelPosition.x - LayerNodeWidth, 0);
					this.parallaxLayerNodes[i, 2] = sprite;
					this.root.AddChild(sprite);
					sprite.LocalPriority = layerInfo.priority;
				}
			}
		}

		private void UpdateParallaxEffect()
		{
			float deviceMoveX = Input.acceleration.y;
			deviceMoveX = (deviceMoveX < 0.5f) ? deviceMoveX : 0.5f;
			deviceMoveX = (deviceMoveX > -0.5f) ? deviceMoveX : -0.5f;
			
			float deviceMoveY = Input.acceleration.x + 0.25f;
			deviceMoveY = (deviceMoveY < 0.5f) ? deviceMoveY : 0.5f;
			deviceMoveY = (deviceMoveY > -0.5f) ? deviceMoveY : -0.5f;
			
			float lastMagnitude = Mathf.Abs(this.lastDeviceMoveX) + Mathf.Abs(this.lastDeviceMoveY);
			float magnitude = Mathf.Abs(deviceMoveX) + Mathf.Abs(deviceMoveY);

			if (Mathf.Abs(lastMagnitude - magnitude) > DeviceMoveErrorThreshold) {				
				List<Roga2dBaseInterval> intervals = new List<Roga2dBaseInterval>();
				for (int i = 0; i < LayerNum; i++) {
					ParallaxLayerInfo layerInfo = layerInfoList[i];
					float parallaxX = deviceMoveX * layerInfo.moveInfluenceRatio * DeviceParallaxMoveAmount;
					float parallaxY = deviceMoveY * layerInfo.moveInfluenceRatio * DeviceParallaxMoveAmount;
					intervals.Add(new Roga2dPositionInterval(this.parallaxLayers[i], this.parallaxLayers[i].LocalPosition, Roga2dUtils.pixelToLocal(new Vector2(parallaxX, parallaxY)), magnitude * 0.5f, true, null));
				}
				if (this.parallaxInterval != null) {
					Roga2dIntervalPlayer.GetInstance().Stop(this.parallaxInterval);
					this.parallaxInterval = null;
				}
				this.parallaxInterval = new Roga2dParallel(intervals);
				Roga2dIntervalPlayer.GetInstance().Play(this.parallaxInterval);
				this.lastDeviceMoveX = deviceMoveX;
				this.lastDeviceMoveY = deviceMoveY;
			}
		}

		public override void Update()
		{
			base.Update();
			
			this.UpdateScroll();
			this.UpdateParallaxEffect();
		}
	}
}