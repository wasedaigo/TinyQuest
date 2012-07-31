using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Data;

namespace TinyQuest.Data.Cache {
	public class CacheFactory {
		public static readonly CacheFactory Instance = new CacheFactory();
		private CacheFactory(){}
		
		private MasterDataCache masterDataCache;
		private LocalUserDataCache localUserDataCache;
		private LocalizedTextCache localizedTextCache;
		
		public MasterDataCache GetMasterDataCache() 
		{
			if (this.masterDataCache == null) {
				this.masterDataCache = new MasterDataCacheMock();
			}
			
			return this.masterDataCache;
		}
		
		
		public LocalUserDataCache GetLocalUserDataCache()
		{
			if (this.localUserDataCache == null) {
				this.localUserDataCache = new LocalUserDataCache();
			}
			
			return this.localUserDataCache;
		}
		
		
		public LocalizedTextCache GetLocalizedTextCache()
		{
			if (this.localizedTextCache == null) {
				if (Config.IsMockEnabled) {
					this.localizedTextCache = new LocalizedTextCacheMock();
				} else {
					this.localizedTextCache = new LocalizedTextCache();
				}
			}
			
			return this.localizedTextCache;
		}
	}
}

