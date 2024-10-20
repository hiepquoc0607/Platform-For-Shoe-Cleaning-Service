using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP4SCS.Library.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationshipDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__AccountAd__Accou__7F2BE32F",
                table: "AccountAddress");

            migrationBuilder.DropForeignKey(
                name: "FK__AssetURL__Busine__797309D9",
                table: "AssetURL");

            migrationBuilder.DropForeignKey(
                name: "FK__AssetURL__Feedba__7A672E12",
                table: "AssetURL");

            migrationBuilder.DropForeignKey(
                name: "FK__AssetURL__Servic__7B5B524B",
                table: "AssetURL");

            migrationBuilder.DropForeignKey(
                name: "FK__BusinessB__Owner__52593CB8",
                table: "BusinessBranch");

            migrationBuilder.DropForeignKey(
                name: "FK__BusinessP__Owner__6A30C649",
                table: "BusinessProfile");

            migrationBuilder.DropForeignKey(
                name: "FK__Cart__AccountId__60A75C0F",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK__CartItem__CartId__6383C8BA",
                table: "CartItem");

            migrationBuilder.DropForeignKey(
                name: "FK__CartItem__Servic__6477ECF3",
                table: "CartItem");

            migrationBuilder.DropForeignKey(
                name: "FK__Feedback__OrderI__74AE54BC",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK__Order__AccountId__6E01572D",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK__OrderDeta__Order__70DDC3D8",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK__OrderDeta__Servi__71D1E811",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK__OrderNoti__Order__0B91BA14",
                table: "OrderNotification");

            migrationBuilder.DropForeignKey(
                name: "FK__Promotion__Servi__5DCAEF64",
                table: "Promotion");

            migrationBuilder.DropForeignKey(
                name: "FK__Service__BranchI__59FA5E80",
                table: "Service");

            migrationBuilder.DropForeignKey(
                name: "FK__Service__Categor__59063A47",
                table: "Service");

            migrationBuilder.DropForeignKey(
                name: "FK__SupportTi__Accou__123EB7A3",
                table: "SupportTicket");

            migrationBuilder.DropForeignKey(
                name: "FK__SupportTi__Categ__1332DBDC",
                table: "SupportTicket");

            migrationBuilder.DropForeignKey(
                name: "FK__SupportTi__Order__14270015",
                table: "SupportTicket");

            migrationBuilder.DropForeignKey(
                name: "FK__Transacti__Accou__06CD04F7",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK__Transacti__Metho__07C12930",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK__Transacti__PackI__08B54D69",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Transact__3214EC07ECA0DBCB",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK__TicketCa__3214EC070CF6B22A",
                table: "TicketCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK__SupportT__3214EC0701A742CA",
                table: "SupportTicket");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Subscrip__3214EC079C84DCBB",
                table: "SubscriptionPack");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Statisti__3214EC078730AED5",
                table: "Statistic");

            migrationBuilder.DropPrimaryKey(
                name: "PK__ServiceC__3214EC0737126168",
                table: "ServiceCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Service__3214EC07850D7AB6",
                table: "Service");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Promotio__3214EC07CE34FD20",
                table: "Promotion");

            migrationBuilder.DropIndex(
                name: "IX_Promotion_ServiceId",
                table: "Promotion");

            migrationBuilder.DropPrimaryKey(
                name: "PK__PaymentM__3214EC07252E1227",
                table: "PaymentMethod");

            migrationBuilder.DropPrimaryKey(
                name: "PK__OrderNot__3214EC073452DA30",
                table: "OrderNotification");

            migrationBuilder.DropPrimaryKey(
                name: "PK__OrderDet__3214EC07EC0ECB4A",
                table: "OrderDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Order__3214EC07B2BCABC4",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Feedback__3214EC07DF01AEE1",
                table: "Feedback");

            migrationBuilder.DropIndex(
                name: "IX_Feedback_OrderItemId",
                table: "Feedback");

            migrationBuilder.DropPrimaryKey(
                name: "PK__CartItem__3214EC0718AEA0EA",
                table: "CartItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Cart__3214EC072A696188",
                table: "Cart");

            migrationBuilder.DropIndex(
                name: "IX_Cart_AccountId",
                table: "Cart");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Business__3214EC0718F928C4",
                table: "BusinessProfile");

            migrationBuilder.DropIndex(
                name: "IX_BusinessProfile_OwnerId",
                table: "BusinessProfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Business__3214EC07F039126D",
                table: "BusinessBranch");

            migrationBuilder.DropPrimaryKey(
                name: "PK__AssetURL__3214EC075A946315",
                table: "AssetURL");

            migrationBuilder.DropPrimaryKey(
                name: "PK__AccountA__3214EC0708B71FF0",
                table: "AccountAddress");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Account__3214EC07F457E10C",
                table: "Account");

            migrationBuilder.RenameIndex(
                name: "UQ__Business__737584F6AF9BE15B",
                table: "BusinessProfile",
                newName: "UQ__Business__737584F6EB61F34F");

            migrationBuilder.RenameIndex(
                name: "UQ__Business__5C7E359E1B70DECC",
                table: "BusinessProfile",
                newName: "UQ__Business__5C7E359E8629A6F2");

            migrationBuilder.RenameIndex(
                name: "UQ__Account__A9D105349B6F33FE",
                table: "Account",
                newName: "UQ__Account__A9D10534C2ABA714");

            migrationBuilder.RenameIndex(
                name: "UQ__Account__5C7E359E2FC9193A",
                table: "Account",
                newName: "UQ__Account__5C7E359ED842675B");

            migrationBuilder.AlterColumn<bool>(
                name: "IsGoogle",
                table: "Account",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Account",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__Transact__3214EC071EC380A8",
                table: "Transaction",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__TicketCa__3214EC070A5FB32E",
                table: "TicketCategory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__SupportT__3214EC07C0B11E58",
                table: "SupportTicket",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Subscrip__3214EC07B2D2E687",
                table: "SubscriptionPack",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Statisti__3214EC0792FDD381",
                table: "Statistic",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__ServiceC__3214EC076B7972C6",
                table: "ServiceCategory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Service__3214EC07119BD25D",
                table: "Service",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Promotio__3214EC07BAE139D1",
                table: "Promotion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__PaymentM__3214EC07EA1ED0C8",
                table: "PaymentMethod",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__OrderNot__3214EC070CBAD700",
                table: "OrderNotification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__OrderDet__3214EC07BDBD4FA7",
                table: "OrderDetail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Order__3214EC075F245998",
                table: "Order",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Feedback__3214EC07BC903870",
                table: "Feedback",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__CartItem__3214EC077E65CDA8",
                table: "CartItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Cart__3214EC0771129BEF",
                table: "Cart",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Business__3214EC07122771FD",
                table: "BusinessProfile",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Business__3214EC07D32975B2",
                table: "BusinessBranch",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__AssetURL__3214EC071756BFA6",
                table: "AssetURL",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__AccountA__3214EC07AB9BDB18",
                table: "AccountAddress",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Account__3214EC07CB385EDE",
                table: "Account",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "UQ__TicketCa__737584F689F12AA9",
                table: "TicketCategory",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Subscrip__737584F64C87D2C0",
                table: "SubscriptionPack",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__ServiceC__737584F6611D0668",
                table: "ServiceCategory",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Promotio__C51BB00BC1F6ACC6",
                table: "Promotion",
                column: "ServiceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__PaymentM__737584F6ACED4F89",
                table: "PaymentMethod",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Feedback__57ED06800530F4D0",
                table: "Feedback",
                column: "OrderItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Cart__349DA5A794B140BA",
                table: "Cart",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Business__819385B9EFEB993D",
                table: "BusinessProfile",
                column: "OwnerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK__AccountAd__Accou__05D8E0BE",
                table: "AccountAddress",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__AssetURL__Busine__00200768",
                table: "AssetURL",
                column: "BusinessProfileId",
                principalTable: "BusinessProfile",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__AssetURL__Feedba__01142BA1",
                table: "AssetURL",
                column: "FeedbackId",
                principalTable: "Feedback",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__AssetURL__Servic__02084FDA",
                table: "AssetURL",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__BusinessB__Owner__5441852A",
                table: "BusinessBranch",
                column: "OwnerId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__BusinessP__Owner__6FE99F9F",
                table: "BusinessProfile",
                column: "OwnerId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Cart__AccountId__656C112C",
                table: "Cart",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__CartItem__CartId__68487DD7",
                table: "CartItem",
                column: "CartId",
                principalTable: "Cart",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__CartItem__Servic__693CA210",
                table: "CartItem",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Feedback__OrderI__7B5B524B",
                table: "Feedback",
                column: "OrderItemId",
                principalTable: "OrderDetail",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Order__AccountId__73BA3083",
                table: "Order",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__OrderDeta__Order__76969D2E",
                table: "OrderDetail",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__OrderDeta__Servi__778AC167",
                table: "OrderDetail",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__OrderNoti__Order__14270015",
                table: "OrderNotification",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Promotion__Servi__619B8048",
                table: "Promotion",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Service__BranchI__5CD6CB2B",
                table: "Service",
                column: "BranchId",
                principalTable: "BusinessBranch",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Service__Categor__5BE2A6F2",
                table: "Service",
                column: "CategoryId",
                principalTable: "ServiceCategory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__SupportTi__Accou__1BC821DD",
                table: "SupportTicket",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__SupportTi__Categ__1CBC4616",
                table: "SupportTicket",
                column: "CategoryId",
                principalTable: "TicketCategory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__SupportTi__Order__1DB06A4F",
                table: "SupportTicket",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Transacti__Accou__0F624AF8",
                table: "Transaction",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Transacti__Metho__10566F31",
                table: "Transaction",
                column: "MethodId",
                principalTable: "PaymentMethod",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Transacti__PackI__114A936A",
                table: "Transaction",
                column: "PackId",
                principalTable: "SubscriptionPack",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__AccountAd__Accou__05D8E0BE",
                table: "AccountAddress");

            migrationBuilder.DropForeignKey(
                name: "FK__AssetURL__Busine__00200768",
                table: "AssetURL");

            migrationBuilder.DropForeignKey(
                name: "FK__AssetURL__Feedba__01142BA1",
                table: "AssetURL");

            migrationBuilder.DropForeignKey(
                name: "FK__AssetURL__Servic__02084FDA",
                table: "AssetURL");

            migrationBuilder.DropForeignKey(
                name: "FK__BusinessB__Owner__5441852A",
                table: "BusinessBranch");

            migrationBuilder.DropForeignKey(
                name: "FK__BusinessP__Owner__6FE99F9F",
                table: "BusinessProfile");

            migrationBuilder.DropForeignKey(
                name: "FK__Cart__AccountId__656C112C",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK__CartItem__CartId__68487DD7",
                table: "CartItem");

            migrationBuilder.DropForeignKey(
                name: "FK__CartItem__Servic__693CA210",
                table: "CartItem");

            migrationBuilder.DropForeignKey(
                name: "FK__Feedback__OrderI__7B5B524B",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK__Order__AccountId__73BA3083",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK__OrderDeta__Order__76969D2E",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK__OrderDeta__Servi__778AC167",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK__OrderNoti__Order__14270015",
                table: "OrderNotification");

            migrationBuilder.DropForeignKey(
                name: "FK__Promotion__Servi__619B8048",
                table: "Promotion");

            migrationBuilder.DropForeignKey(
                name: "FK__Service__BranchI__5CD6CB2B",
                table: "Service");

            migrationBuilder.DropForeignKey(
                name: "FK__Service__Categor__5BE2A6F2",
                table: "Service");

            migrationBuilder.DropForeignKey(
                name: "FK__SupportTi__Accou__1BC821DD",
                table: "SupportTicket");

            migrationBuilder.DropForeignKey(
                name: "FK__SupportTi__Categ__1CBC4616",
                table: "SupportTicket");

            migrationBuilder.DropForeignKey(
                name: "FK__SupportTi__Order__1DB06A4F",
                table: "SupportTicket");

            migrationBuilder.DropForeignKey(
                name: "FK__Transacti__Accou__0F624AF8",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK__Transacti__Metho__10566F31",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK__Transacti__PackI__114A936A",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Transact__3214EC071EC380A8",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK__TicketCa__3214EC070A5FB32E",
                table: "TicketCategory");

            migrationBuilder.DropIndex(
                name: "UQ__TicketCa__737584F689F12AA9",
                table: "TicketCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK__SupportT__3214EC07C0B11E58",
                table: "SupportTicket");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Subscrip__3214EC07B2D2E687",
                table: "SubscriptionPack");

            migrationBuilder.DropIndex(
                name: "UQ__Subscrip__737584F64C87D2C0",
                table: "SubscriptionPack");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Statisti__3214EC0792FDD381",
                table: "Statistic");

            migrationBuilder.DropPrimaryKey(
                name: "PK__ServiceC__3214EC076B7972C6",
                table: "ServiceCategory");

            migrationBuilder.DropIndex(
                name: "UQ__ServiceC__737584F6611D0668",
                table: "ServiceCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Service__3214EC07119BD25D",
                table: "Service");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Promotio__3214EC07BAE139D1",
                table: "Promotion");

            migrationBuilder.DropIndex(
                name: "UQ__Promotio__C51BB00BC1F6ACC6",
                table: "Promotion");

            migrationBuilder.DropPrimaryKey(
                name: "PK__PaymentM__3214EC07EA1ED0C8",
                table: "PaymentMethod");

            migrationBuilder.DropIndex(
                name: "UQ__PaymentM__737584F6ACED4F89",
                table: "PaymentMethod");

            migrationBuilder.DropPrimaryKey(
                name: "PK__OrderNot__3214EC070CBAD700",
                table: "OrderNotification");

            migrationBuilder.DropPrimaryKey(
                name: "PK__OrderDet__3214EC07BDBD4FA7",
                table: "OrderDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Order__3214EC075F245998",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Feedback__3214EC07BC903870",
                table: "Feedback");

            migrationBuilder.DropIndex(
                name: "UQ__Feedback__57ED06800530F4D0",
                table: "Feedback");

            migrationBuilder.DropPrimaryKey(
                name: "PK__CartItem__3214EC077E65CDA8",
                table: "CartItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Cart__3214EC0771129BEF",
                table: "Cart");

            migrationBuilder.DropIndex(
                name: "UQ__Cart__349DA5A794B140BA",
                table: "Cart");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Business__3214EC07122771FD",
                table: "BusinessProfile");

            migrationBuilder.DropIndex(
                name: "UQ__Business__819385B9EFEB993D",
                table: "BusinessProfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Business__3214EC07D32975B2",
                table: "BusinessBranch");

            migrationBuilder.DropPrimaryKey(
                name: "PK__AssetURL__3214EC071756BFA6",
                table: "AssetURL");

            migrationBuilder.DropPrimaryKey(
                name: "PK__AccountA__3214EC07AB9BDB18",
                table: "AccountAddress");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Account__3214EC07CB385EDE",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Account");

            migrationBuilder.RenameIndex(
                name: "UQ__Business__737584F6EB61F34F",
                table: "BusinessProfile",
                newName: "UQ__Business__737584F6AF9BE15B");

            migrationBuilder.RenameIndex(
                name: "UQ__Business__5C7E359E8629A6F2",
                table: "BusinessProfile",
                newName: "UQ__Business__5C7E359E1B70DECC");

            migrationBuilder.RenameIndex(
                name: "UQ__Account__A9D10534C2ABA714",
                table: "Account",
                newName: "UQ__Account__A9D105349B6F33FE");

            migrationBuilder.RenameIndex(
                name: "UQ__Account__5C7E359ED842675B",
                table: "Account",
                newName: "UQ__Account__5C7E359E2FC9193A");

            migrationBuilder.AlterColumn<bool>(
                name: "IsGoogle",
                table: "Account",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Transact__3214EC07ECA0DBCB",
                table: "Transaction",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__TicketCa__3214EC070CF6B22A",
                table: "TicketCategory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__SupportT__3214EC0701A742CA",
                table: "SupportTicket",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Subscrip__3214EC079C84DCBB",
                table: "SubscriptionPack",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Statisti__3214EC078730AED5",
                table: "Statistic",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__ServiceC__3214EC0737126168",
                table: "ServiceCategory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Service__3214EC07850D7AB6",
                table: "Service",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Promotio__3214EC07CE34FD20",
                table: "Promotion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__PaymentM__3214EC07252E1227",
                table: "PaymentMethod",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__OrderNot__3214EC073452DA30",
                table: "OrderNotification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__OrderDet__3214EC07EC0ECB4A",
                table: "OrderDetail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Order__3214EC07B2BCABC4",
                table: "Order",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Feedback__3214EC07DF01AEE1",
                table: "Feedback",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__CartItem__3214EC0718AEA0EA",
                table: "CartItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Cart__3214EC072A696188",
                table: "Cart",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Business__3214EC0718F928C4",
                table: "BusinessProfile",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Business__3214EC07F039126D",
                table: "BusinessBranch",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__AssetURL__3214EC075A946315",
                table: "AssetURL",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__AccountA__3214EC0708B71FF0",
                table: "AccountAddress",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Account__3214EC07F457E10C",
                table: "Account",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_ServiceId",
                table: "Promotion",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_OrderItemId",
                table: "Feedback",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_AccountId",
                table: "Cart",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessProfile_OwnerId",
                table: "BusinessProfile",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK__AccountAd__Accou__7F2BE32F",
                table: "AccountAddress",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__AssetURL__Busine__797309D9",
                table: "AssetURL",
                column: "BusinessProfileId",
                principalTable: "BusinessProfile",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__AssetURL__Feedba__7A672E12",
                table: "AssetURL",
                column: "FeedbackId",
                principalTable: "Feedback",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__AssetURL__Servic__7B5B524B",
                table: "AssetURL",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__BusinessB__Owner__52593CB8",
                table: "BusinessBranch",
                column: "OwnerId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__BusinessP__Owner__6A30C649",
                table: "BusinessProfile",
                column: "OwnerId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Cart__AccountId__60A75C0F",
                table: "Cart",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__CartItem__CartId__6383C8BA",
                table: "CartItem",
                column: "CartId",
                principalTable: "Cart",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__CartItem__Servic__6477ECF3",
                table: "CartItem",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Feedback__OrderI__74AE54BC",
                table: "Feedback",
                column: "OrderItemId",
                principalTable: "OrderDetail",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Order__AccountId__6E01572D",
                table: "Order",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__OrderDeta__Order__70DDC3D8",
                table: "OrderDetail",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__OrderDeta__Servi__71D1E811",
                table: "OrderDetail",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__OrderNoti__Order__0B91BA14",
                table: "OrderNotification",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Promotion__Servi__5DCAEF64",
                table: "Promotion",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Service__BranchI__59FA5E80",
                table: "Service",
                column: "BranchId",
                principalTable: "BusinessBranch",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Service__Categor__59063A47",
                table: "Service",
                column: "CategoryId",
                principalTable: "ServiceCategory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__SupportTi__Accou__123EB7A3",
                table: "SupportTicket",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__SupportTi__Categ__1332DBDC",
                table: "SupportTicket",
                column: "CategoryId",
                principalTable: "TicketCategory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__SupportTi__Order__14270015",
                table: "SupportTicket",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Transacti__Accou__06CD04F7",
                table: "Transaction",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Transacti__Metho__07C12930",
                table: "Transaction",
                column: "MethodId",
                principalTable: "PaymentMethod",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Transacti__PackI__08B54D69",
                table: "Transaction",
                column: "PackId",
                principalTable: "SubscriptionPack",
                principalColumn: "Id");
        }
    }
}
