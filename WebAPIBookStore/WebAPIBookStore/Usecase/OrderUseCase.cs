using AutoMapper;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Enum;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;
using WebAPIBookStore.Usecase;

namespace WebAPIBookStore.UseCase
{
    public class OrderUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IMapper _mapper;
        private readonly IUseCaseOutput _useCaseOutput;

        public OrderUseCase(
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            ICartItemRepository cartItemRepository,
            IMapper mapper,
            IUseCaseOutput useCaseOutput)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _cartItemRepository = cartItemRepository;
            _mapper = mapper;
            _useCaseOutput = useCaseOutput;
        }

        public Output Get()
        {
            var orders = _orderRepository.GetOrders();
            if (orders == null)
            {
                return _useCaseOutput.NotFound("Not found order");
            }

            return _useCaseOutput.Success(orders);
        }

        public Output GetById(int id)
        {
            var order = _orderRepository.GetOrder(id);
            if (order == null)
            {
                return _useCaseOutput.NotFound("Not found order");
            }

            return _useCaseOutput.Success(_mapper.Map<OrderDto>(order));
        }

        public Output GetByStatus(OrderStatus status)
        {
            var orders = _orderRepository.GetOrderByStatus(status);
            if (!orders.Any())
            {
                return _useCaseOutput.NotFound("Not found order");
            }

            return _useCaseOutput.Success(_mapper.Map<List<OrderDto>>(orders));
        }

        public Output GetByUserId(int userId)
        {
            var orders = _orderRepository.GetOrderByUserId(userId);
            if (!orders.Any())
            {
                return _useCaseOutput.NotFound("Not found order");
            }

            return _useCaseOutput.Success(_mapper.Map<List<OrderDto>>(orders));
        }

        public Output Post(OrderCreate orderCreate)
        {
            if (!orderCreate.CartItemIds.Any())
            {
                return _useCaseOutput.InternalServer("No data input");
            }

            if (!_userRepository.UserExists(orderCreate.UserId))
            {
                return _useCaseOutput.NotFound("Not found user");
            }

            List<CartItem> cartItems = new List<CartItem>();
            foreach (int id in orderCreate.CartItemIds)
            {
                var cartItem = _cartItemRepository.GetCartItem(id);
                if (cartItem == null)
                {
                    return _useCaseOutput.NotFound("Not found cartItemId");
                }

                if (cartItem.Status == CartItemStatus.Paid)
                {
                    return _useCaseOutput.InternalServer("Data in cartItem not true");
                }
                  
                cartItems.Add(cartItem);
            }

            var orderMap = _mapper.Map<Order>(orderCreate.OrderDto);
            if (!_orderRepository.CreateOrder(cartItems, orderMap, orderCreate.UserId))
            {
                return _useCaseOutput.InternalServer("Something went wrong while saving");
            }

            return _useCaseOutput.Success(orderCreate);
        }

        public Output Put(OrderUpdate input)
        {
            var orderUpdate = _orderRepository.GetOrder(input.OrderId);
            if (orderUpdate == null)
            {
                return _useCaseOutput.NotFound("Not found orderId");
            }

            if (!_userRepository.ManageExists(input.ManageId))
            {
                return _useCaseOutput.NotFound("Not found manageId");
            }

            if (input.Status != OrderStatus.Cancel || input.Status != OrderStatus.Success)
            {
                return _useCaseOutput.InternalServer("Account has no access permission");
            }

            if (!_orderRepository.UpdateOrder(orderUpdate, input.Status, input.ManageId))
            {
                return _useCaseOutput.InternalServer("Something went wrong while saving");
            }

            return _useCaseOutput.Success(orderUpdate);
        }
    }
}
