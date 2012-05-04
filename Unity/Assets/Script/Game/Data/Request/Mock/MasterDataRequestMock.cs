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
		
		public override void GetZone(int id, System.Action<string> callback) {
			TextAsset txt = (TextAsset)Resources.Load("Data/Zone/" + id, typeof(TextAsset));
			callback(txt.text);
		}

		public override void GetLocalizedText(string lang, System.Action<string> callback) {
			TextAsset txt = (TextAsset)Resources.Load("Data/Localize/" + lang, typeof(TextAsset));
			callback(txt.text);
		}
	}
}