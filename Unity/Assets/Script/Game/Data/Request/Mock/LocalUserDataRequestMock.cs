using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using System.IO;
using TinyQuest.Data;
using Async;

public class LocalUserDataRequestMock<T> : LocalUserDataRequest<T>
	where T : BaseMasterData
{
	public override void Get(System.Action<string> callback) {
		string path = typeof(T).Name;
		TextAsset txt = (TextAsset)Resources.Load("Data/User/Local/" + path, typeof(TextAsset));
		callback(txt.text);
	}
}
