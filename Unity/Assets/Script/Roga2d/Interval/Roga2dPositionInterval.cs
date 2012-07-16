using UnityEngine;

public class Roga2dPositionInterval : Roga2dValueInterval<Vector2> {
	private Roga2dNode node;
	private Roga2dPositionIntervalOption option;
	
	public Roga2dPositionInterval(Roga2dNode node, Vector2 start, Vector2 end, float duration, Roga2dTweenType tween, Roga2dPositionIntervalOption option)
	: base(start, end, duration, tween) 
	{
		this.node = node;
		this.option = option;
	}
	
	protected override Vector2[] TweenBeforeFilter(Vector2 start, Vector2 end) {
		if (this.option != null && this.option.Target != null) {
			start = GetRelativePosition(node, this.option.StartPositionType, this.option.StartPositionAnchor, start);
			end = GetRelativePosition(node, this.option.EndPositionType, this.option.EndPositionAnchor, end);
		}
		
		return new Vector2[2] {start, end};
	}
	
	protected override void SetValue(Vector2 value) {
		this.node.LocalPosition = value;
	}
	
    private Vector2 GetRelativePosition(Roga2dNode node, Roga2dPositionType positionType, Vector2 positionAnchor, Vector2 offset) {
		Vector2 localTargetPosition = offset;
        
		Vector2 casterPixelSize = this.option.CasterPixelSize;
		Roga2dNode target = this.option.Target;
		Roga2dNode targetOrigin = this.option.TargetOrigin; 
		
		if (node.Parent != null) {
			float lossyScaleX = node.Parent != null ? node.Parent.Transform.lossyScale.x : 1;
			Transform root = node.Transform.root.gameObject.transform;
	        if (positionType == Roga2dPositionType.RelativeToTarget) {
				Roga2dGameObjectState state = Roga2dUtils.stashState(root);
	            Vector2 anchorOffset = Roga2dUtils.pixelToLocal(target.GetOffsetByPositionAnchor(casterPixelSize, positionAnchor.x, positionAnchor.y));
				anchorOffset.x *= lossyScaleX;

				Vector2 targetPosition = target.Position + offset + anchorOffset;
				
				localTargetPosition = node.Parent.InverseTransformPoint(targetPosition);
				Roga2dUtils.applyState(root, state);	
				
	        } else if (positionType == Roga2dPositionType.RelativeToTargetOrigin) {
				Roga2dGameObjectState state = Roga2dUtils.stashState(root);
	            Vector2 anchorOffset = Roga2dUtils.pixelToLocal(target.GetOffsetByPositionAnchor(casterPixelSize, positionAnchor.x, positionAnchor.y));
				anchorOffset.x *= lossyScaleX;
				Vector2 targetPosition = targetOrigin.Position + offset + anchorOffset;
				
				localTargetPosition = node.Parent.InverseTransformPoint(targetPosition);
				Roga2dUtils.applyState(root, state);
	        }
		}
        return localTargetPosition;
    }
}