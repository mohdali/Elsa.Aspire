using Elsa.Studio.Abstractions;
using Elsa.Studio.Contracts;
using Elsa.Studio.Keycloak.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.Studio.Keycloak
{
    public class Feature(IAppBarService appBarService) : FeatureBase
    {
        public override ValueTask InitializeAsync(CancellationToken cancellationToken = default)
        {
            appBarService.AddAppBarItem<Account>();

            return ValueTask.CompletedTask;
        }
    }
}
