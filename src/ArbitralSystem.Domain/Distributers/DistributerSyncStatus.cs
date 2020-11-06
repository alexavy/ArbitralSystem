namespace ArbitralSystem.Domain.Distributers
{
    public enum DistributerSyncStatus
    {
        //
        // Summary:
        //     Not connected
        Disconnected = 0,
        //
        // Summary:
        //     Connecting
        Connecting = 1,
        //
        // Summary:
        //     Syncing data
        Syncing = 2,
        //
        // Summary:
        //     Data synced, order book is up to date
        Synced = 3
    }
}