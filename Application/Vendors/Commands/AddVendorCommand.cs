using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Tymish.Application.Interfaces;
using Tymish.Domain.Entities;

namespace Tymish.Application.Vendors.Commands
{
    public class AddVendorCommand : IRequest<Vendor>
    {
        private string _email;
        public string Email
        { 
            get => _email;
            set => _email = value.Trim().ToLower();
        }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public decimal HourlyPay { get; set; }
        public string VendorSiteDomain { get; set; }
    }
    public class AddVendorHandler : IRequestHandler<AddVendorCommand, Vendor>
    {
        private readonly ITymishDbContext _context;
        private readonly IEmailGateway _email;
        public AddVendorHandler(ITymishDbContext context, IEmailGateway email)
        {
            _context = context;
            _email = email;
        }

        public async Task<Vendor> Handle(AddVendorCommand request, CancellationToken cancellationToken)
        {
            var vendor = new Vendor(request.Email)
            {
                Id = Guid.NewGuid(),
                GivenName = request.GivenName,
                FamilyName = request.FamilyName,
                HourlyPay = request.HourlyPay
            };

            await _context.Set<Vendor>().AddAsync(vendor, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            await _email.Send(
                request.Email,
                "Dance Code invites you to register for Tymish",
                $"{request.VendorSiteDomain}/register/{vendor.Id}");

            return vendor;
        }
    }
}