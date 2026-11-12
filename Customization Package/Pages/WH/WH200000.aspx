<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="WH200000.aspx.cs" Inherits="Page_WH200000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="TaxBundle.WHTaxMaint"
        PrimaryView="Tax"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phL" runat="Server">
	<px:PXGrid AutoAdjustColumns="True" ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Primary" AllowAutoHide="false">
		<Levels>
			<px:PXGridLevel DataMember="Tax">
			    <Columns>
				<px:PXGridColumn DataField="WithholdingCD" Width="280" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Type" Width="280" ></px:PXGridColumn>
				<px:PXGridColumn DataField="TaxRate" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Description" Width="280" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Birform" Width="280" ></px:PXGridColumn></Columns>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" ></AutoSize>
		<ActionBar >
		</ActionBar>
	
		<Mode AllowUpload="True" />
		<Mode AutoInsert="True" />
		<Mode AllowUpdate="True" /></px:PXGrid>
</asp:Content>
