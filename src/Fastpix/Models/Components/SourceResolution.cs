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
    using Fastpix.Utils;
    using Newtonsoft.Json;
    using System;
    
    /// <summary>
    /// The actual resolution of the uploaded media. This represents the native quality of the source media.
    /// </summary>
    public enum SourceResolution
    {
        [JsonProperty("2160p")]
        TwoThousandOneHundredAndSixtyp,
        [JsonProperty("1440p")]
        OneThousandFourHundredAndFortyp,
        [JsonProperty("1080p")]
        OneThousandAndEightyp,
        [JsonProperty("720p")]
        SevenHundredAndTwentyp,
        [JsonProperty("480p")]
        FourHundredAndEightyp,
        [JsonProperty("360p")]
        ThreeHundredAndSixtyp,
    }

    public static class SourceResolutionExtension
    {
        public static string Value(this SourceResolution value)
        {
            return ((JsonPropertyAttribute)value.GetType().GetMember(value.ToString())[0].GetCustomAttributes(typeof(JsonPropertyAttribute), false)[0]).PropertyName ?? value.ToString();
        }

        public static SourceResolution ToEnum(this string value)
        {
            foreach(var field in typeof(SourceResolution).GetFields())
            {
                var attributes = field.GetCustomAttributes(typeof(JsonPropertyAttribute), false);
                if (attributes.Length == 0)
                {
                    continue;
                }

                var attribute = attributes[0] as JsonPropertyAttribute;
                if (attribute != null && attribute.PropertyName == value)
                {
                    var enumVal = field.GetValue(null);

                    if (enumVal is SourceResolution)
                    {
                        return (SourceResolution)enumVal;
                    }
                }
            }

            throw new Exception($"Unknown value {value} for enum SourceResolution");
        }
    }

}