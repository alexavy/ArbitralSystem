namespace ArbitralSystem.Domain.Distributers
{
    public enum DistributerSyncStatus
    {
        /// Not connected
        Disconnected=0,
        /// Connecting
        Connecting=1,
        /// Reconnecting
        Reconnecting=2,
        /// Syncing data
        Syncing=3,
        /// Data synced, order book is up to date
        Synced=4,
    }
}