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
    
    public class UpdateSpecificSimulcastOfStreamResponse
    {

        [JsonProperty("-")]
        public HTTPMetadata HttpMeta { get; set; } = default!;

        /// <summary>
        /// Stream&apos;s simulcast details fetched successfully
        /// </summary>
        public SimulcastUpdateResponse? SimulcastUpdateResponse { get; set; }
    }
}