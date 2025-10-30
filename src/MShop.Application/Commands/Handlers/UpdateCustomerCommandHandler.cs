using MediatR;
using MShop.Core.Base;
using Message = MShop.Core.Message;
using MShop.Domain.Entities;
using MShop.Infra.Data.Interface;
using MShop.Core.Data;

namespace MShop.Application.Commands.Handlers
{
    public class UpdateCustomerCommandHandler : BaseCommand, IRequestHandler<UpdateCustomerCommand, bool>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCustomerCommandHandler(
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork,
            Message.INotification notification
        ) : base(notification)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetById(request.Customer.Id);
            if (customer == null)
            {
                Notificar("Customer não encontrado.");
                return false;
            }

            customer.UpdateCustomer(request.Customer.Name,request.Customer.Email, request.Customer.Phone);

            if (!customer.IsValid(Notifications) || TheareErrors())
                return false;

            await _customerRepository.Update(customer, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }
    }
}