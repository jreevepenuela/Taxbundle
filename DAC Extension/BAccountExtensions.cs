using PX.Data;

namespace TaxBundle
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod 
    public sealed class BAccountExt : PXCacheExtension<PX.Objects.CR.BAccount>
    {
        #region UsrFirstName
        [PXDBString(255)]
        [PXUIField(DisplayName = "First Name")]

        public string UsrFirstName { get; set; }
        public abstract class usrFirstName : PX.Data.BQL.BqlString.Field<usrFirstName> { }
        #endregion

        #region UsrMiddleName
        [PXDBString(255)]
        [PXUIField(DisplayName = "Middle Name")]

        public string UsrMiddleName { get; set; }
        public abstract class usrMiddleName : PX.Data.BQL.BqlString.Field<usrMiddleName> { }
        #endregion

        #region UsrLastName
        [PXDBString(255)]
        [PXUIField(DisplayName = "Last Name")]

        public string UsrLastName { get; set; }
        public abstract class usrLastName : PX.Data.BQL.BqlString.Field<usrLastName> { }
        #endregion
    }
}