namespace AndroCloudWCFServices.Tools
{
    using System;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    public class CloudRoleNameInitializer : ITelemetryInitializer
    {
        private readonly string _roleName;

        public CloudRoleNameInitializer(string roleName)
        {
            this._roleName = roleName ?? throw new ArgumentNullException(nameof(roleName));
        }

        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Cloud.RoleName = _roleName;
            telemetry.Context.Cloud.RoleInstance = _roleName;
        }
    }
}