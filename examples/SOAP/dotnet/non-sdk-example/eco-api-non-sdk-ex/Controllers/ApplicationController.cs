using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel;

using Api_nonSDK_example.EcoApiServiceReference;
using Api_nonSDK_example.Models;
using System.Text;


namespace Api_nonSDK_example.Controllers
{
    public class ApplicationController : Controller
    {

        private EconomicWebServiceSoapClient EcoSession
        {
            get
            {
                var session = Session["eco-session"];
                if (session == null)
                {
                    ViewData["Error_message"] = "Not logged in";
                    return null;
                }
                return (EconomicWebServiceSoapClient)session;
            }
        }

        public ActionResult ExampleView()
        {
            if (EcoSession != null)
                ViewData["Auth_message"] = "Connected";
            return View();
        }

        public ActionResult ReturnWithError(string message)
        {
            ViewData["Error_message"] = message;
            return View("ExampleView");
        }

        public ActionResult Connect(string token, string appToken)
        {

            // create the webservice client to get data. Remember the "allowCookies"-part..
            var session = new EconomicWebServiceSoapClient();
            ((BasicHttpBinding)session.Endpoint.Binding).AllowCookies = true;

            try
            {
                session.ConnectWithToken(token, appToken);
            }
            catch (Exception e)
            {
                ViewData["Auth_message"] = "Log in error" + e.Message;
                return View("ExampleView");
            }
            Session["eco-session"] = session;
            ViewData["Auth_message"] = "Connected";

            return View("ExampleView");
        }

        public ActionResult Disconnect()
        {
            var session = EcoSession;
            if (session == null) return View("ExampleView");

            session.Disconnect();
            session.Close();

            Session.Abandon();
            return View("ExampleView");
        }

        public ActionResult CreateProductsAndGroup()
        {
            // fetching the connected e-conomic session 
            var session = EcoSession;
            if (session == null) return View("ExampleView");

            try
            {
                //create a product-group for the new product
                var accHandle = session.Account_FindByNumber(1010);
                var productGroup = new ProductGroupData
                {
                    Number = 1,
                    Name = "Fruits",
                    AccountForVatLiableDebtorInvoicesCurrentHandle = accHandle
                };
                ProductGroupHandle productGroupHandle = session.ProductGroup_CreateFromData(productGroup);

                //create produkts
                var product1 = new ProductData
                {
                    Number = "1",
                    Name = "Apples",
                    SalesPrice = 5,
                    IsAccessible = true,
                    ProductGroupHandle = productGroupHandle
                };
                var product2 = new ProductData
                {
                    Number = "2",
                    Name = "Grapes",
                    SalesPrice = 10,
                    IsAccessible = true,
                    ProductGroupHandle = productGroupHandle
                };
                var products = new ProductData[] { product1, product2 };

                ProductHandle[] productsHandle = session.Product_CreateFromDataArray(products);

                productGroup.Handle = productGroupHandle;
                for (int i = 0; i < productsHandle.Length; i++)
                    products[i].Handle = productsHandle[i];

                EconomicModels.ProductGroup = productGroup;
                EconomicModels.Products = products;

            }
            catch (Exception e)
            {
                return ReturnWithError(e.Message);
            }

            ViewData["message"] = "Products and productgroup created!";

            return View("ExampleView");
        }

        public ActionResult CreateDeptorsAndGroup()
        {
            var session = EcoSession;
            if (session == null) return View("ExampleView");
            
            try
            {
                // create debtor group
                var debtorGroup = new DebtorGroupData
                {
                    Number = 2,
                    Name = "Fruit Retailers",
                    AccountHandle = EconomicModels.ProductGroup.AccountForVatLiableDebtorInvoicesCurrentHandle
                };
                DebtorGroupHandle dgHandle = session.DebtorGroup_CreateFromData(debtorGroup);

                // create debtor
                var currencyHandle = session.Currency_FindByCode("DKK");
                var payTermHandle = session.TermOfPayment_GetAll().First();
                var layoutHandle = session.TemplateCollection_GetAll().First();
                var debtor = new DebtorData
                {
                    Number = "3110",
                    Name = "Fruits'R'Us",
                    IsAccessible = true,
                    VatZone = EcoApiServiceReference.VatZone.HomeCountry,
                    LayoutHandle = layoutHandle,
                    CurrencyHandle = currencyHandle,
                    TermOfPaymentHandle = payTermHandle,
                    DebtorGroupHandle = dgHandle
                };
                DebtorHandle debtorHandle = session.Debtor_CreateFromData(debtor);

                debtorGroup.Handle = dgHandle;
                debtor.Handle = debtorHandle;
                EconomicModels.DebtorGroup = debtorGroup;
                EconomicModels.Debtor = debtor;
            }
            catch (Exception e)
            {
                return ReturnWithError(e.Message);
            }

            ViewData["message"] = "Debtor and debtorgroup created!";
            return View("ExampleView");
        }

        public ActionResult CreateOrderAndLines()
        {
            try
            {
                var accHandle = (AccountHandle)Session["accHandle"];

                var session = EcoSession;
                if (session == null) return View("ExampleView");
            
                //create order using the debtor created before
                var order = new OrderData
                {
                    DebtorHandle = EconomicModels.Debtor.Handle,
                    DebtorName = EconomicModels.Debtor.Name,
                    Date = DateTime.Now,
                    CurrencyHandle = EconomicModels.Debtor.CurrencyHandle,
                    TermOfPaymentHandle = EconomicModels.Debtor.TermOfPaymentHandle

                };
                OrderHandle orderHandle = session.Order_CreateFromData(order);

                var products = EconomicModels.Products;
            
                // add items to this order, using the created products
                var orderLines = new OrderLineData[]
                {
                    new OrderLineData { ProductHandle = products[0].Handle, Description = products[0].Name, Quantity = 500 , OrderHandle = orderHandle, UnitCostPrice = products[0].SalesPrice },
                    new OrderLineData { ProductHandle = products[1].Handle, Description = products[1].Name, Quantity = 200 , OrderHandle = orderHandle, UnitCostPrice = products[1].SalesPrice }
                };

                session.OrderLine_CreateFromDataArray(orderLines);
                order.Handle = orderHandle;
                EconomicModels.Order = order;

            }
            catch (Exception e)
            {
                return ReturnWithError(e.Message);
            }

            ViewData["message"] = "Order created and orderlines added!";
            return View("ExampleView");
        }


        public ActionResult UpgradeOrder()
        {
            var session = EcoSession;
            if (session == null) return View("ExampleView");

            try
            {
                var order_handle = EconomicModels.Order.Handle;

                // make this order as a current-invoice
                session.Order_UpgradeToInvoice(order_handle);
            }
            catch (Exception e)
            {
                return ReturnWithError(e.Message);
            }

            ViewData["message"] = "Order upgraded to invoice!";
            return View("ExampleView");
        }
    }
}
