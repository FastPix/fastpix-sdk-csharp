//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by Speakeasy (https://speakeasy.com). DO NOT EDIT.
//
// Changes to this file may cause incorrect behavior and will be lost when
// the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
namespace Fastpix.Models.Requests
{
    using Fastpix.Models.Components;
    using Fastpix.Utils;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    
    /// <summary>
    /// Displays the result of the request.
    /// </summary>
    public class Data
    {

        /// <summary>
        /// A collection of Playback ID objects utilized for crafting HLS playback URLs.
        /// </summary>
        [JsonProperty("playbackIds")]
        public List<PlaybackId>? PlaybackIds { get; set; }
    }
}