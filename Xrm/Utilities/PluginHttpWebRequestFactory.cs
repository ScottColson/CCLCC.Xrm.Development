﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCLLC.Telemetry;

namespace CCLLC.Xrm.Sdk.Utilities
{
    public class PluginHttpWebRequestFactory : IPluginWebRequestFactory
    {
        public IPluginWebRequest BuildPluginWebRequest(Uri address, ITelemetryFactory telemetryFactory = null, ITelemetryClient telemetryClient = null)
        {
            return new PluginHttpWebRequest(address, telemetryFactory, telemetryClient);
        }
    }
}
