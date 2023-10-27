using basketApi.Context;
using orderApi.Models;

namespace orderApi.Services
{
    public class OrderServices
    {
        OrderDbContext orderDbContext;
        public OrderServices()
        {
            orderDbContext = new OrderDbContext();
        }


        public List<Order> GetAll()
        {
            var order = orderDbContext.Orders.ToList();

            return order;
        }
        public List<Order> GetCustomerById(int id)
        {
            var order = orderDbContext.Orders.Where(x=> x.customerid==id).ToList();
            if (order is not null)
            {
                return order;
            }
            return null;
        }

        public Order InsertOrder(Order order)
        {
            orderDbContext.Orders.Add(order);
            orderDbContext.SaveChanges();
            return order;
        }

        public Order UpdateOrder(Order order)
        {
            orderDbContext.Orders.Update(order);
            orderDbContext.SaveChanges();
            return order;
        }

        public List<Order> UpdateOrder(List<Order> order)
        {
            orderDbContext.Orders.UpdateRange(order);
            orderDbContext.SaveChanges();
            return order;
        }

        public bool DeleteOrder(int id)
        {
            var data = orderDbContext.Orders.Where(o => o.Id == id).FirstOrDefault();
            if (data != null)
            {
                orderDbContext.Orders.Remove(data);
                orderDbContext.SaveChanges();
                return true;
            }
            else { return false; }

        }

        public List<Order> getOrder(int customerid)
        {
            var data = orderDbContext.Orders.Where(o => o.customerid == customerid).ToList();
            if (data != null)
            {

                return data;
            }
            else { return null; }

        }

        public List<Order> getProduct(int productid)
        {
            var data = orderDbContext.Orders.Where(o => o.ProductId.Where(x=> x== productid).Any()).ToList();
            if (data != null)
            {

                return data;
            }
            else { return null; }

        }




    }
}
