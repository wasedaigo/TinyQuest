using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using System.IO;
using TinyQuest.Data;
using Async;

namespace TinyQuest.Data.Request {
	public class LocalUserDataRequestMock<T> : LocalUserDataRequest<T>
	{
		public override void Get(System.Action<string> callback) {
			TextAsset txt = (TextAsset)Resources.Load("Data/LocalUser", typeof(TextAsset));
			callback(txt.text);
		}
		
	}
}
