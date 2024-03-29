﻿using FluentValidation;
using MediatR;
using KafkaMessagingQueue.Messages.Models;
using System;

namespace KafkaMessagingQueue.Messages.Commands
{
    public class AddContact : IRequest<CommandResponse<Guid>>
    {
        public Guid GuideId { get; set; }
        public ContactTypeEnum ContactType { get; set; }
        public string CreateBy { get; set; }
        public string Value { get; set; }
    }

    public class AddContactValidator : AbstractValidator<AddContact>
    {
        public AddContactValidator()
        {
            RuleFor(x => x.ContactType).NotEmpty();
            RuleFor(x => x.Value).NotEmpty();
            RuleFor(x => x.Value).NotEmpty()
                .EmailAddress()
                .When(x => x.ContactType == ContactTypeEnum.EMAIL);

            RuleFor(x => x.Value).NotEmpty()
                .Must(x => x.IsPhoneNumber())
                .When(x => x.ContactType == ContactTypeEnum.PHONE);
        }
    }
}
