using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Economic.Api;
using Economic.Api.Data;
using Economic.Api.Exceptions;

public partial class Default2 : System.Web.UI.Page
{
    EconomicSession _session = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        _session = new EconomicSession();
    }
    private string Connect()
    {
        TextBox4.Text = "Connecting.." + "\n";
        return _session.Connect(Int32.Parse(TextBox1.Text), TextBox2.Text, TextBox3.Text);        
    }
    private void Disconnect()
    {
        TextBox4.Text += "\n" + "Disconnecting..";
        _session.Disconnect();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            //connect
            string connectMessage = Connect();
            TextBox4.Text += "Connection Success. Connection UniqueID: " + connectMessage;            
            
            //get API version.
            TextBox4.Text += "\r\n" + "API version: " + _session.GetApiInformation();
        }
        catch (Exception ex)
        {
            TextBox4.Text += ex.Message;
        }
        finally
        {
            //always disconnect after we are finished.
            Disconnect();
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        TextBox4.Text = "";
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        try
        {
            //connect
            string connectMessage = Connect();

            //get base currency
            ICompany thisCompany = _session.Company.Get();
            if (thisCompany == null) throw new Exception("No company found.");

            ICurrency thisCurrency = thisCompany.BaseCurrency;
            if (thisCurrency == null) throw new Exception("No currency found.");
            
            TextBox4.Text += thisCurrency.Code;
        }
        catch (Exception ex)
        {
            TextBox4.Text += ex.Message;
        }
        finally
        {
            //always disconnect after we are finished.
            Disconnect();
        }
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        try
        {
            //connect
            string connectMessage = Connect();

            //get all accounts
            IAccount[] accounts = _session.Account.GetAll();
            if (accounts.Length == 0) throw new Exception("No accounts exist");
            TextBox4.Text += "Number of accounts: " + accounts.Length.ToString() + "\n";

            //Get data objects for all accounts.
            IAccountData[] accData = _session.AccountData.GetDataArray(accounts);
            if (accData.Length == 0) throw new Exception("No accountData exist");
            foreach (IAccountData ac in accData)
            {
                TextBox4.Text += ac.Number + " " + ac.Name + "\n";
            }
        }
        catch (Exception ex)
        {
            TextBox4.Text += ex.Message;
        }
        finally
        {
            //always disconnect after we are finished.
            Disconnect();
        }
    }
    protected void Button5_Click(object sender, EventArgs e)
    {
        try
        {
            //connect
            string connectMessage = Connect();

            //get all orders
            IOrder[] orders = _session.Order.GetAll();
            if (orders.Length == 0) throw new Exception("No orders exist");
            TextBox4.Text += "Number of orders: " + orders.Length.ToString() + "\n";

            //Get data objects for all orders and display some Properties.
            IOrderData[] orderData = _session.OrderData.GetDataArray(orders);
            if (orderData.Length == 0) throw new Exception("No orderData exist");
            foreach (IOrderData oc in orderData)
            {
                TextBox4.Text += "No: " + oc.Number + " Debtorname: " + oc.Debtor.Name + "\n";
            }
        }
        catch (Exception ex)
        {
            TextBox4.Text += ex.Message;
        }
        finally
        {
            //always disconnect after we are finished.
            Disconnect();
        }
    }
    protected void Button6_Click(object sender, EventArgs e)
    {
        try
        {
            //connect
            string connectMessage = Connect();

            //get all currentinvoices
            ICurrentInvoice[] invoices = _session.CurrentInvoice.GetAll();
            if (invoices.Length == 0) throw new Exception("No currentinvoices exist");
            TextBox4.Text += "Number of currentinvoices: " + invoices.Length.ToString() + "\n";

            //Loop through all Current Invoices and display some information.
            foreach (ICurrentInvoice ci in invoices)
            {
                TextBox4.Text += "Debtorname: " + ci.Debtor.Name + " Invoice Date: " + ci.Date.ToShortDateString();
                ICurrentInvoiceLine [] lines = ci.GetLines();

                if (lines.Length == 0) throw new Exception("\nNo Lines exist on this CurrentInvoice");
                TextBox4.Text += " Number of lines: " + lines.Length.ToString() + "\n";
            }
        }
        catch (Exception ex)
        {
            TextBox4.Text += ex.Message;
        }
        finally
        {
            //always disconnect after we are finished.
            Disconnect();
        }
    }
    protected void Button7_Click(object sender, EventArgs e)
    {
        try
        {
            //connect
            string connectMessage = Connect();

            //Get all debtors
            IDebtor [] debtors = _session.Debtor.GetAll();
            if (debtors.Length == 0) throw new Exception("No Debtors found");

            foreach(IDebtor debtor in debtors)
            {
                TextBox4.Text += "Number: " + debtor.Number + " Name: " + debtor.Name + " Balance: " + debtor.Balance.ToString() + "\n";
            }
        }
        catch (Exception ex)
        {
            TextBox4.Text += ex.Message;
        }
        finally
        {
            //always disconnect after we are finished.
            Disconnect();
        }
    }
    protected void Button8_Click(object sender, EventArgs e)
    {
        try
        {
            //connect
            string connectMessage = Connect();

            //Get next available DebtorNumber
            TextBox4.Text += "Next Available DebtorNumber: " + _session.Debtor.GetNextAvailableNumber().ToString();
        }
        catch (Exception ex)
        {
            TextBox4.Text += ex.Message;
        }
        finally
        {
            //always disconnect after we are finished.
            Disconnect();
        }
    }

    private void WriteDebtorEntries(IDebtor deb, IDebtorEntry[] debEntries)
    {
        TextBox4.Text += "Number of deb entries: " + debEntries.Length + "\nSaldo: " + deb.Balance.ToString() + "\r\n" + "\r\n";

        foreach (IDebtorEntry de in debEntries)
        {
            string text;
            if (de.Text == null)
                text = "----------";
            else
                text = de.Text;

            string amount;
            if (de.Amount.ToString().Length < 8)
                amount = de.Amount.ToString() + "\t\t";
            else
                amount = de.Amount.ToString() + "\t";

            string amountDefaultCurrency;
            if (de.AmountDefaultCurrency.ToString().Length < 8)
                amountDefaultCurrency = de.AmountDefaultCurrency.ToString() + "\t\t";
            else
                amountDefaultCurrency = de.AmountDefaultCurrency.ToString() + "\t";
            TextBox4.Text += //de.Type.ToString() + "\t" +
                //de.Date.ToString() + "\t" +
                //de.Account.Number + "\t" +
                                amountDefaultCurrency +
                                de.Currency.Code + "\t\t" +
                                amount +
                                de.VoucherNumber.ToString() + "\t\t" +
                                text + "\t\t" +
                                de.InvoiceNumber.ToString() + "\t" +
                //de.DueDate.ToString() + "\t" +
                                de.Remainder.ToString() + "\t" +
                                de.RemainderDefaultCurrency.ToString() + "\t" + "\r\n";
        }
    }
    protected void Button9_Click(object sender, EventArgs e)
    {
        try
        {
            //connect
            string connectMessage = Connect();

            //Get first debtor, if any
            IDebtor [] debitors = _session.Debtor.GetAll();
            if (debitors.Length == 0) throw new Exception("No Debitors found");

            IDebtor deb = debitors[0];
            
            IDebtorEntry[] debEntries = deb.GetEntries();
            if (debEntries == null) throw new Exception("No DebtorEntries found.");

            TextBox4.Text += "First Debtor's Entries\n";

            WriteDebtorEntries(deb, debEntries);
        }
        catch (Exception ex)
        {
            TextBox4.Text += ex.Message;
        }
        finally
        {
            //always disconnect after we are finished.
            Disconnect();
        }
    }
    protected void Button10_Click(object sender, EventArgs e)
    {
        try
        {
            //connect
            string connectMessage = Connect();

            //Get all Creditors
            ICreditor[] creditors = _session.Creditor.GetAll();
            if (creditors.Length == 0) throw new Exception("No Creditors found");

            foreach (ICreditor creditor in creditors)
            {
                TextBox4.Text += "Number: " + creditor.Number + " Name: " + creditor.Name + " Address: " + creditor.Address + "\n";
            }
        }
        catch (Exception ex)
        {
            TextBox4.Text += ex.Message;
        }
        finally
        {
            //always disconnect after we are finished.
            Disconnect();
        }
    }
    protected void Button11_Click(object sender, EventArgs e)
    {
        try
        {
            //connect
            string connectMessage = Connect();

            //Get all Subscriptions
            ISubscription[] subscriptions = _session.Subscription.GetAll();
            if (subscriptions.Length == 0) throw new Exception("No Subscriptions found");

            TextBox4.Text += "Number of Subscriptions: " + subscriptions.Length.ToString() + "\n";

            foreach (ISubscription sub in subscriptions)
            {
                TextBox4.Text += "Number: " + sub.Number + " Name: " + sub.Name + " Description: " + sub.Description + "\n";
            }
        }
        catch (Exception ex)
        {
            TextBox4.Text += ex.Message;
        }
        finally
        {
            //always disconnect after we are finished.
            Disconnect();
        }
    }
    protected void Button12_Click(object sender, EventArgs e)
    {
        try
        {
            //connect
            string connectMessage = Connect();

            //Get all Projects
            IProject[] projects = _session.Project.GetAll();
            if (projects.Length == 0) throw new Exception("No Projects found");

            TextBox4.Text += "Number of Projects: " + projects.Length.ToString() + "\n";

            foreach (IProject proj in projects)
            {
                TextBox4.Text += "Number: " + proj.Number + " Name: " + proj.Name + " IsClosed: " + proj.IsClosed + "\n";
            }
        }
        catch (Exception ex)
        {
            TextBox4.Text += ex.Message;
        }
        finally
        {
            //always disconnect after we are finished.
            Disconnect();
        }
    }
    protected void Button13_Click(object sender, EventArgs e)
    {
        try
        {
            //connect
            string connectMessage = Connect();

            //Get all Products
            IProduct[] products = _session.Product.GetAll();
            if (products.Length == 0) throw new Exception("No Products found");

            TextBox4.Text += "Number of Products: " + products.Length.ToString() + "\n";

            foreach (IProduct prod in products)
            {
                TextBox4.Text += "Number: " + prod.Number + " Name: " + prod.Name + " SalesPrice: " + prod.SalesPrice + "\n";
            }
        }
        catch (Exception ex)
        {
            TextBox4.Text += ex.Message;
        }
        finally
        {
            //always disconnect after we are finished.
            Disconnect();
        }
    }
    protected void Button14_Click(object sender, EventArgs e)
    {
        try
        {
            //connect
            string connectMessage = Connect();

            //Create New Product
            IProduct newProduct = _session.Product.CreateFromData(GetProductData());
            if (newProduct == null) throw new Exception("New Product not created!");

            string productNameSaved = newProduct.Name;

            //delete this newly created product here.
            newProduct = null;

            //find the product again. This is to make sure, it was properly saved in the database.
            newProduct = _session.Product.FindByName(productNameSaved)[0];
            if(newProduct == null) throw new Exception("New Product not found!");

            TextBox4.Text += "My newly created product: \n";
            TextBox4.Text += "ProductNumber: " + newProduct.Number + "\n";
            TextBox4.Text += "ProductName: " + newProduct.Name + "\n";
            TextBox4.Text += "ProductGroup: " + newProduct.ProductGroup.Name + "\n";
        }
        catch (Exception ex)
        {
            TextBox4.Text += ex.Message;
        }
        finally
        {
            //always disconnect after we are finished.
            Disconnect();
        }
    }

    private IProductData GetProductData()
    {
        int prodNumber = new Random().Next(1000000);
        string productNumber = "Nr_ " + prodNumber.ToString();
        string productName = "My New ProductName " + productNumber;
        //Use Product name, if there is one.
        if (TextBox5.Text != "") productName = TextBox5.Text;
        IProductGroup productGroup = _session.ProductGroup.GetAll()[0];
        if (productGroup == null) throw new Exception("No productgroup found.");

        IProductData product = _session.ProductData.Create(productNumber, productGroup, productName);

        //set sales price, as we ask for that when testing.
        product.SalesPrice = prodNumber;
        return product;
    }
    protected void Button15_Click(object sender, EventArgs e)
    {
        try
        {
            //connect
            string connectMessage = Connect();

            //Create New Product
            IProduct [] products = _session.Product.GetAll();

            if (products.Length == 0) throw new Exception("No Products found.");
            
            //delete last product
            IProduct deleteProduct = products[products.Length - 1];
            if (deleteProduct == null) throw new Exception("No Product found.");
            string deleteProductName = deleteProduct.Name;
            deleteProduct.Delete();
            TextBox4.Text += "Product deleted: " + deleteProductName + "\n";        
        }
        catch (Exception ex)
        {
            TextBox4.Text += ex.Message;
        }
        finally
        {
            //always disconnect after we are finished.
            Disconnect();
        }
    }
    protected void Button16_Click(object sender, EventArgs e)
    {
        try
        {
            //connect
            string connectMessage = Connect();

            IDebtor [] myDebtor = _session.Debtor.GetAll();
            if (myDebtor.Length == 0) throw new Exception("No Debtors found");

            ICurrentInvoice mycurInvoice = _session.CurrentInvoice.Create(myDebtor[0]);            
            if (mycurInvoice == null) throw new Exception("No curInvoice created");
            
            ICurrentInvoiceLine myCurInvoiceLine = _session.CurrentInvoiceLine.Create(mycurInvoice);
            if (myCurInvoiceLine == null) throw new Exception("No CurInvoiceLine created");

            IProduct product = _session.Product.FindByNumber("101");
            if (product == null) throw new Exception("No product found");

            myCurInvoiceLine.Product = product;
            myCurInvoiceLine.Quantity = 3;
            myCurInvoiceLine.UnitNetPrice = 1000;

            mycurInvoice.Date = DateTime.Now;
            int otherRefNumber = new Random().Next(1000000);
            mycurInvoice.OtherReference = "Ref_" + otherRefNumber.ToString();

            IInvoice bookedInvoice = mycurInvoice.Book();

            TextBox4.Text += "Current Invoice Booked.\nOther ref: " + bookedInvoice.OtherReference;
        }
        catch (Exception ex)
        {
            TextBox4.Text += ex.Message;
        }
        finally
        {
            //always disconnect after we are finished.
            Disconnect();
        }
    }
    protected void Button17_Click(object sender, EventArgs e)
    {
        try
        {
            //connect
            string connectMessage = Connect();

            //does an other ref exist?
            if (TextBox6.Text == "")
            {
                throw new Exception("No Other ref found.");
            }

            IInvoice [] myOtherRefInvoice = _session.Invoice.FindByOtherReference(TextBox6.Text);
            if (myOtherRefInvoice.Length == 0) throw new Exception("No Invoice with OtherRef " + TextBox6.Text + " found");
                       
            TextBox4.Text += "Invoice Number: " + myOtherRefInvoice[0].Number + "\n";
            TextBox4.Text += "Invoice Other Ref: " + myOtherRefInvoice[0].OtherReference + "\n";
            TextBox4.Text += "Invoice GrossAmount: " + myOtherRefInvoice[0].GrossAmount + "\n";
        }
        catch (Exception ex)
        {
            TextBox4.Text += ex.Message;
        }
        finally
        {
            //always disconnect after we are finished.
            Disconnect();
        }

    }
}

