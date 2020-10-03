using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Tymish.Application.Invoices.Commands
{
    public class SubmitInvoiceCommand : IRequest<Invoice>
    {
        public Guid VendorId { get; set; }
        public IList<TimeEntry> TimeEntries { get; set; }
        public Decimal GetTotalInvoiceAmount(Decimal hourlyPay)
        {
            var invoiceHours = TimeEntries
                .Select(timeEntry
                    => (timeEntry.End - timeEntry.Start).Hours)
                .ToList()
                .Sum();
            
            return hourlyPay * invoiceHours;
        }
    }

    public class SubmitInvoiceHandler : IRequestHandler<SubmitInvoiceCommand, Invoice>
    {
        private readonly ITymishDbContext _context;
        public SubmitInvoiceHandler(ITymishDbContext context) {
            _context = context;
        }
        public async Task<Invoice> Handle(SubmitInvoiceCommand request, CancellationToken cancellationToken)
        {
            var vendor = await _context
                .Set<Vendor>()
                .SingleOrDefaultAsync(vendor
                    => vendor.Id == request.VendorId,
                    cancellationToken);

            var invoice = new Invoice
            {
                Id = Guid.NewGuid(),
                VendorId = request.VendorId,
                TimeEntries = request.TimeEntries,
                Created = DateTime.UtcNow,
                Submitted = DateTime.UtcNow,
                TotalAmount = request.GetTotalInvoiceAmount(vendor.HourlyPay)
            };

            _context.Set<Invoice>().Add(invoice);
            await _context.SaveChangesAsync(cancellationToken);

            return invoice;
        }
    }
}