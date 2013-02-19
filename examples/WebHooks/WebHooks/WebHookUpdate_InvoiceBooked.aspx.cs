#region using
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;

using Economic.Api;
using Economic.Api.Data;
using Economic.Api.Exceptions;
#endregion using

namespace WebHooks
{
    /// <summary>
    /// Example of a page that does some work based on a webhook request received from e-conomic
    /// </summary>
    public partial class WebHookUpdate_InvoiceBooked : System.Web.UI.Page
    {       
        private int _invNo = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["invoiceno"] != null)
            {
                //a new invlice has been created at e-conomic
                _invNo = int.Parse(Request.QueryString["invoiceno"]);

                //spawn thread to handle work as the actual work will take some time 
                //and e-conomic needs an answer that we have received the WebHook
                Thread t = new Thread(CreateCashBookEntry);     
                t.Start();             
            }
        }

        #region connect / disconnect

        private void Connect(EconomicSession session)
        {
            session.Connect(179476, "CES", "c97jxsq8");
        }

        private void Disconnect(EconomicSession session)
        {          
            session.Disconnect();            
        }

        #endregion connect / disconnect

        #region work

        /// <summary>
        /// Create a cashbookentry at e-conomic
        /// </summary>
        private void CreateCashBookEntry()
        {
            EconomicSession session = new EconomicSession();
                         
            Connect(session);                

            //get the invoice from e-conomic
            IInvoice invoice = session.Invoice.FindByNumber(_invNo);

            //get cashbook and contraaccount from e-conomic
            ICashBook cashBook = session.CashBook.GetAll()[0];
            IAccount contraAccount = session.Account.FindByNumber(1012);

            //create a debtorpayment for the invoice
            ICashBookEntryData cashBookEntryData = session.CashBookEntryData.Create(CashBookEntryType.DebtorPayment, cashBook, invoice.Debtor, null, null, contraAccount);
            cashBookEntryData.Date = DateTime.Today;
            cashBookEntryData.VoucherNumber = (new Random().Next(1000000)) + 1;
            cashBookEntryData.Currency = session.Currency.FindByCode("DKK");
            cashBookEntryData.Amount = invoice.GrossAmount;
            cashBookEntryData.AmountDefaultCurrency = invoice.GrossAmount;
            cashBookEntryData.Text = "Payment, Invoice: " + invoice.Number;
            ICashBookEntry entry = session.CashBookEntry.CreateFromData(cashBookEntryData);
            entry.DebtorInvoiceNumber = invoice.Number;                               
         
            Disconnect(session);           
        }

        #endregion work

    }
}