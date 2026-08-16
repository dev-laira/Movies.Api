namespace Movies.Api.Auth
{
    public static class AuthConstants
    {
        public const string AdminPolicyName = "Admin";
        public const string TrustedMemberPolicyName = "TrustedMember";
        public const string AdminUserClaimName = "admin";
        public const string TrusterMemberUserClaimName = "trusted_member";
    }
}
