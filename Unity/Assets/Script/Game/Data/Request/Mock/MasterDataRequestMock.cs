using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using System.IO;
using TinyQuest.Data;
using Async;

namespace TinyQuest.Data.Request {
	public class MasterDataRequestMock : MasterDataRequest
	{
		public override void GetStartUpData(System.Action<string> callback) {
			TextAsset txt = (TextAsset)Resources.Load("Data/Master", typeof(TextAsset));
			callback(txt.text);
		}
	}
}