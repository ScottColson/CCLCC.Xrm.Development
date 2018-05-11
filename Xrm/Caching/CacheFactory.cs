﻿using System;

namespace CCLCC.Xrm.Caching
{
    public class CacheFactory : ICacheFactory
    {
        public IXrmCache CreateOrganizationCache(Guid organizationId)
        {
            return new XrmOrganizationCache(organizationId);
        }

        public IXrmCache CreatePluginCache()
        {
            return new XrmPluginCache();
        }
    }
}