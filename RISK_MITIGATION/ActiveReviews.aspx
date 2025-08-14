<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActiveReviews.aspx.cs" Inherits="RiskReviewForm.ActiveReviews" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Active Risk Reviews</title>
    <style>
        table, th, td {
            border: 1px solid black;
            border-collapse: collapse;
            padding: 5px;
        }

        .details-row {
            background-color: #f9f9f9;
        }

        h2 {
            margin-bottom: 20px;
        }

        .grid-container {
            width: 100%;
            overflow-x: auto;
        }

        .action-buttons input[type="submit"] {
            margin-right: 5px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-bottom: 15px;">
    <a href="RiskForm.aspx">New Risk Form</a> |

    <a href="ClosedReviews.aspx">Closed Reviews</a>
</div>

        <div class="grid-container">
            <h2>Active Risk Reviews</h2>

            <asp:GridView ID="GridViewActive" runat="server" AutoGenerateColumns="False"
                DataKeyNames="RiskId"
                OnRowCommand="GridViewActive_RowCommand"
                OnSelectedIndexChanged="GridViewActive_SelectedIndexChanged"
                GridLines="Both">

                <Columns>
                    <asp:BoundField DataField="RiskId" HeaderText="Risk ID" />
                    <asp:BoundField DataField="TokenId" HeaderText="Token ID" />
                    <asp:BoundField DataField="SubKeyProcess" HeaderText="Sub & Key Process" />
                    <asp:BoundField DataField="InterestedParty" HeaderText="Interested Party" />
                    <asp:BoundField DataField="RiskStatement" HeaderText="Risk Statement" />
                    <asp:BoundField DataField="Owners" HeaderText="Owners" />
                    <asp:BoundField DataField="OpportunityIdentified" HeaderText="Opportunity" />
                    <asp:BoundField DataField="Likelihood" HeaderText="Likelihood" />
                    <asp:BoundField DataField="Impact" HeaderText="Impact" />
                    <asp:BoundField DataField="Score" HeaderText="Score" />
                    <asp:BoundField DataField="ReviewType" HeaderText="Review Type" />
                    <asp:BoundField DataField="ImpactDetails" HeaderText="Impact Details" />
                    <asp:BoundField DataField="MitigationPlan" HeaderText="Mitigation Plan" />
                    <asp:BoundField DataField="TargetDate" HeaderText="Target Date" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" />

                    <asp:TemplateField HeaderText="Actions">
    <ItemTemplate>
        <div class="action-buttons">
            <asp:Button ID="btnEdit" runat="server" CommandName="EditEntry" Text="Edit"
                        CommandArgument='<%# Container.DataItemIndex %>' />

            <asp:Button ID="btnDelete" runat="server" CommandName="DeleteEntry" Text="Delete"
                        CommandArgument='<%# Container.DataItemIndex %>'
                        OnClientClick="return confirm('Are you sure you want to delete this entry?');" />

            <asp:Button ID="btnClose" runat="server" CommandName="CloseEntry" Text="Close"
                        CommandArgument='<%# Container.DataItemIndex %>'
                        OnClientClick="return confirm('Are you sure you want to close this entry?');" />

            <asp:Button ID="btnDetails" runat="server" CommandName="Details" Text="Details"
                        CommandArgument='<%# Container.DataItemIndex %>' />
        </div>
    </ItemTemplate>
</asp:TemplateField>

                </Columns>
            </asp:GridView>

            
            <br />
            <asp:Literal ID="litDetails" runat="server" />
        </div>
    </form>
</body>
</html>
