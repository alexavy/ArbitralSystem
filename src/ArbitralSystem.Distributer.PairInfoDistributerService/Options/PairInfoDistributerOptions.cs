namespace ArbitralSystem.Distributer.PairInfoDistributerService.Options
{
    public class PairInfoDistributerOptions : IPairInfoDistributerOptions
    {
        public SiftType SiftType { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}