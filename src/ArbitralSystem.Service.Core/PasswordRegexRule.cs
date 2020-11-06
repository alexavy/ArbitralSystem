using System;

namespace ArbitralSystem.Service.Core
{
    public class PasswordRegexRule
    {
        public string RegexPattern { get; set; }
        public Type ObjectType { get; set; }
        public string NameOfProperty { get; set; }
    }
}