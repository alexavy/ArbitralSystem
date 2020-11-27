namespace ArbitralSystem.Domain.Distributers
{
    public enum DistributerSyncStatus
    {
        /// <summary>Not connected</summary>
        Disconnected=0,
        /// <summary>Connecting</summary>
        Connecting=1,
        /// <summary>Reconnecting</summary>
        Reconnecting=2,
        /// <summary>Syncing data</summary>
        Syncing=3,
        /// <summary>Data synced, order book is up to date</summary>
        Synced=4,
    }
}