//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by Speakeasy (https://speakeasy.com). DO NOT EDIT.
//
// Changes to this file may cause incorrect behavior and will be lost when
// the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
namespace Fastpix.Models.Components
{
    using Fastpix.Models.Components;
    using Fastpix.Utils;
    using Newtonsoft.Json;
    
    /// <summary>
    /// A collection of Playback ID objects utilized for crafting HLS playback urls.
    /// </summary>
    public class PlaybackId
    {

        /// <summary>
        /// A unique identifier is generated by FastPix for the playbacks.
        /// </summary>
        [JsonProperty("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Determines if access to the streamed content is kept private or available to all.
        /// </summary>
        [JsonProperty("accessPolicy")]
        public PlaybackIdAccessPolicy? AccessPolicy { get; set; }

        /// <summary>
        /// Controls access based on domains and user agents. Defines a default policy (either &quot;allow&quot; or &quot;deny&quot;) and provides lists for explicitly allowed or denied domains and user agents.
        /// </summary>
        [JsonProperty("accessRestrictions")]
        public PlaybackIdAccessRestrictions? AccessRestrictions { get; set; }
    }
}