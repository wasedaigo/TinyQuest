using UnityEngine;

public class Roga2dPositionInterval : Roga2dValueInterval<Vector2> {
	private Roga2dNode node;
	private Roga2dPositionIntervalOption option;
	
	public Roga2dPositionInterval(Roga2dNode node, Vector2 start, Vector2 end, int duration, bool tween, Roga2dPositionIntervalOption option)
	: base(start, end, duration, tween) 
	{
		this.node = node;
		this.option = option;
	}
	
	protected override Vector2[] TweenBeforeFilter(Vector2 start, Vector2 end) {
		if (this.option.Target != null) {
			start = GetRelativePosition(node, this.option.Target, this.option.StartPositionType, this.option.StartPositionAnchor, start);
			end = GetRelativePosition(node, this.option.Target, this.option.EndPositionType, this.option.EndPositionAnchor, end);
		}
		
		return new Vector2[2] {start, end};
	}
	
	protected override void SetValue(Vector2 value) {
		this.node.LocalPosition = value;
	}
	
    private Vector2 GetRelativePosition(Roga2dNode node, Roga2dNode target, Roga2dPositionType positionType, Vector2 positionAnchor, Vector2 offset) {
		Vector2 localTargetPosition = offset;
        
		if (node.Parent != null) {
	        if (positionType == Roga2dPositionType.RelativeToTarget) {
	            Vector2 anchorOffset = target.GetOffsetByPositionAnchor(positionAnchor.x, positionAnchor.y);
	            
				Vector2 targetPosition = target.Position + offset + anchorOffset;
				localTargetPosition = node.Parent.InverseTransformPoint(targetPosition);
	        } else if (positionType == Roga2dPositionType.RelativeToTargetOrigin) {
	            Vector2 anchorOffset = target.GetOffsetByPositionAnchor(positionAnchor.x, positionAnchor.y);
				Vector2 targetPosition = target.Origin + offset + anchorOffset;
				localTargetPosition = node.Parent.InverseTransformPoint(targetPosition);
	        }
		}
        return localTargetPosition;
    }
}