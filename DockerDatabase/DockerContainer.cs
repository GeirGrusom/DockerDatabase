// Original filename: DockerContainer.cs
// Author: Henning Moe
// This file is licensed under MIT.
// The full license is available under the root in a file names LICENSE.

namespace DockerDatabase
{
    using System.Threading;
    using System.Threading.Tasks;
    using Docker.DotNet;

    /// <summary>
    /// This class is a wrapper for a stoppable docker container
    /// </summary>
    internal sealed class DockerContainer
    {
        private readonly DockerClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="DockerContainer"/> class.
        /// </summary>
        /// <param name="id">Id of running docker container</param>
        /// <param name="dockerClient">Client used to connect to Docker daemon</param>
        public DockerContainer(string id, DockerClient dockerClient)
        {
            this.Id = id;
            this.client = dockerClient;
        }

        /// <summary>
        /// Gets the Id associated with this container
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Asynchronously stops the container
        /// </summary>
        /// <param name="cancel">Cancellation token used to cancel process</param>
        /// <returns>Asynchronous task</returns>
        public async Task StopAsync(CancellationToken cancel = default(CancellationToken))
        {
            await this.client.Containers.StopContainerAsync(this.Id, new Docker.DotNet.Models.ContainerStopParameters(), cancel);
            await this.client.Containers.WaitContainerAsync(this.Id, cancel);
        }
    }
}
