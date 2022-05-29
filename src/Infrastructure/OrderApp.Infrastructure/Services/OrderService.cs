using AutoMapper;
using Newtonsoft.Json;
using OrderApp.Application.Dtos.Requests;
using OrderApp.Application.Interfaces.IRepository;
using OrderApp.Application.Interfaces.IService;
using OrderApp.Application.Interfaces.IUnitOfWork;
using OrderApp.Domain.Entities;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBaseRepository<Order> _baseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OrderService(IBaseRepository<Order> baseRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _baseRepository = baseRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> CreateOrderAsync(CreateOrderRequest request)
        {
            var model = await _baseRepository.AddAsync(_mapper.Map<Order>(request));
            await _unitOfWork.CommitAsync();

            return model.Id;
        }
    }
}
