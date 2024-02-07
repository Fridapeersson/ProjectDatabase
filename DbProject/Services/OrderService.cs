using DbProject.Dtos;
using DbProject.Entities;
using DbProject.Repositories;
using System;
using System.Linq.Expressions;

namespace DbProject.Services;

public class OrderService
{
    private readonly OrderRepository _orderRepository;
    private readonly ProductRepository _productRepository;

    public OrderService(OrderRepository orderRepository, ProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    /// <summary>
    ///     Creates a new OrderEntity to database based on the provided DTO
    /// </summary>
    /// <param name="newOrderDto">the data transfer object containing the information about the new order</param>
    /// <returns>The created Order entity, else null</returns>
    public Order CreateOrder(NewOrderDto newOrderDto)
    {
        try
        {
            var order = new Order
            {
                Orderdate = DateTime.Now
            };

            // loopa igenom varje OrderRow i newOrderDto och skapa en OrderRow-entitet för varje
            foreach(var orderRowDto in newOrderDto.OrderRows)
            {
                // hämta produktens ID baserat på produktnamnet
                var product = _productRepository.GetOne(x => x.Id == orderRowDto.ProductId);
                if (product == null)
                {
                    Console.WriteLine($"Productid {orderRowDto.ProductId} was not found.");
                }

                var orderRow = new OrderRow
                {
                    Quantity = orderRowDto.Quantity,
                    ProductId = product.Id
                };

                order.OrderRows.Add(orderRow);
            }

            //skapa nya ordern i dbn
            var createdOrder = _orderRepository.Create(order);
            return createdOrder;
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Gets all Orders from database
    /// </summary>
    /// <returns>A collection of OrderEntity objects, else null</returns>
    public IEnumerable<Order> GetAllOrders()
    {
        try
        {
            var orders = _orderRepository.GetAll();
            if(orders != null)
            {
                var orderList = new HashSet<Order>();
                foreach(var order in orders)
                {
                    orderList.Add(order);
                }
                return orderList;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Gets one OrderEntity from database based on provided predicate/expression
    /// </summary>
    /// <param name="predicate">The predicate/expression used to filter OrderEntity objects</param>
    /// <returns>The OrderEntity that matches the predicate/expression, else null</returns>
    public Order GetOneOrder(Expression<Func<Order, bool>> predicate)
    {
        try
        {
            var orderEntity = _orderRepository.GetOne(predicate);
            if(orderEntity != null)
            {
                return orderEntity;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Updates a existing OrderEntity
    /// </summary>
    /// <param name="orderEntity">The orderEntity containing the updated data</param>
    /// <returns>The updated OrderEntity, else null</returns>
    public Order UpdateOrder(Order orderEntity)
    {
        try
        {
            var updatedOrder = _orderRepository.Update(x => x.Id == orderEntity.Id, orderEntity);
            if(updatedOrder != null)
            {
                return updatedOrder;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Deletes an OrderEntity based on the provided predicate/expression
    /// </summary>
    /// <param name="predicate">The predicate/expression used to filter OrderEntity objects</param>
    /// <returns>True if deleted successfully, else false</returns>
    public bool DeleteOrder(Expression<Func<Order, bool>> predicate)
    {
        try
        {
            var orderEntity = _orderRepository.Delete(predicate);
            if(orderEntity)
            {
                return true;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }
}
