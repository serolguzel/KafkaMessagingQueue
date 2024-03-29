﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using KafkaMessagingQueue.Messages.Events;
using KafkaMessagingQueue.Messages.Models;
using KafkaMessagingQueue.Messages.Queries;
using System.Threading.Tasks;

namespace KafkaMessagingQueue.Api.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IMediator mediator;

        public ReportController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public Task<string> Get([FromQuery] ReportEventByLocation request)
        {
            mediator.Publish(request);
            return Task.Run(() => "Talebiniz işleme alınmıştır.");
        }

        [HttpGet("GetReports")]
        public Task<ReportModel[]> Get([FromQuery] GetReports request) => mediator.Send(request);

        [HttpGet("{Id:guid}")]
        public Task<ReportModel> Get([FromRoute] GetReportById request) => mediator.Send(request);
    }
}
