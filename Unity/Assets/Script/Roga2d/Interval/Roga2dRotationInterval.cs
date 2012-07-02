using UnityEngine;

public class Roga2dRotationInterval : Roga2dValueInterval<float> {
	private Roga2dNode node;
	private Roga2dRotationIntervalOption option;
	private Roga2dRotationIntervalDataStore dataStore;
	
	public Roga2dRotationInterval(Roga2dNode node, float start, float end, float duration, Roga2dTweenType tween, Roga2dRotationIntervalOption option) 
	: base(start, end, duration, tween) 
	{
		this.node = node;
		this.option = option;
	}
	
	protected override float[] TweenBeforeFilter(float start, float end) {
		if (this.option.FacingType != Roga2dFacingType.None) {
			start = GetDynamicRotation( start, end, this.option, this.option.Target, this.option.DataStore);
			end = start;
		}
		return new float[2]{start, end};
	}

	protected override void SetValue(float value) {
		this.node.LocalRotation = value;
	}
	
	public override void Start() {
		if (this.option.DataStore != null) {
			this.option.DataStore.initialized = false;
		}
		base.Start();
	}
	
    private float GetDynamicRotation(float start, float end, Roga2dRotationIntervalOption option, Roga2dNode target, Roga2dRotationIntervalDataStore dataStore) {
        float result = start;
		if (target != null) {
			Transform root = node.Transform.root.gameObject.transform;
			bool reverse = node.Parent != null ? node.Parent.IsReversed : false;
            if (this.option.FacingType == Roga2dFacingType.FaceToDir) {
				Roga2dGameObjectState state = Roga2dUtils.stashState(root);
                float dx = target.Position.x - node.Position.x;
                float dy = target.Position.y - node.Position.y;
                result += (Mathf.Atan2(dy,dx) / Mathf.PI)  * 180;
				Roga2dUtils.applyState(root, state);
            } else if (this.option.FacingType == Roga2dFacingType.FaceToMov) {
				Roga2dGameObjectState state = Roga2dUtils.stashState(root);
                float dx = node.Position.x - dataStore.lastAbsPosition.x;
                float dy = node.Position.y - dataStore.lastAbsPosition.y;
				if (reverse) { dx *= -1; }
                
				float rotation = 0;
				if (dataStore.initialized) {
					node.Show();
					if (dx == 0 && dy == 0) {
						rotation = dataStore.lastRotation;
					} else {
						rotation = (Mathf.Atan2(dy,dx) / Mathf.PI)  * 180;
					}
				} else {
					node.Hide();
					dataStore.initialized = true;
				}

				result += rotation;
				
				dataStore.lastRotation = rotation;
                dataStore.lastAbsPosition = node.Position;
				Roga2dUtils.applyState(root, state);
            }
        }
		
        return result;
    }
}