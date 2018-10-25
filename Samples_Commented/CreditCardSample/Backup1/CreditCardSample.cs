using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using Pageflex.Interfaces.Storefront;


namespace CreditCardSample
{
    /// <summary>
    /// Summary description for CreditCardSample.
    /// </summary>

    public class CreditCardSample : StorefrontExtension
    {
        public override string DisplayName
        {
            get
            {
                return "CreditCardSample (testing only)";
            }
        }

        public override string UniqueName
        {
            get
            {
                return "creditCardSample.payment.testing.storefront.pageflex.com";
            }
        }

        public override bool IsModuleType(string x)
        {
            if (x.ToLower().Equals("payment"))
                return true;
            if (x.ToLower().Equals("collection"))
                return true;

            return base.IsModuleType (x);
        }

        public override int EnumeratePickLists(out string[] lists)
        {
            lists = new string[1];
            lists[0] = "Available Payment Methods";
            return eSuccess;
        }

        public override int GetPickListData(string listname, 
            string fieldType, string objectId, bool forDisplayOnly, bool forUserSite, 
            out PickListItem[] items)
        {
            ArrayList answer = new ArrayList();
            if (listname == "Available Payment Methods")
            {
                PickListItem pi = new PickListItem();
                pi.name  = "Credit Card";
                pi.val   = "Credit Card";
                pi.selected = true;
                answer.Add(pi);
            }

            items = (PickListItem[])answer.ToArray(typeof(PickListItem));
            return eSuccess;
        }
        
        public override int AuthorizeTransaction(
            PaymentRequest ccrq, 
            out PaymentResponse ccs)
        {
            if (ccrq.methodOfPayment != "Credit Card")
            {
                ccs = new PaymentResponse();
                ccs.result = ccUNIMPLEMENTED;
                return eFailure;  // we can't handle this
            }

            // this module approves everything
            //
            ccs = new PaymentResponse();
            ccs.addrOK = ccSUCCESS;
            ccs.cvvOK  = ccSUCCESS;
            ccs.zipOK  = ccSUCCESS;
            ccs.result = ccSUCCESS;
            ccs.message = "Approved";
            ccs.TransactionID   = Guid.NewGuid().ToString();
            ccs.CollectionToken = ccs.TransactionID;
            return eSuccess;
        }

        // And collects anything.
        //
        public override int DelayedCapture(
            string captureToken, 
            double amount, 
            string isoCurrencyCode, 
            out PaymentResponse ccs)
        {
            ccs = new PaymentResponse();
            ccs.addrOK = ccUNAVAILABLE;
            ccs.cvvOK  = ccUNAVAILABLE;
            ccs.zipOK  = ccUNAVAILABLE;
            ccs.result = ccSUCCESS;
            ccs.message = "Approved";
            ccs.TransactionID   = Guid.NewGuid().ToString();
            ccs.CollectionToken = ccs.TransactionID.Replace("-"," ");
            return eSuccess;
        }

        public override int VoidTransaction(
            string transactionID, 
            out PaymentResponse ccs)
        {
            ccs = new PaymentResponse();
            ccs.addrOK = ccUNAVAILABLE;
            ccs.cvvOK  = ccUNAVAILABLE;
            ccs.zipOK  = ccUNAVAILABLE;
            ccs.result = ccSUCCESS;

            ccs.TransactionID   = Guid.NewGuid().ToString();
            return eSuccess;
        }

        public override int CouldCollect(string orderID)
        {
            string sxiname = Storefront.GetValue("OrderProperty", "SxiModuleForCollection", orderID);
            if (sxiname == this.UniqueName)
                return eSuccess;
            return eFailure;
        }

    }
}
