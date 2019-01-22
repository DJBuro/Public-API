namespace AndroCloudWCFServices.Tools
{
    using System;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    public class CloudRoleNameInitializer : ITelemetryInitializer
    {
        private readonly string roleName;

        public CloudRoleNameInitializer(string roleName)
        {
            this.roleName = roleName ?? throw new ArgumentNullException(nameof(roleName));
        }

        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Cloud.RoleName = roleName;
            telemetry.Context.Cloud.RoleInstance = roleName;
        }
    }
}