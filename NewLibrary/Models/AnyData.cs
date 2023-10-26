namespace NewLibrary.Models
{
    public class AnyData
    {
        public TimeSpan RandomTime { get; set; }
        public string RandomValue { get; set; }
        public int Count { get; set; }

        new public string ToString()
        {
            return $"{RandomTime} | {RandomValue} | {Count}";
        }
    }
}
