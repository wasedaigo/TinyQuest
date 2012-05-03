using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using System.IO;
using TinyQuest.Data;

public class LocalUserDataRequest<T>
	where T : BaseMasterData
{
	public virtual void Get(System.Action<MasterDataCollection<T>> callback) {
	}
}
