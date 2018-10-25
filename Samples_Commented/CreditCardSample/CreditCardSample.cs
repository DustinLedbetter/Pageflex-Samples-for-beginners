using System;
using System.Collections;
using Pageflex.Interfaces.Storefront;


namespace CreditCardSample
{

    /// <summary>
    /// This is an example of an extension for use with handling credit cards on the storefront.
    /// </summary>


    public class CreditCardSample : StorefrontExtension
    {

        #region At minimum an extension must ovveride these two methods
        
        // These methods set the display and unique names on the extension page in the storefront

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

        #endregion


        #region This section is used to determine if the user is in the payment or collection section where credit card handling occurs

        // If we are in either section on the storefront then we return true so we know to begin processing credit card methods
        public override bool IsModuleType(string x)
        {
            if (x.ToLower().Equals("payment"))
                return true;
            if (x.ToLower().Equals("collection"))
                return true;

            return base.IsModuleType (x);
        }

        #endregion


        #region This section is used to tell the names of pick lists this module can supply data for.

        public override int EnumeratePickLists(out string[] lists)
        {

            // Set the list for return with the Pick List name “Available Payment Methods”
            lists = new string[1];
            lists[0] = "Available Payment Methods";

            // Return eSuccess to indicate that the pick list names have been set.
            return eSuccess;
        }

        #endregion


        #region  Gets the pick list items for the given list: "Available Payment Methods".
        
        public override int GetPickListData(string listname, string fieldType, string objectId, bool forDisplayOnly, bool forUserSite, out PickListItem[] items)
        {

            // Create an array list to hold items
            ArrayList answer = new ArrayList();

            // Check to see if the listname "Available Payment Methods" exists
            if (listname == "Available Payment Methods")
            {

                // Add a type of payment to the list
                PickListItem pi = new PickListItem();
                pi.name  = "Credit Card";
                pi.val   = "Credit Card";
                pi.selected = true;
                answer.Add(pi);
            }

            // Add the list to the "items" list to return to the storefront
            items = (PickListItem[])answer.ToArray(typeof(PickListItem));

            // Return eSuccess to indicate that the pick list items have been set.
            return eSuccess;
        }

        #endregion

        #region This section checks the authorization of a transaction

        public override int AuthorizeTransaction( PaymentRequest ccrq, out PaymentResponse ccs)
        {

            // Check to see if the method of payment is not a credit card
            if (ccrq.methodOfPayment != "Credit Card")
            {
                // If it is not then we can't handle this process
                ccs = new PaymentResponse();
                ccs.result = ccUNIMPLEMENTED;
                return eFailure;  // we can't handle this
            }

            // If it is the we approve all the fields
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

        // And collects any other data as needed
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

        #endregion


        #region This section is used to void a transaction

        public override int VoidTransaction(string transactionID, out PaymentResponse ccs)
        {
            // The module should attempt to void the transaction and return eSuccess, with the ccrsp field containing the result of the void transaction.
            ccs = new PaymentResponse();
            ccs.addrOK = ccUNAVAILABLE;
            ccs.cvvOK  = ccUNAVAILABLE;
            ccs.zipOK  = ccUNAVAILABLE;
            ccs.result = ccSUCCESS;

            ccs.TransactionID   = Guid.NewGuid().ToString();
            return eSuccess;
        }

        #endregion


        #region This section checks to see if the order can be collected on

        public override int CouldCollect(string orderID)
        {

            // Asks the module whether it could collect on the order. 
            // This controls enabling of  the Collect Payment button in the Orders and Order Details pages on the Administrator site.
            string sxiname = Storefront.GetValue("OrderProperty", "SxiModuleForCollection", orderID);
            if (sxiname == this.UniqueName)
                return eSuccess;
            return eFailure;
        }

        #endregion


    }
}
