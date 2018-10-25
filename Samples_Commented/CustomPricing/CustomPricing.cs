using System;
using Pageflex.Interfaces.Storefront;

namespace CustomPricing
{

    /// <summary>
    /// This sample module overrides all Storefront price tables with its own price calculation.
    /// </summary>


    public class CustomPricing : StorefrontExtension
    {

        #region At minimum an extension must ovveride these two methods

        // These methods set the display and unique names on the extension page in the storefront

        public override string UniqueName
        {
            get { return "CustomPricing.pricing.samples.storefront.pageflex.com"; }
        }

        public override string DisplayName
        {
            get { return "CustomPricing"; }
        }

        #endregion


        #region This section is called when the document prices need to be calculated

        // Invoked from the Printing Step, the Shopping Cart, and the Checkout Submit pages when the document prices need to be calculated.
        // A module that implements this event can override the price and taxable portion of a document.


        //this method has been depreciated and is now using "CalculateDocumentPrices3"
        public override int CalculateDocumentPrices(string[] docIds, bool pricesOnly, out double[] prices, out string isoCurrencyCode)
        {
            isoCurrencyCode = "USD";
            prices = new double[docIds.Length];
            ComputeDocumentPrices(docIds, prices);
            return eSuccess;
        }

        // This is the new method to use
        // public override int CalculateDocumentPrices3( string[] docIds, bool pricesOnly, TaxLocation taxLoc, ref double[] docPrices, ref double[] taxable, out string isoCurrencyCode){}

        #endregion


        #region This section is created to compute the document prices

        protected void ComputeDocumentPrices(string[] docids, double[] prices)
        {
            try 
            {
                for (int i = 0; i < docids.Length; i++)
                {
                    string id = (string)docids[i];

                    string n = "";

                    string q = Storefront.GetValue("PrintingField","PrintingQuantity",id);
                    if (q == null || q == "")
                        q = "1";
                    string db = Storefront.GetValue("DocumentProperty","DatabaseToMerge_AssetID",id);
                    if (db != null && db != "")
                        n = Storefront.GetValue("AssetProperty","NumberOfRows", db);

                    if (n == null || n == "")
                        n = "1";

                    int ncopies  = int.Parse(q) * int.Parse(n);
                
                    prices[i] = ncopies * 9.00;  // $9.00 each
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }

        #endregion


    }
}
