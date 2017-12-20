// Original filename: Container.cs
// Author: Henning Moe
// This file is licensed under MIT.
// The full license is available under the root in a file names LICENSE.

namespace DockerDatabase.Tests
{
    /// <summary>
    /// This class is used to assert something about Docker containers
    /// </summary>
    public static class Container
    {
        /// <summary>
        /// Gets a constraint for whether a container exists or not.
        /// </summary>
        public static ContainerExistsConstraint Exists
        {
            get
            {
                return new ContainerExistsConstraint();
            }
        }
    }
}
