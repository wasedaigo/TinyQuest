using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Data;

namespace TinyQuest.Data.Request {
	public class RequestFactory {
		public static readonly RequestFactory Instance = new RequestFactory();
		private RequestFactory(){}

		private bool mockEnabled;
		public void EnableMock(bool mockEnabled) {
			this.mockEnabled = mockEnabled;
		}
		
		public MasterDataRequest<T> GetMasterDataRequest<MasterDataRequest, T>() 
			where T : BaseMasterData
		{
			if (this.mockEnabled) {
				return new MasterDataRequestMock<T>();
			} else {
				return new MasterDataRequest<T>();
			}
		}
		
		public LocalUserDataRequest<T> GetUserDataRequest<LocalUserDataRequest, T>() 
			where T : BaseMasterData
		{
			if (this.mockEnabled) {
				return new LocalUserDataRequestMock<T>();
			} else {
				return new LocalUserDataRequest<T>();
			}
		}
	}
}

