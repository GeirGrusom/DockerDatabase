// Original filename: DockerContainerBuilder.cs
// Author: Henning Moe
// This file is licensed under MIT.
// The full license is available under the root in a file names LICENSE.

namespace DockerDatabase
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Docker.DotNet;
    using Docker.DotNet.Models;

    /// <summary>
    /// This class is used to build a DockerContainer for database engines
    /// </summary>
    internal sealed class DockerContainerBuilder
    {
        private readonly string baseImage;
        private readonly string tag;
        private readonly DockerClient client;
        private readonly string exposedPort;
        private readonly string[] environmentVariables;

        /// <summary>
        /// Initializes a new instance of the <see cref="DockerContainerBuilder"/> class.
        /// </summary>
        /// <param name="baseImage">Image to base container on</param>
        /// <param name="tag">Tag to use for this container</param>
        /// <param name="exposedPort">The port to expose</param>
        /// <param name="environmentVariables">Environment variables to pass to the image</param>
        public DockerContainerBuilder(string baseImage, string tag, int exposedPort, params string[] environmentVariables)
        {
            this.baseImage = baseImage;
            this.tag = tag;
            this.client = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();
            this.exposedPort = exposedPort.ToString();
            this.environmentVariables = environmentVariables;
        }

        /// <summary>
        /// Starts the database container
        /// </summary>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Asynchronous task</returns>
        public async Task<DockerContainer> Start(CancellationToken cancel = default(CancellationToken))
        {
            await this.client.Images.CreateImageAsync(
                new ImagesCreateParameters
                {
                    FromImage = this.baseImage,
                    Tag = this.tag,
                },
                new AuthConfig(),
                new Progress(),
                cancel);

            var cfg = new Config
            {
                Image = $"{this.baseImage}:{this.tag}",
                Env = this.environmentVariables,
                ExposedPorts = new Dictionary<string, EmptyStruct>
                {
                    [this.exposedPort] = default(EmptyStruct),
                },
            };

            var hostConfig = new HostConfig
            {
                PortBindings = new Dictionary<string, IList<PortBinding>>
                {
                    [this.exposedPort] = new[]
                    {
                        new PortBinding { HostPort = this.exposedPort, HostIP = "127.0.0.1" },
                    },
                },
            };

            var containerCreated = await this.client.Containers.CreateContainerAsync(new CreateContainerParameters(cfg) { HostConfig = hostConfig }, cancel);

            await this.client.Containers.StartContainerAsync(containerCreated.ID, new ContainerStartParameters(), cancel);

            var inspectResult = await this.client.Containers.InspectContainerAsync(containerCreated.ID, cancel);

            return new DockerContainer(containerCreated.ID, this.client);
        }

        private sealed class Progress : IProgress<JSONMessage>
        {
            public void Report(JSONMessage value)
            {
            }
        }
    }
}
