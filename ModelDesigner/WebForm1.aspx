<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WebForm1.aspx.vb" Inherits="MvcApplication1.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:PricingDatabaseConnectionString %>" 
            DeleteCommand="DELETE FROM [_FoodType] WHERE [ID] = @ID" 
            InsertCommand="INSERT INTO [_FoodType] ([Name], [IDGroup], [upperName]) VALUES (@Name, @IDGroup, @upperName)" 
            SelectCommand="SELECT * FROM [_FoodType]" 
            UpdateCommand="UPDATE [_FoodType] SET [Name] = @Name, [IDGroup] = @IDGroup, [upperName] = @upperName WHERE [ID] = @ID">
            <DeleteParameters>
                <asp:Parameter Name="ID" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="Name" Type="String" />
                <asp:Parameter Name="IDGroup" Type="Int32" />
                <asp:Parameter Name="upperName" Type="String" />
                <asp:Parameter Name="ID" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="Name" Type="String" />
                <asp:Parameter Name="IDGroup" Type="Int32" />
                <asp:Parameter Name="upperName" Type="String" />
            </InsertParameters>
        </asp:SqlDataSource>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
            DataKeyNames="ID" DataSourceID="SqlDataSource1">
            <Columns>
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" 
                    ReadOnly="True" SortExpression="ID" />
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="IDGroup" HeaderText="IDGroup" 
                    SortExpression="IDGroup" />
                <asp:BoundField DataField="upperName" HeaderText="upperName" ReadOnly="True" 
                    SortExpression="upperName" />
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
