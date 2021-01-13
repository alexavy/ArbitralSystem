using System;
using System.Collections.Generic;
using System.Text;

namespace ArbitralSystem.Messaging.Options
{
    public static class Constants
    {
        public static class Queues
        {
            public static string MQOrderBookDistributorPrefix = "mq_obd";
            public static string MQOrderBookDistributorCancellationPrefix = "mq_obd_cancellation_";
            public static string MQManagerConsumer = "mq_manager";
            public static string MQManagerHeartBeatConsumer = "mq_manager_heartbeat";
            public static string OrderBookAggregatorConsumer = "orderbook_aggregator_consumer";
            public static string OrderbooksStorageConsumer = "orderbooks-storage_consumer";
            public static string DistributerConnectionStatesConsumer = "distributer_connection_states_consumer";
        }
    }
}
