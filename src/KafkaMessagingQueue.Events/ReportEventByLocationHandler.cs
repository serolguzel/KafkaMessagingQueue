﻿using MassTransit.KafkaIntegration;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using KafkaMessagingQueue.Messages.Events;
using KafkaMessagingQueue.Messages.Models;
using KafkaMessagingQueue.Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaMessagingQueue.Events
{
    public class ReportEventByLocationHandler : INotificationHandler<ReportEventByLocation>
    {
        private readonly DatabaseContext context;
        private readonly ITopicProducer<ReportByLocationPreparing> preparingProducer;
        private readonly ITopicProducer<ReportByLocationCompleted> completedProducer;
        public ReportEventByLocationHandler(DatabaseContext context, ITopicProducer<ReportByLocationPreparing> preparingProducer, ITopicProducer<ReportByLocationCompleted> completedProducer)
        {
            this.context = context;
            this.preparingProducer = preparingProducer;
            this.completedProducer = completedProducer;
        }

        public Task Handle(ReportEventByLocation notification, CancellationToken cancellationToken)
        {
            var reportId = Guid.NewGuid();
            preparingProducer.Produce(new ReportByLocationPreparing
            {
                ReportId = reportId,
                Name = $"{notification.Location} {nameof(ReportEventByLocation)}",
                CreateBy = "SYSTEM",
            });

            var guides = context.Guides.Include(x => x.Contacts)
                .Where(x => x.Contacts.Any(y => y.Value == notification.Location));

            var contacts = guides.SelectMany(x => x.Contacts);
            var report = new ReportLocationModel
            {
                Loaction = notification.Location,
                TotalGuide = guides.Count(),
                TotalPhone = contacts.Count(y => y.ContactType == Domain.ContactType.PHONE),
            };


            if (guides.Any())
                completedProducer.Produce(new ReportByLocationCompleted
                {
                    ReportId = reportId,
                    Name = $"{notification.Location} {nameof(ReportEventByLocation)}",
                    CreateBy = "SYSTEM",
                    Data = JsonConvert.SerializeObject(report)
                });
            return Task.CompletedTask;
        }
    }
}
