using UnityEngine;
using System.Collections.Generic;

public class Stage : MonoBehaviour {
	
	public const int LayerNodeWidth = 108;
	public const int LayerNodeHeight = 160;
	public const int LayerNum = 6;
	public const int LayerNodeNum = 3;
	public const float DeviceMoveErrorThreshold = 0.05f;
	public const float DeviceParallaxMoveAmount = 8.0f;
	private Roga2dNode root;
	private Roga2dRenderObject renderObject;
	private Roga2dNode[,] parallaxLayerNodes = new Roga2dNode[LayerNum, LayerNodeNum];
	private Roga2dNode[] parallaxLayers = null;
	private string bgFilePath;
	private float lastDeviceMoveX;
	private float lastDeviceMoveY;
	private Roga2dBaseInterval parallaxInterval;
	private float scrollDistance;

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
		new ParallaxLayerInfo(0.3f, 5, 0.3f)
	};

	void Awake() 
	{
		if (this.parallaxLayers == null) {
			this.parallaxLayers = this.buildParallaxLayers();
		}
	}
	
	private Roga2dNode[] buildParallaxLayers() {
		
		Roga2dNode[] parallaxLayers = new Roga2dNode[LayerNum];
		this.bgFilePath = "bg/night_background";
		
		Roga2dGameObjectState state = Roga2dUtils.stashState(this.transform);
		this.root = new Roga2dNode();
		this.root.Transform.parent = this.transform;
		Roga2dUtils.applyState(this.transform, state);
		
		Roga2dSprite sprite = new Roga2dSprite(this.bgFilePath, new Vector2(107, 160), new Vector2(0, 0), new Rect(0, 0, 107, 160));
		parallaxLayers[3] = sprite;
		this.root.AddChild(sprite);
		
		return parallaxLayers;
	}
	
	private Roga2dNode[] ParallaxLayers {
		get {
			if (this.parallaxLayers == null) {
				this.parallaxLayers = this.buildParallaxLayers();
			}
			return this.parallaxLayers;
		}
	}
	
	public Roga2dNode GetCharacterLayer() {
		return this.ParallaxLayers[3];
	}
	
	public void Scroll(float distance) {
		this.scrollDistance = distance;
	}
	
	private void UpdateScroll()
	{
		for (int i = 0; i < LayerNum; i++) {
			ParallaxLayerInfo layerInfo = layerInfoList[i];
			float deltaX = layerInfo.autoScrollSpeed * Time.deltaTime + layerInfo.moveInfluenceRatio * this.scrollDistance;
			float deltaY = 0;

			for (int j = 0; j < LayerNodeNum; j++) {
				Roga2dNode layerNode = this.parallaxLayerNodes[i, j];
				layerNode.LocalPixelPosition = new Vector2(layerNode.LocalPixelPosition.x + deltaX, layerNode.LocalPixelPosition.y + deltaY);
			}
			if (this.parallaxLayerNodes[i, 0].LocalPixelPosition.x > LayerNodeWidth) {
				
				this.ParallaxLayers[i].RemoveChild(this.parallaxLayerNodes[i, 0]);
				this.parallaxLayerNodes[i, 0] = this.parallaxLayerNodes[i, 1];
				this.parallaxLayerNodes[i, 1] = this.parallaxLayerNodes[i, 2];
	
				Roga2dSprite sprite = new Roga2dSprite(this.bgFilePath, new Vector2(LayerNodeWidth, LayerNodeHeight), new Vector2(0, 0), new Rect(0, i * LayerNodeHeight, LayerNodeWidth, LayerNodeHeight));
				sprite.LocalPixelPosition = new Vector2(this.parallaxLayerNodes[i, 1].LocalPixelPosition.x - LayerNodeWidth, 0);
				this.ParallaxLayers[i].AddChild(sprite);
				this.parallaxLayerNodes[i, 2] = sprite;
				sprite.LocalPriority = layerInfo.priority;
			}
		}
		
		this.scrollDistance = 0;
	}

	private void UpdateParallaxEffect(bool immediate)
	{
		/*
		float deviceMoveX = Input.acceleration.x;
		deviceMoveX = (deviceMoveX < 0.5f) ? deviceMoveX : 0.5f;
		deviceMoveX = (deviceMoveX > -0.5f) ? deviceMoveX : -0.5f;
		
		float deviceMoveY = Input.acceleration.y + 0.25f;
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
				
				if (immediate) {
					this.parallaxLayers[i].LocalPixelPosition = new Vector2(parallaxX, parallaxY);
				} else {
					intervals.Add(new Roga2dPositionInterval(this.parallaxLayers[i], this.parallaxLayers[i].LocalPosition, Roga2dUtils.pixelToLocal(new Vector2(parallaxX, parallaxY)), magnitude * 0.5f, true, null));
				}
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
		*/
	}

	public void UpdateView()
	{
		//this.UpdateScroll();
		//this.UpdateParallaxEffect(false);
		this.root.Update();
	}
}