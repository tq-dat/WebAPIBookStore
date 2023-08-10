using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPIBookStore.Consts;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.UseCase
{
    public class OrderUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IMapper _mapper;

        public OrderUseCase(
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            ICartItemRepository cartItemRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _cartItemRepository = cartItemRepository;
            _mapper = mapper;
        }

        public Output Get()
        {
            var output = new Output();
            var orders = _orderRepository.GetOrders();
            if (orders == null)
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found order";
                return output;
            }

            output.Success = true;
            output.Data = orders; 
            return output;
        }

        public Output GetById(int id)
        {
            var output = new Output();
            var order = _orderRepository.GetOrder(id);
            if (order == null)
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found order";
                return output;
            }

            output.Success = true;
            output.Data = _mapper.Map<OrderDto>(order);
            return output;
        }

        public Output GetByStatus(string status)
        {
            var output = new Output();
            var orders = _orderRepository.GetOrderByStatus(status);
            if (!orders.Any())
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found order";
                return output;
            }

            output.Success = true;
            output.Data = _mapper.Map<List<OrderDto>>(orders);
            return output;
        }

        public Output GetByUserId(int userId)
        {
            var output = new Output();
            var orders = _orderRepository.GetOrderByUserId(userId);
            if (!orders.Any())
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found order";
                return output;
            }

            output.Success = true;
            output.Data = _mapper.Map<List<OrderDto>>(orders);
            return output;
        }

        public Output Post(OrderCreate orderCreate)
        {
            var output = new Output();
            if (!orderCreate.CartItemIds.Any())
            {
                output.Success = false;
                output.Error = StatusCodeAPI.InternalServer;
                output.Message = "No data input";
                return output;
            }

            if (!_userRepository.UserExists(orderCreate.UserId))
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found user";
                return output;
            }

            List<CartItem> cartItems = new List<CartItem>();
            foreach (int id in orderCreate.CartItemIds)
            {
                var cartItem = _cartItemRepository.GetCartItem(id);
                if (cartItem == null)
                {
                    output.Success = false;
                    output.Error = StatusCodeAPI.NotFound;
                    output.Message = "Not found cartItemId";
                    return output;
                }

                if (cartItem.Status == "Paid")
                {
                    output.Success = false;
                    output.Error = StatusCodeAPI.InternalServer;
                    output.Message = "Data in cartItem not true";
                    return output;
                }
                  
                cartItems.Add(cartItem);
            }

            var orderMap = _mapper.Map<Order>(orderCreate.OrderDto);
            if (!_orderRepository.CreateOrder(cartItems, orderMap, orderCreate.UserId))
            {
                output.Success = false;
                output.Error = StatusCodeAPI.InternalServer;
                output.Message = "Something went wrong while saving";
                return output;
            }

            output.Success = true;
            output.Data = orderCreate;
            return output;
        }

        public Output Put(OrderUpdate input)
        {
            var output = new Output();
            var orderUpdate = _orderRepository.GetOrder(input.OrderId);
            if (orderUpdate == null)
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found orderId";
                return output;
            }

            if (!_userRepository.ManageExists(input.ManageId))
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found manageId";
                return output;
            }

            if (input.Status != "Cancel" || input.Status != "Success")
            {
                output.Success = false;
                output.Error = StatusCodeAPI.InternalServer;
                output.Message = "Account has no access permission";
                return output;
            }

            if (!_orderRepository.UpdateOrder(orderUpdate, input.Status, input.ManageId))
            {
                output.Success = false;
                output.Error = StatusCodeAPI.InternalServer;
                output.Message = "Something went wrong while saving";
                return output;
            }

            output.Success = true;
            output.Data = orderUpdate;
            return output;
        }
    }
}
