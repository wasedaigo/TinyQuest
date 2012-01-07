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
	
	protected override void TweenBeforeFilter(ref Vector2 start, ref Vector2 end) {
		if (this.option.Target != null) {
			start = GetRelativePosition(node, this.option.Target, this.option.StartPositionType, this.option.StartPositionAnchor, start);
			end = GetRelativePosition(node, this.option.Target, this.option.EndPositionType, this.option.EndPositionAnchor, end);
		}
	}
	
	protected override void SetValue(Vector2 value) {
		this.node.LocalPosition = value;
	}
	
    private Vector2 GetRelativePosition(Roga2dNode node, Roga2dNode target, Roga2dPositionType positionType, Vector2 positionAnchor, Vector2 offset) {
        Vector2 localTargetPosition = offset;
        
        if (positionType == Roga2dPositionType.RelativeToTarget) {
            Vector2 anchorOffset = target.GetOffsetByPositionAnchor(positionAnchor.x, positionAnchor.y);
			
			// A lot of Pixel-Local coordinate transformation. Waste...
			Vector2 targetPosition = Roga2dUtils.localToPixel(target.Position) + offset + anchorOffset;
			localTargetPosition = node.InverseTransformPoint(Roga2dUtils.pixelToLocal(targetPosition));
			localTargetPosition = Roga2dUtils.localToPixel(localTargetPosition);
        } else if (positionType == Roga2dPositionType.RelativeToTargetOrigin) {
            Vector2 anchorOffset = target.GetOffsetByPositionAnchor(positionAnchor.x, positionAnchor.y);
			Vector2 targetPosition = target.Origin + offset + anchorOffset;
			localTargetPosition = node.InverseTransformPoint(targetPosition);
            
        }
        return localTargetPosition;
    }
}