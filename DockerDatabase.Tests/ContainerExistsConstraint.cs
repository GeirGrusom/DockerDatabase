// Original filename: ContainerExistsConstraint.cs
// Author: Henning Moe
// This file is licensed under MIT.
// The full license is available under the root in a file names LICENSE.

namespace DockerDatabase.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Docker.DotNet;
    using Docker.DotNet.Models;
    using NUnit.Framework.Constraints;

    /// <summary>
    /// Checks whether a container exists in Docker or not
    /// </summary>
    public class ContainerExistsConstraint : Constraint
    {
        /// <inheritdoc/>
        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            string id;
            if (actual is DatabaseContainer dc)
            {
                id = dc.Id;
            }
            else if (actual is string s)
            {
                id = s;
            }
            else
            {
                throw new NotSupportedException();
            }

            var client = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();

            try
            {
                var task = client.Containers.GetContainerStatsAsync(id, new ContainerStatsParameters(), new NullProgress(), CancellationToken.None);
                Task.Run(() => task);
            }
            catch (DockerContainerNotFoundException)
            {
                return new ConstraintResult(this, actual, false);
            }

            return new ConstraintResult(this, actual, true);
        }

        private class NullProgress : IProgress<JSONMessage>
        {
            public void Report(JSONMessage value)
            {
            }
        }
    }
}
