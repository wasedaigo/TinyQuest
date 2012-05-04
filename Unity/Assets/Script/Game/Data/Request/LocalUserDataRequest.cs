using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using System.IO;
using TinyQuest.Data;

namespace TinyQuest.Data.Request {
	public class LocalUserDataRequest<T>
	{
		public virtual void Get(System.Action<string> callback) {
		}
	}
}
