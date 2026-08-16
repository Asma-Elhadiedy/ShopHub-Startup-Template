
namespace myshop.Web.Permissions;

internal class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : base(options) { }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);
        if (policy != null) return policy;

        if (policyName.StartsWith(ConstPermissions.Permission, StringComparison.OrdinalIgnoreCase))
        {
            var policyBuilder = new AuthorizationPolicyBuilder();
            policyBuilder.AddRequirements(new PermissionRequirement(policyName));
            return (AuthorizationPolicy?)(policyBuilder.Build());
        }

        return null;
    }
}
