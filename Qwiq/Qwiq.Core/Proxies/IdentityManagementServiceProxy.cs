﻿using System.Collections.Generic;
using System.Linq;
using Tfs = Microsoft.TeamFoundation.Framework;

namespace Microsoft.IE.Qwiq.Proxies
{
    public class IdentityManagementServiceProxy : IIdentityManagementService
    {
        private readonly Tfs.Client.IIdentityManagementService2 _identityManagementService2;

        internal IdentityManagementServiceProxy(Tfs.Client.IIdentityManagementService2 identityManagementService2)
        {
            _identityManagementService2 = identityManagementService2;
        }

        public IEnumerable<ITeamFoundationIdentity> ReadIdentities(IEnumerable<IIdentityDescriptor> descriptors)
        {
            var rawDescriptors = descriptors.Select(descriptor =>
                new Tfs.Client.IdentityDescriptor(descriptor.IdentityType, descriptor.Identifier)).ToArray();

            var identities = _identityManagementService2.ReadIdentities(rawDescriptors, Tfs.Common.MembershipQuery.None,
                Tfs.Common.ReadIdentityOptions.None);

            return identities.Select(item => new TeamFoundationIdentityProxy(item));
        }

        public IEnumerable<ITeamFoundationIdentity> ReadIdentities(IdentitySearchFactor searchFactor, string[] searchFactorValues)
        {
            var factor = (Tfs.Common.IdentitySearchFactor) searchFactor;
            var identities = _identityManagementService2.ReadIdentities(factor, searchFactorValues,
                Tfs.Common.MembershipQuery.None, Tfs.Common.ReadIdentityOptions.None)[0];

            return identities.Select(item => new TeamFoundationIdentityProxy(item));
        }

        public IIdentityDescriptor CreateIdentityDescriptor(string identityType, string identifier)
        {
            return new IdentityDescriptorProxy(new Tfs.Client.IdentityDescriptor(identityType, identifier));
        }
    }
}