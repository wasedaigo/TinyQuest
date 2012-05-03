using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using System.IO;
using TinyQuest.Data;
using Async;

public class MasterDataRequestMock<T> : MasterDataRequest<T>
	where T : BaseMasterData
{
	public override void Get(System.Action<MasterDataCollection<T>> callback) {
		string path = typeof(T).Name;
		TextAsset txt = (TextAsset)Resources.Load("Data/Master/" + path, typeof(TextAsset));
		MasterDataCollection<T> data = JsonReader.Deserialize<MasterDataCollection<T>>(txt.text);
		callback(data);
	}
}
