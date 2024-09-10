using System;
using PX.Data;

namespace TaxBundle
{
    [PXCacheName("Withholding Tax")]
    public class WHTax : IBqlTable
    {
        #region WithholdingID
        [PXDBIdentity]
        public virtual int? WithholdingID { get; set; }
        public abstract class withholdingID : PX.Data.BQL.BqlInt.Field<withholdingID> { }
        #endregion

        #region WithholdingCD
        [PXDBString(255, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXDefault]
        [PXUIField(DisplayName = "Withholding Tax Code")]
        public virtual string WithholdingCD { get; set; }
        public abstract class withholdingCD : PX.Data.BQL.BqlString.Field<withholdingCD> { }
        #endregion

        #region Type
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Type")]
        public virtual string Type { get; set; }
        public abstract class type : PX.Data.BQL.BqlString.Field<type> { }
        #endregion

        #region Description
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Description")]
        public virtual string Description { get; set; }
        public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
        #endregion

        #region TaxRate
        [PXDBDecimal(2)]
        [PXUIField(DisplayName = "Tax Rate")]
        public virtual Decimal? TaxRate { get; set; }
        public abstract class taxRate : PX.Data.BQL.BqlDecimal.Field<taxRate> { }
        #endregion

        #region Birform
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Bir Form")]
        public virtual string Birform { get; set; }
        public abstract class birform : PX.Data.BQL.BqlString.Field<birform> { }
        #endregion

        #region CreatedDateTime
        [PXDBCreatedDateTime()]
        public virtual DateTime? CreatedDateTime { get; set; }
        public abstract class createdDateTime : PX.Data.BQL.BqlDateTime.Field<createdDateTime> { }
        #endregion

        #region CreatedByID
        [PXDBCreatedByID()]
        public virtual Guid? CreatedByID { get; set; }
        public abstract class createdByID : PX.Data.BQL.BqlGuid.Field<createdByID> { }
        #endregion

        #region CreatedByScreenID
        [PXDBCreatedByScreenID()]
        public virtual string CreatedByScreenID { get; set; }
        public abstract class createdByScreenID : PX.Data.BQL.BqlString.Field<createdByScreenID> { }
        #endregion

        #region LastModifiedDateTime
        [PXDBLastModifiedDateTime()]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime> { }
        #endregion

        #region LastModifiedByID
        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID { get; set; }
        public abstract class lastModifiedByID : PX.Data.BQL.BqlGuid.Field<lastModifiedByID> { }
        #endregion

        #region LastModifiedByScreenID
        [PXDBLastModifiedByScreenID()]
        public virtual string LastModifiedByScreenID { get; set; }
        public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field<lastModifiedByScreenID> { }
        #endregion

        #region Tstamp
        [PXDBTimestamp()]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
        #endregion

        #region Noteid
        [PXNote()]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
        #endregion
    }
}