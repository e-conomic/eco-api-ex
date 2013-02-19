<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 135px;
        }
        .style2
        {
            width: 135px;
            height: 30px;
        }
        .alignBottom
        {
            vertical-align:bottom;
        }
        .style7
        {
            height: 30px;
            width: 513px;
        }
        .style8
        {
            width: 513px;
        }
        .table
        {
            width: 600px; 
            padding:0px; 
            margin:0px; 
            border-spacing:0;
        }
        .table td
        {
            padding:0px; 
            margin:0px; 
            border-spacing:0;
            width:200px;   
        }
        .style9
        {
            height: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width:100%;">
        <tr>
            <td class="style2">    
                <asp:Label ID="Label1" runat="server" Text="Agreementnumber:"></asp:Label>
            </td>
            <td class="style7">
                <asp:TextBox ID="TextBox1" runat="server" Width="200px">179476</asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="TextBox1" 
                    ErrorMessage="* Agreementnumber required (6-digits)"></asp:RequiredFieldValidator>
            </td>            
        </tr>
        <tr>
            <td class="style1">
                <asp:Label ID="Label2" runat="server" Text="UserID"></asp:Label>
            </td>
            <td class="style8">
                <asp:TextBox ID="TextBox2" runat="server" Width="200px">CES</asp:TextBox>    
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ErrorMessage="* Username required" ControlToValidate="TextBox2"></asp:RequiredFieldValidator>
            </td>            
        </tr>
        <tr>
            <td>        
                <asp:Label ID="Label3" runat="server" Text="PWD"></asp:Label>        
            </td>
            <td>     
                <asp:TextBox ID="TextBox3" runat="server" Width="200px">c97jxsq8</asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ErrorMessage="* Password required" ControlToValidate="TextBox3"></asp:RequiredFieldValidator>
            </td>            
        </tr>
        <tr>
            <td>        
                Connection</td>
            <td>        
                <asp:Button ID="Button1" runat="server" Text="Connect" onclick="Button1_Click" Width="200px" BackColor="#CCFF66"/>                
                &nbsp;                
                <asp:Button ID="Button2" runat="server" Text="Reset Textbox" 
                    onclick="Button2_Click" Width="200px" BackColor="#CCFF66"/>                    
            </td>            
        </tr>
        <tr>
            <td>Currency</td>                
            <td>
                <asp:Button ID="Button3" runat="server" Text="Get Base Currency" onclick="Button3_Click" Width="200px" BackColor="#CCFF66"/>
            </td>
        </tr>
         <tr>
                <td class="style9">Account</td>                
                <td class="style9">                
                    <asp:Button ID="Button4" runat="server"  Text="Get Accounts" onclick="Button4_Click" Width="200px" BackColor="#CCFF66"/>                    
                </td>
        </tr>
        <tr>
                <td>Order</td>                
                <td >                
                    <asp:Button ID="Button5" runat="server"  Text="Get Orders" onclick="Button5_Click" Width="200px" BackColor="#CCFF66"/>                    
                </td>
        </tr>
        <tr>
                <td>CurrentInvoice/Invoice</td>                
                <td >                
                    <table class="table">                    
                        <tr>
                            <td>
                            </td>
                            <td>                                
                                <table>
                                    <tr>
                                        <td>Other Ref:</td>
                                        <td><asp:TextBox ID="TextBox6" runat="server" 
                                        style="margin-bottom: 0px" Height="16px" Width="100px"></asp:TextBox></td>
                                    </tr>
                                </table>    
                            </td>
                            <td>                                
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <asp:Button ID="Button6" runat="server"  Text="Get CurrentInvoices" onclick="Button6_Click" Width="200px" BackColor="#CCFF66"/>                    
                            </td>
                            <td >                              
                                <asp:Button ID="Button17" runat="server" 
                                    Text="Find Booked Invoice by Other Ref" Width="200px" BackColor="#CCFF66" 
                                    onclick="Button17_Click" />
                            </td>
                            <td >
                                <asp:Button ID="Button16" runat="server" Text="Create and Book CurrentInvoice" 
                                    Width="200px" BackColor="#0099FF" onclick="Button16_Click" 
                                    style="height: 26px"/>
                            </td>
                        </tr>                    
                    </table>                                    
                </td>
        </tr>
        <tr>
            <td>Debtor</td>                
            <td>
                <table class="table">                        
                    <tr>
                        <td >
                            <asp:Button ID="Button7" runat="server" Text="Get Debtors" 
                            onclick="Button7_Click" Width="200px" BackColor="#CCFF66"/>
                        </td>
                        <td >
                            <asp:Button ID="Button8" runat="server" Text="Get Next DebtorNumber" 
                            onclick="Button8_Click" Width="200px" BackColor="#CCFF66"/>    
                        </td>
                        <td >
                            <asp:Button ID="Button9" runat="server" Text="Get Debtor Entries" 
                            onclick="Button9_Click" Width="200px" BackColor="#CCFF66"/>             
                        </td>
                    </tr>                        
                </table>                                     
            </td>
        </tr>
        <tr>
            <td>Creditor</td>                
            <td>
                <asp:Button ID="Button10" runat="server" Text="Get Creditors" 
                    onclick="Button10_Click" Width="200px" BackColor="#CCFF66"/>
            </td>
        </tr>
        <tr>
            <td>Subscription</td>
            <td>
                <asp:Button ID="Button11" runat="server" Text="Get Subscriptions" 
                    onclick="Button11_Click" Width="200px" BackColor="#CCFF66"/>
            </td>
        </tr>
        <tr>
            <td>Project</td>
            <td>
                <asp:Button ID="Button12" runat="server" Text="Get Projects" 
                    onclick="Button12_Click" Width="200px" BackColor="#CCFF66"/>
            </td>
        </tr>    
        <tr>
            <td>Product</td>
            <td style="width:200px">
                <table class="table">
                    <tr>
                        <td>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>ProductName:</td>
                                    <td><asp:TextBox ID="TextBox5" runat="server" 
                                    style="margin-bottom: 0px" Height="16px" Width="100px"></asp:TextBox></td>
                                </tr>
                            </table>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td >
                        <asp:Button ID="Button13" runat="server" Text="Get Products" 
                    onclick="Button13_Click" Width="200px" BackColor="#CCFF66" CssClass="alignBottom"/>
                        </td>
                        <td >
                           
                        <asp:Button ID="Button14" runat="server" Text="Create New Product" 
                    onclick="Button14_Click" Width="200px" BackColor="#0099FF" CssClass="alignBottom"/>
                        </td>
                        <td >
                            
                        <asp:Button ID="Button15" runat="server" Text="Delete Last Product" 
                    onclick="Button15_Click" Width="200px" BackColor="#FF5050" CssClass="alignBottom"/>                    
                        </td>
                    </tr>
                    
                </table>
            </td>            
        </tr>    
        <tr>
            <td></td>
            <td></td>
        </tr>    
        </table>
        
        Result:<br />
        <asp:TextBox ID="TextBox4" runat="server" Height="243px" Width="100%" TextMode="MultiLine"></asp:TextBox>
    </div>
    </form>
</body>
</html>
