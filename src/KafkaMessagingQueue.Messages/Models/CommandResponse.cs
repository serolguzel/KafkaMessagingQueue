﻿namespace KafkaMessagingQueue.Messages.Models
{
    public class CommandResponse<T>
    {
        public T Aggregate { get; set; }
    }
}
