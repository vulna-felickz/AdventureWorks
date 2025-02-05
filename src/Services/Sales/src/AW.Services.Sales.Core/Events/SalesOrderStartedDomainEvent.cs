﻿using MediatR;
using System;

namespace AW.Services.Sales.Core.Events
{
    public class SalesOrderStartedDomainEvent : INotification
    {
        public string UserId { get; }
        public string UserName { get; }
        public string CardType { get; }
        public string CardNumber { get; }
        public string CardSecurityNumber { get; }
        public string CardHolderName { get; }
        public DateTime CardExpiration { get; }
        public Entities.SalesOrder SalesOrder { get; }

        public SalesOrderStartedDomainEvent(Entities.SalesOrder salesOrder, string userId, string userName,
            string cardType, string cardNumber,
            string cardSecurityNumber, string cardHolderName,
            DateTime cardExpiration
        )
        {
            SalesOrder = salesOrder;
            UserId = userId;
            UserName = userName;
            CardType = cardType;
            CardNumber = cardNumber;
            CardSecurityNumber = cardSecurityNumber;
            CardHolderName = cardHolderName;
            CardExpiration = cardExpiration;
        }
    }
}