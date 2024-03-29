﻿using MediatR;
using Newtonsoft.Json;
using KafkaMessagingQueue.Messages.Models;
using KafkaMessagingQueue.Messages.Queries;
using KafkaMessagingQueue.Services;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaMessagingQueue.Queries
{
    public class GetReportByIdHandler : IRequestHandler<GetReportById, ReportModel>
    {
        private readonly IReportService reportService;

        public GetReportByIdHandler(IReportService reportService)
        {
            this.reportService = reportService;
        }

        public async Task<ReportModel> Handle(GetReportById request, CancellationToken cancellationToken)
        {
            var item = await reportService.GetReportById(request.Id);
            if (item == null)
                return null;
            return new ReportModel
            {
                Id = item.Id,
                Name = item.Name,
                CreateBy = item.CreateBy,
                CreateDate = item.CreateDate,
                Data = JsonConvert.DeserializeObject<ReportLocationModel>(item.Data)
            };
        }
    }
}
