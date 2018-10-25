using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;

using Pageflex.Interfaces.Storefront;

namespace CustomPricing
{
    // This sample module overrides all Storefront price tables with its own
    //   price calculation.
    //
    public class CustomPricing : StorefrontExtension
    {
        public override string UniqueName
        {
            get { return "CustomPricing.pricing.samples.storefront.pageflex.com"; }
        }

        public override string DisplayName
        {
            get { return "CustomPricing"; }
        }


        public override int CalculateDocumentPrices(
            string[] docIds, 
            bool pricesOnly, 
            out double[] prices, 
            out string isoCurrencyCode)
        {
            isoCurrencyCode = "USD";
            prices = new double[docIds.Length];
            ComputeDocumentPrices(docIds, prices);
            return eSuccess;
        }


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


    }
}
