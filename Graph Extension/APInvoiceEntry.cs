using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.AP;
using PX.Objects.IN;
using System;
using System.Collections;
using System.Collections.Generic;

namespace TaxBundle
{

    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod 
    public class APInvoiceEntry_Extension : PXGraphExtension<PX.Objects.AP.APInvoiceEntry>
    {
        #region Event Handlers

        public PXAction<APInvoice> WithholdingTax;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Withholding Tax")]
        protected void withholdingTax()
        {

            var rowline = Base.Transactions.Cache;

            if (rowline is null)
                return;

            WHTax();

        }

        public PXAction<APInvoice> Print2307;
        [PXUIField(DisplayName = "Print 2307", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXLookupButton(Category = "Reports")]
        protected void print2307()
        {
            APInvoice doc = Base.Document.Current;
            if (doc.RefNbr != null)
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters["RefNbr"] = doc.RefNbr;
                throw new PXReportRequiredException(parameters, "TX642000", null);
            }
        }

        public void WHTax()
        {
            int? Intid;

            PXCache cache = Base.Transactions.Cache;

            InventoryItem wtax = PXSelect<InventoryItem, Where<InventoryItem.inventoryCD, Equal<Required<InventoryItem.inventoryCD>>>>.Select(Base, "WTAX");

            if (wtax != null)
                Intid = wtax.InventoryID;
            else
                return;

            foreach (APTran aPTran in Base.Transactions.Select())
            {
                var aPTranExt = PXCache<APTran>.GetExtension<APTranExt>(aPTran);

                if (aPTranExt.UsrWithholdingCD == null)
                    cache.SetValueExt<APTranExt.usrWithholdingCD>(aPTranExt, "N/A");

                if (aPTranExt.UsrWithholdingCD == null)
                    cache.SetValueExt<APTranExt.usrTaxDescription>(aPTranExt, "N/A");

                if (aPTran.InventoryID == Intid)
                    cache.Delete(aPTran);
            }

            APTran aPTran1 = new APTran();

            aPTran1.InventoryID = Intid;
            decimal? curyLineAmt = 0;
            decimal? prepayment = 0;
            bool? checkwtaxexist = false;

            foreach (APTran aPTran2 in Base.Transactions.Select())
            {
                var aPTranExt = PXCache<APTran>.GetExtension<APTranExt>(aPTran2);

                if (aPTranExt.UsrWithholdingCD != "N/A")
                    checkwtaxexist = true;

                var taxAmount = aPTranExt.UsrTaxAmount;
                curyLineAmt += taxAmount;

                prepayment = aPTran2.PrepaymentPct;
            }
            decimal? curyLineAmtTotal = Math.Round(curyLineAmt.Value, 2, MidpointRounding.ToEven);
            aPTran1.CuryLineAmt = curyLineAmtTotal * -1;
            aPTran1.PrepaymentPct = prepayment;
            //aPTran1.SubID = 2; //  Default Sub Account 0;

            if (checkwtaxexist != false)
                cache.Insert(aPTran1);
        }

        public delegate IEnumerable ReleaseDelegate(PXAdapter adapter);
        [PXOverride]
        public IEnumerable Release(PXAdapter adapter, ReleaseDelegate baseMethod)
        {
            CheckWTax();
            return baseMethod(adapter);
        }

