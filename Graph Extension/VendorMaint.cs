using PX.Data;
using PX.Objects.AP;
using PX.Objects.CR;

namespace TaxBundle
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod 
    public class VendorMaint_Extension : PXGraphExtension<PX.Objects.AP.VendorMaint>
    {
        #region Event Handlers

        public string fname = "";
        public string mname = "";
        public string lname = "";
        public string stringbuild;
        protected void _(Events.RowSelected<VendorR> e)
        {

            var row = (VendorR)e.Row;
            if (row != null)
            {
                BAccountExt bAccountExt = row.GetExtension<BAccountExt>();
                if (bAccountExt != null)
                {
                    lname = bAccountExt.UsrLastName;
                }
                foreach (VendorR rowxx in Base.BAccount.Cache.Cached)
                {
                    var rowExxT = PXCache<BAccount>.GetExtension<BAccountExt>(rowxx);
                    var firstn = rowExxT.UsrFirstName;
                    var midn = rowExxT.UsrMiddleName;
                    var lasn = rowExxT.UsrLastName;
                    stringbuild = firstn + " " + midn + " " + lasn;
                }

                if (!string.IsNullOrWhiteSpace(stringbuild))
                {
                    // Acuminator disable once PX1047 RowChangesInEventHandlersForbiddenForArgs [Justification]
                    e.Cache.SetValue<VendorR.acctName>(row, stringbuild);
                }
            }

        }

        #endregion
    }
}