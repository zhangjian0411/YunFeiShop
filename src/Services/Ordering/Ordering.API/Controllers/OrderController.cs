using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ZhangJian.YunFeiShop.Services.Ordering.API.Application.Commands;
using ZhangJian.YunFeiShop.Services.Ordering.Domain.AggregatesModel.OrderAggregate;

namespace ZhangJian.YunFeiShop.Services.Ordering.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly Infrastructure.OrderingContext _context;

        public OrderController(IMediator mediator, Infrastructure.OrderingContext context)
        {
            _mediator = mediator;
            _context = context;
        }
        
        [HttpGet("createorder")]
        public async Task<IActionResult> CreateOrder()
        {
            var buyerId = new Guid("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAB");
            var orderLines = new[] {
                new CreateOrderCommand.OrderLine(new Guid("11111111-1111-1111-1111-111111111111"), "Prod1", 1),
                new CreateOrderCommand.OrderLine(new Guid("22222222-1111-1111-1111-111111111111"), "Prod2", 2)
            };
            var command = new CreateOrderCommand(buyerId, orderLines);

            await _mediator.Send(command);

            return Ok("created success");
        }



        [HttpGet("data")]
        public async Task<IActionResult> GetData()
        {
            var orders = await _context.Orders.Include(o => o.OrderLines).ToArrayAsync();

            // var integrationEvents = await _eventLogContext.IntegrationEventEntries.ToArrayAsync();

            return Ok(new { Orders = orders });
        }

        // [HttpGet("test")]
        // public async Task<IActionResult> GetTest()
        // {
        //     var buyerId = new Guid("BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBD");

        //     // Output: <empty>;
        //     System.Console.WriteLine("Current Transaction 1: {0}", _context.Database.CurrentTransaction?.TransactionId);

        //     using(var transaction = _context.Database.BeginTransaction())
        //     {
        //         // Output: 9036e379-0a3d-458f-ae7c-08b1f80fe6bf
        //         System.Console.WriteLine("Current Transaction 2.1.1: {0}", _context.Database.CurrentTransaction?.TransactionId);
        //         System.Console.WriteLine("Current Transaction 2.1.2: {0}", _eventLogContext.Database.CurrentTransaction?.TransactionId);
                

        //         _eventLogContext.Database.UseTransaction(transaction.GetDbTransaction());

        //         System.Console.WriteLine("Current Transaction 2.1.3: {0}", _context.Database.CurrentTransaction?.TransactionId);
        //         System.Console.WriteLine("Current Transaction 2.1.4: {0}", _eventLogContext.Database.CurrentTransaction?.TransactionId);
        //         System.Console.WriteLine("Current Transaction 2.1.5: {0}", _eventLogContext.Database.CurrentTransaction?.GetDbTransaction() == transaction.GetDbTransaction());

        //         // _context.Database.UseTransaction(transaction.GetDbTransaction());\
        //         var sameDbContextTransaction = _context.Database.CurrentTransaction == transaction;
        //         var sameDbTransaction = _context.Database.CurrentTransaction.GetDbTransaction() == transaction.GetDbTransaction();
        //         var sameDbContextTransaction2 = _eventLogContext.Database.CurrentTransaction == transaction;
        //         var sameDbTransaction2 = _eventLogContext.Database.CurrentTransaction.GetDbTransaction() == transaction.GetDbTransaction();
        //         System.Console.WriteLine($"------- {sameDbContextTransaction} -- {sameDbTransaction} -- {sameDbContextTransaction2} -- {sameDbTransaction2}");

                
        //         var order = new Order(buyerId,
        //             new[] {
        //                 new OrderLine { ProductId = new Guid("99999999-9999-9999-9999-999999999999"),  Name = "P9", Quantity = 9 }
        //             });
        //         _context.Add(order);
        //         await _context.SaveChangesAsync();

        //         var line = order.OrderLines.First();
        //         line.Quantity = 19;
        //         await _context.SaveChangesAsync();

        //         transaction.Commit();
        //         // Output: 9036e379-0a3d-458f-ae7c-08b1f80fe6bf
        //         System.Console.WriteLine("Current Transaction 2.2: {0}", transaction.TransactionId);
        //         // Output: <empty>;
        //         System.Console.WriteLine("Current Transaction 2.3: {0}", _context.Database.CurrentTransaction?.TransactionId);
        //     }

        //     // Output: <empty>;
        //     System.Console.WriteLine("Current Transaction 3: {0}", _context.Database.CurrentTransaction?.TransactionId);

        //     using(var transaction = _context.Database.BeginTransaction())
        //     {
        //         System.Console.WriteLine("Current Transaction 4.1: {0}", _context.Database.CurrentTransaction?.TransactionId);

        //         var order = _context.Orders.Include(o => o.OrderLines).SingleOrDefault(o => o.BuyerId == buyerId);
        //         var line = order.OrderLines.First();
        //         line.Quantity = 29;
        //         await _context.SaveChangesAsync();

        //         transaction.Commit();

        //         System.Console.WriteLine("Current Transaction 4.2: {0}", transaction.TransactionId);
        //         System.Console.WriteLine("Current Transaction 4.3: {0}", _context.Database.CurrentTransaction?.TransactionId);
        //     }

        //     System.Console.WriteLine("Current Transaction 5: {0}", _context.Database.CurrentTransaction?.TransactionId);

        //     return Ok();
        // }
    }
}
