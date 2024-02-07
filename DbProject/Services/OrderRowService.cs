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

    /// <summary>
    ///     
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    //public OrderRow CreateOrderRow(OrderRow entity)
    //{
    //    try
    //    {
    //        if(!_orderRowRepository.Exists(x => x.Id == entity.Id))
    //        {

    //        }
    //    }
    //    catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    //    return null!;
    //}


    /// <summary>
    ///     Gets all orderRows from database
    /// </summary>
    /// <returns>a collection of orderrow objects, else null</returns>
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

    /// <summary>
    ///     Gets one order based on the provided predicate/expression
    /// </summary>
    /// <param name="expression">The predicate/expression that filters OrderRowEntity objects</param>
    /// <returns>The OrderRowEntity that matches predicate/expression</returns>
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

    /// <summary>
    ///     Updates an existing OrderRowEntity
    /// </summary>
    /// <param name="orderRowEntity">The orderRowEntity containing the updated data</param>
    /// <returns>The updated OrderRowEntity, else null</returns>
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

    /// <summary>
    ///     Deletes an OrderRowEntity based on the provided predicate/expression
    /// </summary>
    /// <param name="expression">The predicate/expression used to filter OrderRowEntitiy objects</param>
    /// <returns>True if deleted successfully, else false</returns>
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

    /// <summary>
    ///     Gets Orderrows from each group of orderRows with the same orderId
    /// </summary>
    /// <returns>a collection of OrderRow objects that represents one row from each group with the same orderId</returns>
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
