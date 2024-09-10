using PX.Data;

namespace TaxBundle
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod 
    public sealed class InventoryItemExt : PXCacheExtension<PX.Objects.IN.InventoryItem>
    {
        #region UsrWithholdingCD
        [PXDBString(255)]
        [PXUIField(DisplayName = "Withholding Tax Code")]
        [PXSelector(
        typeof(Search<WHTax.withholdingCD>),
            typeof(WHTax.description))]
        public string UsrWithholdingCD { get; set; }
        public abstract class usrWithholdingCD : PX.Data.BQL.BqlString.Field<usrWithholdingCD> { }
        #endregion
    }
}