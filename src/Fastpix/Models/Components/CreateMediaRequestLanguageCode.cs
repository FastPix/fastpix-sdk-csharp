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
    /// Language codes are concise, standardized symbols that denote languages, utilizing either two or three characters for identification. The language code must be compliant with the BCP 47 standard to ensure compatibility. (for text only).<br/>
    /// 
    /// <remarks>
    /// 
    /// </remarks>
    /// </summary>
    public enum CreateMediaRequestLanguageCode
    {
        [JsonProperty("en")]
        En,
        [JsonProperty("it")]
        It,
        [JsonProperty("pl")]
        Pl,
        [JsonProperty("es")]
        Es,
        [JsonProperty("fr")]
        Fr,
        [JsonProperty("ru")]
        Ru,
        [JsonProperty("nl")]
        Nl,
    }

    public static class CreateMediaRequestLanguageCodeExtension
    {
        public static string Value(this CreateMediaRequestLanguageCode value)
        {
            return ((JsonPropertyAttribute)value.GetType().GetMember(value.ToString())[0].GetCustomAttributes(typeof(JsonPropertyAttribute), false)[0]).PropertyName ?? value.ToString();
        }

        public static CreateMediaRequestLanguageCode ToEnum(this string value)
        {
            foreach(var field in typeof(CreateMediaRequestLanguageCode).GetFields())
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

                    if (enumVal is CreateMediaRequestLanguageCode)
                    {
                        return (CreateMediaRequestLanguageCode)enumVal;
                    }
                }
            }

            throw new Exception($"Unknown value {value} for enum CreateMediaRequestLanguageCode");
        }
    }

}