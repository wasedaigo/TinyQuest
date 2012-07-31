using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Data;

namespace TinyQuest.Data.Request {
	public class RequestFactory {
		public static readonly RequestFactory Instance = new RequestFactory();
		private RequestFactory(){}
		
		private MasterDataRequest masterDataRequest;
		private LocalUserDataRequest localUserDataRequest;
		
		
		public MasterDataRequest GetMasterDataRequest() 
		{
			if (this.masterDataRequest == null) {
				this.masterDataRequest = new MasterDataRequestMock();
			}
			
			return this.masterDataRequest;
		}
		
		public LocalUserDataRequest GetLocalUserRequest() 
		{
			if (this.localUserDataRequest == null) {
				if (Config.IsMockEnabled) {
					this.localUserDataRequest = new LocalUserDataRequestMock();
				} else {
					this.localUserDataRequest = new LocalUserDataRequest();
				}
			}
			
			return this.localUserDataRequest;
		}
	}
}

