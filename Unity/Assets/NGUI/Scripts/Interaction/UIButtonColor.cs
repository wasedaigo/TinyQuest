﻿using UnityEngine;

/// <summary>
/// Simple example script of how a button can be colored when the mouse hovers over it or it gets pressed.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Button Color")]
public class UIButtonColor : MonoBehaviour
{
	public GameObject tweenTarget;
	public Color hover = new Color(0.6f, 1f, 0.2f, 1f);
	public Color pressed = Color.grey;
	public float duration = 0.2f;

	Color mColor;

	void Start ()
	{
		if (tweenTarget == null) tweenTarget = gameObject;
		UIWidget widget = tweenTarget.GetComponent<UIWidget>();

		if (widget != null)
		{
			mColor = widget.color;
		}
		else
		{
			Renderer ren = tweenTarget.renderer;

			if (ren != null)
			{
				mColor = ren.material.color;
			}
			else
			{
				Light lt = tweenTarget.light;

				if (lt != null)
				{
					mColor = lt.color;
				}
				else
				{
					Debug.LogWarning(NGUITools.GetHierarchy(gameObject) + " has nothing for UIButtonColor to color");
					enabled = false;
				}
			}
		}
	}

	void OnPress (bool isPressed)
	{
		if (enabled) TweenColor.Begin(tweenTarget, duration, isPressed ? pressed : mColor);
	}

	void OnHover (bool isOver)
	{
		if (enabled) TweenColor.Begin(tweenTarget, duration, isOver ? hover : mColor);
	}
}