using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using System.IO;
using TinyQuest.Data;

namespace TinyQuest.Data.Request {
	public class MasterDataRequest
	{
		public virtual void GetStartUpData(System.Action<string> callback) {
		}
	}
}