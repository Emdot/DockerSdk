﻿using System;
using System.Text;
using DockerSdk.Registries.Dto;
using Newtonsoft.Json;

namespace DockerSdk.Registries
{
    /// <summary>
    /// Holds authentication information for a Docker registry.
    /// </summary>
    internal class RegistryEntry
    {
        public RegistryEntry(RegistryReference server)
        {
            Server = server;
            encodedAuthData = Encode(new AuthConfig());
        }

        /// <summary>
        /// Gets or sets the auth object that the core API uses for authentication.
        /// </summary>
        public AuthConfig AuthObject
        {
            get => Decode(encodedAuthData);
            set => encodedAuthData = Encode(value);
        }

        public bool IsAnonymous { get; set; }

        public string Server { get; }

        private string encodedAuthData;

        /// <summary>
        /// De-obfuscates an auth object.
        /// </summary>
        /// <param name="codedForm">An obsfucated, serialized form of the given auth details.</param>
        /// <returns>An object holding the auth details.</returns>
        private static AuthConfig Decode(string codedForm)
        {
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(codedForm));
            return JsonConvert.DeserializeObject<AuthConfig>(json)!;
        }

        /// <summary>
        /// Obsfucates the auth object. This doesn't add much real security, but at least the passwords won't be plainly
        /// visible in memory dumps.
        /// </summary>
        /// <param name="auth">An object holding the auth details.</param>
        /// <returns>An obsfucated, serialized form of the given auth details.</returns>
        private static string Encode(AuthConfig auth)
        {
            var json = JsonConvert.SerializeObject(auth);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        }
    }
}
