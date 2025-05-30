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
    using Fastpix.Utils;
    
    public class GetLiveStreamPlaybackIdRequest
    {

        /// <summary>
        /// Upon creating a new live stream, FastPix assigns a unique identifier to the stream.
        /// </summary>
        [SpeakeasyMetadata("pathParam:style=simple,explode=false,name=streamId")]
        public string StreamId { get; set; } = default!;

        /// <summary>
        /// Upon creating a new playbackId, FastPix assigns a unique identifier to the playback.
        /// </summary>
        [SpeakeasyMetadata("pathParam:style=simple,explode=false,name=playbackId")]
        public string PlaybackId { get; set; } = default!;
    }
}