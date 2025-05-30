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
    /// Displays the result of the request.
    /// </summary>
    public class DirectUpload
    {

        /// <summary>
        /// When creating the upload, FastPix assigns a universally unique identifier with a maximum length of 255 characters.
        /// </summary>
        [JsonProperty("id")]
        public string? Id { get; set; }

        /// <summary>
        /// When creating the media, FastPix assigns a universally unique identifier with a maximum length of 255 characters.
        /// </summary>
        [JsonProperty("mediaId")]
        public string? MediaId { get; set; }

        /// <summary>
        /// Determines the media&apos;s status, which can be one of the possible values.
        /// </summary>
        [JsonProperty("status")]
        public string? Status { get; set; }

        /// <summary>
        /// The url hosts the media file for FastPix, which needs to be download to use further.  It supports formats like MP3, MP4, MOV, MKV, or TS, and includes text tracks for subtitles/CC (SRT file/VTT file). While FastPix can handle various audio and video formats and codecs, using standard inputs can help with optimal processing speed.
        /// </summary>
        [JsonProperty("url")]
        public string? Url { get; set; }

        /// <summary>
        /// The duration set for the validity of the upload URL. If the upload isn&apos;t completed within this timeframe, it&apos;s marked as timed out.<br/>
        /// 
        /// <remarks>
        /// 
        /// </remarks>
        /// </summary>
        [JsonProperty("timeout")]
        public double? Timeout { get; set; } = 14400D;

        /// <summary>
        /// Upload media directly from a device using the url name or enter &apos;*&apos; to allow all.
        /// </summary>
        [JsonProperty("corsOrigin")]
        public string? CorsOrigin { get; set; }

        [JsonProperty("pushMediaSettings")]
        public DirectUploadResponse? PushMediaSettings { get; set; }
    }
}