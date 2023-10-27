using DotNetCore.CAP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using orderApi.Models;
using orderApi.Services;

namespace customerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        OrderServices orderServices;
        private readonly ICapPublisher capPublisher;
        public OrderController(ICapPublisher _capPublisher)
        {
            orderServices = new OrderServices();
            capPublisher = _capPublisher;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var data = orderServices.GetAll();
                return Ok(data);
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
        }



        [HttpPost("InsertOrder")]
        public async Task<IActionResult> InsertOrder(Basket basket)
        {
            Order order = new Order
            {
                customerid = basket.customerid,
                ProductId = basket.ProductId,
                Description = "Ürün Sipariş Edildi",
                Email = "Örnek@mail.com",
                Firstname = "Cengiz ",
                Id = 0,
                CreatedDateTime = DateTime.Now.ToShortDateString(),
                Lastname = "YEKTAŞ",
                Phone = "xxxxx ",
                Status = 1
            };
            var data = orderServices.InsertOrder(order);
            await capPublisher.PublishAsync<Order>("order.add", order);
            return Ok(data.Id);
        }


        [NonAction]
        [CapSubscribe("customer.put")]
        public void UpdateOrderFromCap(Customer customer)
        {
            List<Order> orders = orderServices.GetCustomerById(customer.Id);
            if (orders is not null)
            {
                foreach (var item in orders)
                {
                    item.Firstname = customer.Firstname;
                    item.Lastname = customer.Lastname;
                    item.Phone = customer.Phone;
                    item.Description = $"Müşteri bilgileri {DateTime.Now.ToString("dd.MM.yyyy")}  Tarihinde Güncellendi.";
                }
                var data = orderServices.UpdateOrder(orders);
            }
            else
            {
                Console.Write("Günceleme İşlemi Başarısız Oldu");
            }
        }


        [NonAction]
        [CapSubscribe("order.add")]
        public void AddOrderFromCap(Order order)
        {
            order.Description = "CAP Üzerinden Gelen Sipariş Onaylandı.";
            var data = orderServices.UpdateOrder(order);
        }



        [HttpPost("UpdateOrder")]
        public IActionResult Update(Order order)
        {
            var data = orderServices.UpdateOrder(order);
            return Ok(data);
        }

        [HttpGet("DeleteOrder")]
        public IActionResult Delete(int id)
        {
            var data = orderServices.DeleteOrder(id);
            return Ok(data);
        }
        [HttpGet("GetOrdersByCustomerId")]
        public IActionResult Customer(int id)
        {
            var data = orderServices.getOrder(id);
            return Ok(data);
        }
        [HttpGet("GetOrdersByProductId")]
        public IActionResult xxx(int id)
        {
            var data = orderServices.getProduct(id);
            
            return Ok(data);
        }

    }
}
