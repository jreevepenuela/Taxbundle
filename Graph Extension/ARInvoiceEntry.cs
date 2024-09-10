using PX.Data;
using PX.Objects.AR;
using PX.Objects.IN;
using System;
using System.Collections;

namespace TaxBundle
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod 
    public class ARInvoiceEntry_Extension : PXGraphExtension<PX.Objects.AR.ARInvoiceEntry>
    {
        #region Event Handlers
        public PXAction<ARInvoice> WithholdingTax;
        [PXButton]
        [PXUIField(DisplayName = "Withholding Tax")]
        protected void withholdingTax()
        {
            int? Intid = 0;
            InventoryItem wtax = PXSelect<InventoryItem, Where<InventoryItem.inventoryCD, Equal<Required<InventoryItem.inventoryCD>>>>.Select(Base, "WTAX");

            if (wtax != null)
                Intid = wtax.InventoryID;

            PXCache cache = Base.Transactions.Cache;
            PXCache cache1 = Base.Document.Cache;
            PXCache cache2 = Base.Transactions.Cache;
            PXCache cache3 = Base.Adjustments_1.Cache;

            var status = Base.Document.Current;
            decimal? totalWHTax = 0;

            foreach (ARTran wTaxExist in Base.Transactions.Select())
            {
                if (wTaxExist.InventoryID == Intid)
                    return;
            }

            foreach (ARInvoice aRInvoice in Base.Document.Cache.Cached)
            {
                if (aRInvoice.DocType == "CRM")
                {
                    foreach (ARTran aRTran in Base.Transactions.Cache.Cached)
                    {
                        var aRTranExt = PXCache<ARTran>.GetExtension<ARTranExt>(aRTran);
                        totalWHTax += aRTranExt.UsrTaxAmount;
                        cache.Delete(aRTran);

                    }

                    decimal? WHTaxTotal = Math.Round(totalWHTax.Value, 2, MidpointRounding.ToEven);
                    cache1.SetValue<ARInvoice.docDesc>(aRInvoice, "Withholding Tax");
                    ARTran aRTran1 = new ARTran();
                    aRTran1.InventoryID = Intid;
                    aRTran1.CuryExtPrice = WHTaxTotal;
                    cache2.Insert(aRTran1);

                    ARAdjust aRAdjust = new ARAdjust();
                    aRAdjust.AdjdRefNbr = aRInvoice.OrigRefNbr;
                    aRAdjust.CuryAdjgAmt = WHTaxTotal;
                    cache3.Insert(aRAdjust);
                }
                else if (aRInvoice.Released == false)
                {
                    // Acuminator disable once PX1050 HardcodedStringInLocalizationMethod [Justification]
                    throw new PXException("Release and Reverse Before Applying WithHolding Tax");
                }
            }
        }

        public delegate IEnumerable ReleaseDelegate(PXAdapter adapter);
        [PXOverride]
        public IEnumerable Release(PXAdapter adapter, ReleaseDelegate baseMethod)
        {
            string errorSaving = "Error on saving WTAX line click WTAX button";

            foreach (ARInvoice aRInvoice in Base.Document.Cache.Cached)
            {
                if (aRInvoice != null)
                {
                    if (aRInvoice.DocType == "CRM")
                    {
                        foreach (ARTran aRTran in Base.Transactions.Cache.Cached)
                        {
                            ARTranExt aRTranExt = aRTran.GetExtension<ARTranExt>();
                            if (aRTranExt.UsrWithholdingCD != "N/A")
                                throw new PXException(errorSaving);
                        }
                    }

                }
            }

            return baseMethod(adapter);
        }

        protected void _(Events.RowSelected<ARInvoice> e)
        {
            var row = e.Row;
            string withHeld = "N/A";

            foreach (ARTran aRTran in Base.Transactions.Cache.Cached)
            {
                ARTranExt aRTranExt = aRTran.GetExtension<ARTranExt>();
                if (aRTranExt.UsrWithholdingCD != null)
                {
                    if (aRTranExt.UsrWithholdingCD != "N/A")
                    {
                        withHeld = aRTranExt.UsrWithholdingCD;
                        break;
                    }
                }


            }

            if (row.DocType == "INV" || row.DocType == "DRM" ||
                row.DocType == "FCH" || row.DocType == "SMC")
                WithholdingTax.SetEnabled(false);
            else if ((row.DocType == "CRM" && row.Status == "H") && withHeld != "N/A")
                WithholdingTax.SetEnabled(true);
            else
                WithholdingTax.SetEnabled(false);

            if (row.Released == true)
                WithholdingTax.SetEnabled(false);

            if (row.Status == "B")
                WithholdingTax.SetEnabled(false);
        }

        protected void _(Events.FieldUpdated<ARTran, ARTran.inventoryID> e)
        {
            var row = e.Row;

            if (row.InventoryID != null)
            {
                InventoryItem item = PXSelectorAttribute.Select<ARTran.inventoryID>(e.Cache, row) as InventoryItem;

                InventoryItemExt itemExt = item.GetExtension<InventoryItemExt>();
                ARTranExt aRTranExt = row.GetExtension<ARTranExt>();
                aRTranExt.UsrWithholdingCD = itemExt.UsrWithholdingCD;
            }
            e.Cache.SetDefaultExt<ARTranExt.usrTaxRate>(e.Row);
            e.Cache.SetDefaultExt<ARTranExt.usrTaxDescription>(e.Row);
        }

        protected void _(Events.FieldUpdated<ARTran, ARTranExt.usrWithholdingCD> e)
        {
            ARTran aRTran = e.Row;
            ARTranExt aRTranExt = aRTran.GetExtension<ARTranExt>();
            WHTax wHTax = PXSelectorAttribute.Select<ARTranExt.usrWithholdingCD>(e.Cache, aRTran) as WHTax;

            if (aRTranExt.UsrWithholdingCD != null)
                aRTranExt.UsrTaxRate = wHTax.TaxRate;

            e.Cache.SetDefaultExt<ARTranExt.usrTaxRate>(e.Row);
            e.Cache.SetDefaultExt<ARTranExt.usrTaxDescription>(e.Row);
        }

        protected void _(Events.FieldUpdated<ARTran, ARTranExt.usrTaxRate> e)
        {
            ARTran aRTran = e.Row;

            if (aRTran != null)
            {
                ARTranExt aRTranExt = aRTran.GetExtension<ARTranExt>();
                if (aRTranExt != null)
                {
                    var curyTranAmount = aRTran.CuryTranAmt;
                    var taxRate = aRTranExt.UsrTaxRate;
                    var totalAmount = curyTranAmount * taxRate;
                    e.Cache.SetValue<ARTranExt.usrTaxAmount>(aRTran, totalAmount);
                }
            }
        }

        protected void _(Events.FieldUpdated<ARTran, ARTran.curyTranAmt> e)
        {
            ARTran aRTran = e.Row;
            if (aRTran != null)
            {
                ARTranExt aRTranExt = aRTran.GetExtension<ARTranExt>();
                if (aRTranExt != null)
                {
                    var curyTranAmount = aRTran.CuryTranAmt;
                    var taxRate = aRTranExt.UsrTaxRate;
                    var totalAmount = curyTranAmount * taxRate;
                    e.Cache.SetValue<ARTranExt.usrTaxAmount>(aRTran, totalAmount);
                }
            }
        }
        #endregion
    }
}