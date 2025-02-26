using Microsoft.AspNetCore.Mvc;
using BizOrders_MVC.Models;
namespace BizOrders_MVC.Controllers
{
    public class AdminController : Controller
    {
        BizDBContext Context;
        public AdminController()
        {
            Context = new BizDBContext();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("OrderDetails")]
        public IActionResult Result(int Choice, string CustomerName)
        {
            if (Choice == 0)
            {
                var Orders = Context.Orders.Where(res => res.OrderPlacedTime >= DateTime.UtcNow.AddMinutes(-60)).ToList<Order>();
                var customerstable = Context.Customers;
                var result = (from ord in Orders
                              join cust in customerstable on ord.CustomerId equals cust.CustomerId
                              select new Result()
                              {
                                  CustomerName = cust.CustomerName,
                                  OrderId = ord.OrderId,
                                  CustomerId = cust.CustomerId,
                                  NoOfItems = ord.NoOfItems,
                                  PayableAmount = ord.PayableAmount,
                                  StartDate = Convert.ToDateTime(cust.PrimeStartDate),
                                  EndDate = Convert.ToDateTime(cust.PrimeEndDate),
                              }).ToList();
                return View(result);
            }

            else
            {
                var Customers = Context.Customers.Where(r => r.CustomerName == CustomerName).ToList();
                var orderstable = Context.Orders;
                var result = (from cust in Customers
                              join ord in orderstable on cust.CustomerId equals ord.CustomerId
                              select new Result()
                              {
                                  CustomerName = cust.CustomerName,
                                  OrderId = ord.OrderId,
                                  CustomerId = cust.CustomerId,
                                  NoOfItems = ord.NoOfItems,
                                  PayableAmount = ord.PayableAmount,
                                  StartDate = Convert.ToDateTime(cust.PrimeStartDate),
                                  EndDate = Convert.ToDateTime(cust.PrimeEndDate),
                              }).ToList();
                return View(result);
            }
        }
    }
    public class Result
    {
        public string? CustomerName { get; set; }
        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public int? NoOfItems { get; set; }
        public double? PayableAmount { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