        public void CheckWTax()
        {
            int? wTaxID = 0;
            bool wTaxExist = false;
            decimal? wTaxAmountTotal = 0;
            decimal? wTaxAmount = 0;
            decimal? wTaxAmountTotalFinal = 0;
            decimal? curyLineAmount = 0;
            decimal? curyLineAmountTotal = 0;
            string errorSaving = "Error on saving WTAX line click WTAX button";
            string errorDuplicate = "Duplicate Withholding Tax";
            List<int?> wTaxCount = new List<int?>();

            var row = Base.Transactions.Current;

            InventoryItem wTax = SelectFrom<InventoryItem>.Where<InventoryItem.inventoryCD.
                IsEqual<@P.AsString>>.View.Select(Base, "WTAX");

            if (wTax != null)
                wTaxID = wTax.InventoryID;

            foreach (APTran aPTran in Base.Transactions.Select())
            {
                if (aPTran.InventoryID == wTaxID)
                    wTaxCount.Add(aPTran.InventoryID);

            }

            foreach (APTran aPTran1 in Base.Transactions.Select())
            {
                APTranExt aPTranExt = aPTran1.GetExtension<APTranExt>();

                wTaxAmount = aPTranExt.UsrTaxAmount;

                if (aPTranExt.UsrWithholdingCD != null)
                    wTaxExist = true;

                if (wTaxCount.Count > 1)
                    throw new PXException(errorDuplicate);

                wTaxAmountTotal += wTaxAmount;
            }

            if (wTaxAmountTotal > 0)
            {
                wTaxAmountTotalFinal = wTaxAmountTotal * -1;

                foreach (APTran aPTran in Base.Transactions.Select())
                {
                    APTranExt aPTranExt = aPTran.GetExtension<APTranExt>();
                    if (aPTran.InventoryID == wTaxID)
                    {
                        curyLineAmount = aPTran.CuryLineAmt;
                        curyLineAmountTotal = Math.Round(wTaxAmountTotalFinal.Value, 2, MidpointRounding.ToEven);
                        if (curyLineAmountTotal == curyLineAmount)
                            return;
                        else if (wTaxExist == true)
                            throw new PXException(errorSaving);
                        else
                            throw new PXException(errorSaving);
                    }
                }
                if (row.InventoryID != wTaxID)
                    throw new PXException(errorSaving);
            }
        }

        protected void _(Events.RowSelected<APInvoice> e)
        {
            WithholdingTax.SetEnabled(e.Row.Status == APDocStatus.Hold);
        }

        protected void _(Events.FieldUpdated<APTran, APTran.inventoryID> e)
        {
            var row = e.Row;

            if (row.InventoryID != null)
            {
                InventoryItem item = PXSelectorAttribute.Select<APTran.inventoryID>(e.Cache, row) as InventoryItem;

                InventoryItemExt itemExt = item.GetExtension<InventoryItemExt>();
                APTranExt aPTranExt = row.GetExtension<APTranExt>();
                aPTranExt.UsrWithholdingCD = itemExt.UsrWithholdingCD;
            }
            e.Cache.SetDefaultExt<APTranExt.usrTaxRate>(e.Row);
            e.Cache.SetDefaultExt<APTranExt.usrTaxDescription>(e.Row);
        }

        protected void _(Events.FieldUpdated<APTran, APTranExt.usrWithholdingCD> e)
        {
            APTran aPTran = e.Row;
            APTranExt aPTranExt = aPTran.GetExtension<APTranExt>();
            WHTax wHTax = PXSelectorAttribute.Select<APTranExt.usrWithholdingCD>(e.Cache, aPTran) as WHTax;

            if (aPTranExt.UsrWithholdingCD != null)
                aPTranExt.UsrTaxRate = wHTax.TaxRate;

            e.Cache.SetDefaultExt<APTranExt.usrTaxRate>(e.Row);
            e.Cache.SetDefaultExt<APTranExt.usrTaxDescription>(e.Row);
        }

        protected void _(Events.FieldUpdated<APTran, APTranExt.usrTaxRate> e)
        {
            APTran aPTran = e.Row;

            if (aPTran != null)
            {
                APTranExt aPTranExt = aPTran.GetExtension<APTranExt>();
                if (aPTranExt != null)
                {
                    var curyTranAmount = aPTran.CuryTranAmt;
                    var taxRate = aPTranExt.UsrTaxRate;
                    var totalAmount = curyTranAmount * taxRate;
                    e.Cache.SetValue<APTranExt.usrTaxAmount>(aPTran, totalAmount);
                }
            }
        }

        protected void _(Events.FieldUpdated<APTran, APTran.curyTranAmt> e)
        {
            APTran aPTran = e.Row;
            if (aPTran != null)
            {
                APTranExt aPTranExt = aPTran.GetExtension<APTranExt>();
                if (aPTranExt != null)
                {
                    var curyTranAmount = aPTran.CuryTranAmt;
                    var taxRate = aPTranExt.UsrTaxRate;
                    var totalAmount = curyTranAmount * taxRate;
                    e.Cache.SetValue<APTranExt.usrTaxAmount>(aPTran, totalAmount);
                }
            }
        }
        #endregion
    }
}