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
    using Newtonsoft.Json;
    using System;
    
    /// <summary>
    /// Specifies the default access policy for user agents (browsers, bots, etc.). <br/>
    /// 
    /// <remarks>
    /// If set to `allow`, all user agents are allowed access unless otherwise specified in the `deny` list. <br/>
    /// If set to `deny`, all user agents are denied access unless otherwise specified in the `allow` list.<br/>
    /// 
    /// </remarks>
    /// </summary>
    public enum DirectUploadVideoMediaUserAgentsDefaultPolicy
    {
        [JsonProperty("allow")]
        Allow,
        [JsonProperty("deny")]
        Deny,
    }

    public static class DirectUploadVideoMediaUserAgentsDefaultPolicyExtension
    {
        public static string Value(this DirectUploadVideoMediaUserAgentsDefaultPolicy value)
        {
            return ((JsonPropertyAttribute)value.GetType().GetMember(value.ToString())[0].GetCustomAttributes(typeof(JsonPropertyAttribute), false)[0]).PropertyName ?? value.ToString();
        }

        public static DirectUploadVideoMediaUserAgentsDefaultPolicy ToEnum(this string value)
        {
            foreach(var field in typeof(DirectUploadVideoMediaUserAgentsDefaultPolicy).GetFields())
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

                    if (enumVal is DirectUploadVideoMediaUserAgentsDefaultPolicy)
                    {
                        return (DirectUploadVideoMediaUserAgentsDefaultPolicy)enumVal;
                    }
                }
            }

            throw new Exception($"Unknown value {value} for enum DirectUploadVideoMediaUserAgentsDefaultPolicy");
        }
    }

}