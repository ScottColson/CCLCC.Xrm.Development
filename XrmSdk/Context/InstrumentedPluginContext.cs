﻿using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using CCLLC.Core;
using CCLLC.Telemetry;
using CCLLC.Xrm.Sdk.Utilities;

namespace CCLLC.Xrm.Sdk.Context
{
    public class InstrumentedPluginContext : InstrumentedContext, ILocalPluginContext
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public virtual IPluginExecutionContext PluginExecutionContext { get { return (IPluginExecutionContext)base.ExecutionContext; } }

        public virtual ePluginStage Stage { get { return (ePluginStage)this.PluginExecutionContext.Stage; } }

        /// <summary>
        /// Returns the first registered 'Pre' image for the pipeline execution
        /// </summary>
        public virtual Entity PreImage
        {
            get
            {
                if (this.PluginExecutionContext.PreEntityImages.Any())
                {
                    return this.PluginExecutionContext.PreEntityImages[this.PluginExecutionContext.PreEntityImages.FirstOrDefault().Key];
                }
                return null;
            }
        }

        /// <summary>
        /// Returns the first registered 'Post' image for the pipeline execution
        /// </summary>
        public virtual Entity PostImage
        {
            get
            {
                if (this.PluginExecutionContext.PostEntityImages.Any())
                {
                    return this.PluginExecutionContext.PostEntityImages[this.PluginExecutionContext.PostEntityImages.FirstOrDefault().Key];
                }
                return null;
            }
        }

        private Entity _preMergedTarget = null;
        /// <summary>
        /// Returns an Entity record with all attributes from the current inbound target and any additional attributes
        /// that might exist in the Pre Image entity if provided. PreMergedTarget is cached so future calls
        /// return the same entity object and will not reflect changes made to that Target since initial
        /// request.
        /// </summary>
        public virtual Entity PreMergedTarget
        {
            get
            {
                if (_preMergedTarget == null)
                {
                    _preMergedTarget = new Entity(this.PluginExecutionContext.PrimaryEntityName);
                    _preMergedTarget.Id = this.PluginExecutionContext.PrimaryEntityId;
                    _preMergedTarget.MergeWith(this.TargetEntity);
                    _preMergedTarget.MergeWith(this.PreImage);
                }

                return _preMergedTarget;
            }
        }

        protected internal InstrumentedPluginContext(IServiceProvider serviceProvider, IIocContainer container, IPluginExecutionContext executionContext, IComponentTelemetryClient telemetryClient)
            : base(executionContext, container, telemetryClient)
        {
            this.ServiceProvider = serviceProvider;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._preMergedTarget = null;
                this.ServiceProvider = null;
            }
            
            base.Dispose(disposing);
        }

        protected override IOrganizationServiceFactory CreateOrganizationServiceFactory()
        {
            return (IOrganizationServiceFactory)this.ServiceProvider.GetService(typeof(IOrganizationServiceFactory));
        }

        protected override ITracingService CreateTracingService()
        {
            return (ITracingService)this.ServiceProvider.GetService(typeof(ITracingService));
        }
    }

}

