namespace ArbitralSystem.Service.Core
{
    public static class RegexPasswordPatterns
    {
        public static readonly string RabbitMq = @"(?<=(:\/\/)).*(?=@)";
        public static readonly string Database = @"(?<=((P|p)assword(\s|=))).*(?=;)";
    }
}