using UnityEngine;

public class Roga2dRotationInterval : Roga2dValueInterval<float> {
	private Roga2dNode node;
	private Roga2dRotationIntervalOption option;
	private Roga2dRotationIntervalDataStore dataStore;
	
	public Roga2dRotationInterval(Roga2dNode node, float start, float end, int duration, bool tween, Roga2dRotationIntervalOption option) 
	: base(start, end, duration, tween) 
	{
		this.node = node;
		this.option = option;
		
		if (this.option.FacingType == Roga2dFacingType.FaceToMov) {
			this.dataStore = new Roga2dRotationIntervalDataStore();
		}
	}

	protected override float[] TweenBeforeFilter(float start, float end) {
		if (this.option.FacingType != Roga2dFacingType.None) {
			start = GetDynamicRotation( start, end, this.option, this.option.Target, this.dataStore);
			end = start;
		}
		return new float[2]{start, end};
	}

	protected override void SetValue(float value) {
		this.node.LocalRotation = value;
	}
	
    private float GetDynamicRotation(float start, float end, Roga2dRotationIntervalOption option, Roga2dNode target, Roga2dRotationIntervalDataStore dataStore) {
        float result = start;
		if (target != null) {
			Transform root = node.Transform.root.gameObject.transform;
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
                
                result += (Mathf.Atan2(dy,dx) / Mathf.PI)  * 180;
                if (dataStore.ignore) {
                    node.LocalAlpha = dataStore.lastAlpha;
                } else {
                    dataStore.ignore = true;
                    dataStore.lastAlpha = node.LocalAlpha;
                    node.LocalAlpha = 0;
                }
                // Extra data for FaceToMov option
                dataStore.lastAbsPosition = node.Position;
				Roga2dUtils.applyState(root, state);
            }
        }
		
        return result;
    }
}