using UnityEngine;
using TinyQuest.Core;

namespace TinyQuest.Component {
	public class BaseComponent : Roga2dNode {
		public event WindowMessageEventHandler MessageEvent;
	
		public void SendMessage(WindowMessage message) 
		{
			if (this.MessageEvent != null && message != null) {
				this.MessageEvent(message);	
			}
		}
		
		public virtual void ReceiveMessage(WindowMessage message) 
		{
		}

		public virtual void OnTouchMoved(Vector2 delta) {
		}
	}
}