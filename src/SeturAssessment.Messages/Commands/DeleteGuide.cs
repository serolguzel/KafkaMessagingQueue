﻿using MediatR;
using SeturAssessment.Messages.Models;
using System;

namespace SeturAssessment.Messages.Commands
{
    public record DeleteGuide(Guid Id) : IRequest<CommandResponse<Guid>>;
}
