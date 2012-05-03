using System;

namespace Async {
	public interface IAsync {
		void Load(System.Action callback);
	}
}