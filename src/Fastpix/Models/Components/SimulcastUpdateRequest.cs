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
    
    public class SimulcastUpdateRequest
    {

        /// <summary>
        /// When the value is set to false, the simulcast will be disabled for the given stream.
        /// </summary>
        [JsonProperty("isEnabled")]
        public bool? IsEnabled { get; set; }

        /// <summary>
        /// Arbitrary user-supplied metadata that will be included in the simulcast details. Can be used to store your own ID for a video along with the simulcast. Max:255 characters, Upto 10 entries are allowed.
        /// </summary>
        [JsonProperty("metadata")]
        public SimulcastUpdateRequestMetadata? Metadata { get; set; }
    }
}