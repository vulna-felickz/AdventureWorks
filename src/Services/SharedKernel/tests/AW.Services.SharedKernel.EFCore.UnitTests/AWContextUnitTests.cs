﻿using AW.Services.SharedKernel.EFCore.UnitTests.TestData;
using AW.SharedKernel.UnitTesting;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AW.Services.SharedKernel.EFCore.UnitTests
{
    public class AWContextUnitTests
    {
        private static ItemsContext CreateContext(DbContextOptions<AWContext> options, IMediator mediator)
            => new(options, mediator);

        [Theory, AutoMoqData]
        public async Task SetModified_EntityStateIsModified(
            Mock<IMediator> mockMediator
        )
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AWContext>()
                .UseInMemoryDatabase(databaseName: nameof(SetModified_EntityStateIsModified))
                .Options;

            using var context = CreateContext(options, mockMediator.Object);

            context.Items.Add(new Item { Name = "Item1" });
            await context.SaveChangesAsync();

            //Act
            var item = context.Items.Single(i => i.Name == "Item1");
            context.SetModified(item);

            //Assert
            context.Entry(item).State.Should().Be(EntityState.Modified);
        }

        [Theory, AutoMoqData]
        public async Task SaveEntitiesAsync_ChangesAreSaved_DomainEventsAreDispatched(
            Mock<IMediator> mockMediator
        )
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AWContext>()
                .UseInMemoryDatabase(databaseName: nameof(SaveEntitiesAsync_ChangesAreSaved_DomainEventsAreDispatched))
                .Options;

            using var context = CreateContext(options, mockMediator.Object);

            //Act
            context.Items.Add(new Item { Name = "Item1" });
            var result = await context.SaveEntitiesAsync();

            //Assert
            result.Should().Be(true);
        }

        [Theory, AutoMoqData]
        public async Task Execute_FuncIsExecuted(
            Mock<IMediator> mockMediator
        )
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AWContext>()
                .UseInMemoryDatabase(databaseName: nameof(Execute_FuncIsExecuted))
                .Options;

            using var context = CreateContext(options, mockMediator.Object);

            //Act
            int result = 0;
            Func<Task> func = () =>
            {
                result = 2 + 2;
                return Task.FromResult(result);
            };
            await context.Execute(func);

            //Assert
            result.Should().Be(4);
        }

        [Theory, AutoMoqData]
        public async Task BeginTransactionAsync_TransactionIsStarted(
            Mock<IMediator> mockMediator
        )
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AWContext>()
                .UseInMemoryDatabase(databaseName: nameof(BeginTransactionAsync_TransactionIsStarted))
                .ConfigureWarnings(config => config.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = CreateContext(options, mockMediator.Object);

            //Act
            Func<Task> func = async() => await context.BeginTransactionAsync();

            //Assert
            await func.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Relational-specific methods can only be used when the context is using a relational database provider.");
        }
    }
}