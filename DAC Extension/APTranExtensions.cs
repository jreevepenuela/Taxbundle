using PX.Data;
using PX.Objects.AP;
using PX.Objects.IN;
using System;

namespace TaxBundle
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod 
    public sealed class APTranExt : PXCacheExtension<APTran>
    {
        #region UsrWithholdingCD
        [PXDBString(255)]
        [PXUIField(DisplayName = "Withholding Tax Code")]
        [PXDefault(TypeCode.String, "N/A", typeof(Search<InventoryItemExt.usrWithholdingCD,
            Where<InventoryItem.inventoryID, Equal<Current<APTran.inventoryID>>>>),
            PersistingCheck = PXPersistingCheck.Nothing)]
        [PXSelector(
        typeof(Search<WHTax.withholdingCD>),
        typeof(WHTax.description),
            typeof(WHTax.taxRate))]
        public string UsrWithholdingCD { get; set; }
        public abstract class usrWithholdingCD : PX.Data.BQL.BqlString.Field<usrWithholdingCD> { }
        #endregion

        #region UsrTaxRate
        [PXDBDecimal(2)]
        [PXUIField(DisplayName = "Withholding Tax Rate", IsReadOnly = true)]
        [PXDefault(TypeCode.Decimal, "0.0", typeof(Search<WHTax.taxRate,
            Where<WHTax.withholdingCD, Equal<Current<APTranExt.usrWithholdingCD>>>>),
            PersistingCheck = PXPersistingCheck.Nothing)]
        public Decimal? UsrTaxRate { get; set; }
        public abstract class usrTaxRate : PX.Data.BQL.BqlDecimal.Field<usrTaxRate> { }
        #endregion

        #region UsrTaxAmount
        [PXDBDecimal(4)]
        [PXUIField(DisplayName = "Withholding Tax Amount", IsReadOnly = true)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public Decimal? UsrTaxAmount { get; set; }
        public abstract class usrTaxAmount : PX.Data.BQL.BqlDecimal.Field<usrTaxAmount> { }
        #endregion

        #region UsrTaxDescription
        [PXDBString(255)]
        [PXUIField(DisplayName = "Withholding Tax Description", IsReadOnly = true)]
        [PXDefault(TypeCode.String, "N/A", typeof(Search<WHTax.description,
            Where<WHTax.withholdingCD, Equal<Current<APTranExt.usrWithholdingCD>>>>),
            PersistingCheck = PXPersistingCheck.Nothing)]
        public string UsrTaxDescription { get; set; }
        public abstract class usrTaxDescription : PX.Data.BQL.BqlString.Field<usrTaxDescription> { }
        #endregion
    }
}