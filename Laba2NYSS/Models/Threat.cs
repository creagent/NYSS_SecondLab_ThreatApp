using Newtonsoft.Json;
using System;

namespace Laba2NYSS.Models
{
    class Threat
    {
        private const string YES = "Да";
        private const string NO = "Нет";

        private string _privacyViolation;
        private string _integrityViolation;
        private string _accessViolation;
        private string _id;

        public Threat()
        {

        }
        public Threat(string id, string name, string description, string source, string @object, 
            string privacyViolation, string integrityViolation, string accessViolation)
        {
            Id = id;
            Name = name;
            Description = description;
            Source = source;
            Object = @object;
            PrivacyViolation = privacyViolation;
            IntegrityViolation = integrityViolation;
            AccessViolation = accessViolation;
        }

        [JsonProperty(PropertyName = "latestUpdateDate")]
        public static DateTime LatestUpdateDate { get; private set; }

        public static void SetLatestUpdateDate()
        {
            LatestUpdateDate = DateTime.Now;
        }

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get => _id;
            private set
            {
                if (!value.Contains("УБИ."))
                {
                    _id = "УБИ." + value;
                }
                else
                {
                    _id = value;
                }
            }
        }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; private set; }

        [JsonProperty(PropertyName = "source")]
        public string Source { get; private set; }

        [JsonProperty(PropertyName = "object")]
        public string Object { get; private set; }

        [JsonProperty(PropertyName = "privacy_violation")]
        public string PrivacyViolation
        {
            get
            {
                return _privacyViolation;
            }
            private set
            {
                int temp;
                if (int.TryParse(value, out temp))
                {
                    if (int.Parse(value) > 0)
                    {
                        _privacyViolation = YES;
                    }
                    else
                    {
                        _privacyViolation = NO;
                    }
                }
                else
                {
                    _privacyViolation = value;
                }
            }
        }

        [JsonProperty(PropertyName = "integrity_violation")]
        public string IntegrityViolation
        {
            get
            {
                return _integrityViolation;
            }
            private set
            {
                int temp;
                if (int.TryParse(value, out temp))
                {
                    if (int.Parse(value) > 0)
                    {
                        _integrityViolation = YES;
                    }
                    else
                    {
                        _integrityViolation = NO;
                    }
                }
                else
                {
                    _integrityViolation = value;
                }
            }
        }

        [JsonProperty(PropertyName = "access_violation")]
        public string AccessViolation
        {
            get
            {
                return _accessViolation;
            }
            private set
            {
                int temp;
                if (int.TryParse(value, out temp))
                {
                    if (int.Parse(value) > 0)
                    {
                        _accessViolation = YES;
                    }
                    else
                    {
                        _accessViolation = NO;
                    }
                }
                else
                {
                    _accessViolation = value;
                }
            }
        }
    }
}

