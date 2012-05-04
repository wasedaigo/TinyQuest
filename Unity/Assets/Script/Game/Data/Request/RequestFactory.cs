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
				if (Config.IsMockEnabled) {
					this.masterDataRequest = new MasterDataRequestMock();
				} else {
					this.masterDataRequest = new MasterDataRequest();
				}
			}
			
			return this.masterDataRequest;
		}
		
		public LocalUserDataRequest GetUserDataRequest() 
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

