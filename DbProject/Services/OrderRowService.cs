using DbProject.Entities;
using DbProject.Repositories;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace DbProject.Services;

public class OrderRowService
{
    private readonly OrderRowRepository _orderRowRepository;

    public OrderRowService(OrderRowRepository orderRowRepository)
    {
        _orderRowRepository = orderRowRepository;
    }

    public OrderRow CreateOrderRow(OrderRow entity)
    {
        try
        {
            if(!_orderRowRepository.Exists(x => x.Id == entity.Id))
            {

            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public IEnumerable<OrderRow> GetAllOrderRows() 
    {
        try
        {
            var orderRows = _orderRowRepository.GetAll();
            if(orderRows != null)
            {
                var orderRowList = new HashSet<OrderRow>();
                foreach(var orderRow in orderRows)
                {
                    orderRowList.Add(orderRow);
                }
                return orderRowList;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public OrderRow GetOneOrderRow(Expression<Func<OrderRow, bool>> expression)
    {
        try
        {
            var orderRowEntity = _orderRowRepository.GetOne(expression);
            if(orderRowEntity != null)
            {
                return orderRowEntity;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public OrderRow UpdateOrderRow(OrderRow orderRowEntity)
    {
        try
        {
            var updatedOrderRow = _orderRowRepository.Update(x => x.Id == orderRowEntity.Id, orderRowEntity);
            if(updatedOrderRow != null)
            {
                return updatedOrderRow;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public bool DeleteOrderRow(Expression<Func<OrderRow, bool>> expression)
    {
        try
        {
            var orderRowEntity = _orderRowRepository.Delete(expression);
            if (orderRowEntity)
            {
                return true;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }

    public IEnumerable<OrderRow> GetOrdersWithSameId()
    {
        try
        {
            var orderRows = _orderRowRepository.GetAll();

            // gruppera orderRows efter OrderId
            var groupedOrderRows = orderRows.GroupBy(i => i.OrderId);

            foreach (var groupedOrderRow in groupedOrderRows)
            {
                Console.WriteLine($"Order id {groupedOrderRow.Key}:");
                foreach (var orderRow in groupedOrderRow)
                {
                    Console.WriteLine($"Product: {orderRow.Product.ProductName}, Quantity: {orderRow.Quantity}");
                }
                Console.WriteLine();
            }
            return orderRows;
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }
}
