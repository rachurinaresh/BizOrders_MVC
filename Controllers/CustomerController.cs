using Microsoft.AspNetCore.Mvc;
using BizOrders_MVC.Models;

namespace BizOrders_MVC.Controllers
{
    public class CustomerController : Controller
    {
        BizDBContext Context;
        public static Item[] items = new Item[10];
        public InvoicContent[] InvoicContents = new InvoicContent[1];
        public static int NoOfItems = 0;
        public static Customer Customerobj = new Customer();
        public static Order Orderobj = new Order();


        public CustomerController()
        {
            Context = new BizDBContext();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("Customer/items")]
        public IActionResult CreateItems(string CustomerName, DateTime PrimeStartDate, DateTime PrimeEndDate)
        {
            Customerobj.CustomerName = CustomerName;
            Customerobj.PrimeStartDate = PrimeStartDate;
            Customerobj.PrimeEndDate = PrimeEndDate;
            Context.Customers.Add(Customerobj);
            Context.SaveChanges();
            return View();
        }
        [HttpPost("Customer/order")]
        public IActionResult PlaceOrder(List<string> ItemName, List<float> ItemQuantity, List<float> ItemPrice, List<int> ItemDiscountEligible)
        {
            int length = ItemName.Count;
            for (int i = 0; i < length; i++)
            {
                Item item = new Item();
                item.ItemName = ItemName[i];
                item.ItemQuantity = ItemQuantity[i];
                item.ItemPrice = ItemPrice[i];
                item.ItemDiscountEligible = ItemDiscountEligible[i];
                items[i] = item;
                NoOfItems += 1;
            }
            return View();
        }
        [HttpPost("Customer/invoice")]
        public IActionResult InvoiceView(string PackageName, int DropAddressPinCode, string PaymentMode)
        {
            double DiscountEligibleGrandTotal = 0;
            double NonDiscountEligibleGrandTotal = 0;
            double PayableAmount = 0;
            double Invoice = 0;
            int OrderId;
            bool IsPrimeCustomer = false;
            DateTime PrimeStartDate = (DateTime)Context.Customers.Max(p => p.PrimeStartDate);
            DateTime PrimeEndDate = (DateTime)Context.Customers.Max(p => p.PrimeEndDate);
            double GrandTotal = 0;
            Orderobj.NoOfItems = NoOfItems;
            Orderobj.CustomerId = Context.Customers.Max(p => p.CustomerId);
            Orderobj.OrderPlacedTime = DateTime.UtcNow;
            Orderobj.PackageName = PackageName;
            Orderobj.DropAddressPinCode = DropAddressPinCode;
            Orderobj.PaymentMode = PaymentMode;
            DateTime today = DateTime.Today;
            int res1 = DateTime.Compare(today, PrimeStartDate);
            int res2 = DateTime.Compare(PrimeEndDate, today);

            if (res1 >= 0 && res2 >= 0)
            {
                IsPrimeCustomer = true;
            }

            for (int i = 0; i < NoOfItems; i++)
            {
                if (items[i].ItemDiscountEligible == 1)
                {
                    DiscountEligibleGrandTotal = (double)(DiscountEligibleGrandTotal + (items[i].ItemQuantity * items[i].ItemPrice));
                }
                else
                {
                    NonDiscountEligibleGrandTotal = (double)(NonDiscountEligibleGrandTotal + (items[i].ItemQuantity * items[i].ItemPrice));
                }
            }
            GrandTotal = DiscountEligibleGrandTotal + NonDiscountEligibleGrandTotal;
            Orderobj.GrandTotal = GrandTotal;
            Invoice = GrandTotal + (GrandTotal * 0.05);
            Orderobj.Invoice = Invoice;

            if (DiscountEligibleGrandTotal > 1500 && IsPrimeCustomer)
            {
                PayableAmount = Invoice - ((Invoice * 10) / 100);
            }
            else if (DiscountEligibleGrandTotal >= 1000 && DiscountEligibleGrandTotal <= 1500 && IsPrimeCustomer)
            {
                PayableAmount = Invoice - 50;
            }
            else
            {
                if (PaymentMode == "Cash" && !(IsPrimeCustomer))
                {
                    PayableAmount = Invoice - ((Invoice * 3) / 100);
                }

                else
                {
                    PayableAmount = Invoice;
                }
            }
            Orderobj.PayableAmount = PayableAmount;
            Context.Orders.Add(Orderobj);
            Context.SaveChanges();
            OrderId = Context.Orders.Max(p => p.OrderId);
            for (int i = 0; i < NoOfItems; i++)
            {
                items[i].OrderId = OrderId;
                Context.Items.Add(items[i]);
            }
            Context.SaveChanges();
            InvoicContent obj = new InvoicContent();
            obj.items = items;
            obj.Customerobj = Customerobj;
            obj.Orderobj = Orderobj;
            InvoicContents[0] = obj;
            return View(InvoicContents);
        }
    }
    public class InvoicContent
    {
        public Item[] items = new Item[10];
        public Customer Customerobj = new Customer();
        public Order Orderobj = new Order();
    }
}
