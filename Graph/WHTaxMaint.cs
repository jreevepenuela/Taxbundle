using PX.Data;
using PX.Data.BQL.Fluent;

namespace TaxBundle
{
    public class WHTaxMaint : PXGraph<WHTaxMaint>
    {
        [PXImport()]
        public SelectFrom<WHTax>.View Tax;

        public PXSave<WHTax> Save;
        public PXCancel<WHTax> Cancel;
    }
}