namespace ArbitralSystem.Connectors.Core.Models
{
    public interface IOrderbookEntry
    {
        decimal Quantity { get; }

        decimal Price { get; }
    }
}