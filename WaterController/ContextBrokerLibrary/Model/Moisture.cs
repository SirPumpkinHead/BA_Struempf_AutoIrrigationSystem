namespace ContextBrokerLibrary.Model
{
    public class Moisture
    {
        public string Type { get; set; }
        public string Value { get; set; }

        public int? GetValue()
        {
            return int.Parse(Value);
        }
    }
}