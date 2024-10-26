using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP4SCS.Library.Migrations
{
    /// <inheritdoc />
    public partial class InitialDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Gender = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Dob = table.Column<DateOnly>(type: "date", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    IsGoogle = table.Column<bool>(type: "bit", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    FCMToken = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Account__3214EC07D2F2871B", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Material",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Storage = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Material__3214EC07DDCC2B47", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PaymentM__3214EC072F62BE42", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ServiceC__3214EC074F5D6087", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPack",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Period = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Subscrip__3214EC0768CEF925", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TicketCa__3214EC07223ACD26", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ward = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Province = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AccountA__3214EC07D8D8ABB7", x => x.Id);
                    table.ForeignKey(
                        name: "FK__AccountAd__Accou__534D60F1",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BusinessProfile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(3,1)", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    TotalOrder = table.Column<int>(type: "int", nullable: false),
                    PendingAmount = table.Column<int>(type: "int", nullable: false),
                    ProcessingAmount = table.Column<int>(type: "int", nullable: false),
                    FinishedAmount = table.Column<int>(type: "int", nullable: false),
                    CanceledAmount = table.Column<int>(type: "int", nullable: false),
                    RegisteredTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ExpiredTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Business__3214EC07B9CD4797", x => x.Id);
                    table.ForeignKey(
                        name: "FK__BusinessP__Owner__59FA5E80",
                        column: x => x.OwnerId,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cart__3214EC07AAB9F8B2", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Cart__AccountId__75A278F5",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    MethodId = table.Column<int>(type: "int", nullable: false),
                    PackId = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ProcessTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Transact__3214EC07CBA7DAF8", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Transacti__Accou__17F790F9",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Transacti__Metho__18EBB532",
                        column: x => x.MethodId,
                        principalTable: "PaymentMethod",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Transacti__PackI__19DFD96B",
                        column: x => x.PackId,
                        principalTable: "SubscriptionPack",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    DeliveredTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsAutoReject = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DeliveredFee = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ShippingUnit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ShippingCode = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Order__3214EC07D3BA8734", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Order__AccountId__7E37BEF6",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Order__AddressId__7F2BE32F",
                        column: x => x.AddressId,
                        principalTable: "AccountAddress",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BusinessBranch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ward = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Province = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmployeeIds = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    PendingAmount = table.Column<int>(type: "int", nullable: false),
                    ProcessingAmount = table.Column<int>(type: "int", nullable: false),
                    FinishedAmount = table.Column<int>(type: "int", nullable: false),
                    CanceledAmount = table.Column<int>(type: "int", nullable: false),
                    IsDeliverySupport = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Business__3214EC07BAA71D9F", x => x.Id);
                    table.ForeignKey(
                        name: "FK__BusinessB__Busin__5DCAEF64",
                        column: x => x.BusinessId,
                        principalTable: "BusinessProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderNotification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    NotificationTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderNot__3214EC0764990903", x => x.Id);
                    table.ForeignKey(
                        name: "FK__OrderNoti__Order__1CBC4616",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SupportTicket",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SupportT__3214EC072BE152EF", x => x.Id);
                    table.ForeignKey(
                        name: "FK__SupportTi__Accou__245D67DE",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__SupportTi__Categ__25518C17",
                        column: x => x.CategoryId,
                        principalTable: "TicketCategory",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__SupportTi__Order__2645B050",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(3,1)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    OrderedNum = table.Column<int>(type: "int", nullable: false),
                    FeedbackedNum = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Service__3214EC072B056EB1", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Service__BranchI__66603565",
                        column: x => x.BranchId,
                        principalTable: "BusinessBranch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Service__Categor__656C112C",
                        column: x => x.CategoryId,
                        principalTable: "ServiceCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CartItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CartItem__3214EC07791337C2", x => x.Id);
                    table.ForeignKey(
                        name: "FK__CartItem__CartId__787EE5A0",
                        column: x => x.CartId,
                        principalTable: "Cart",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__CartItem__Materi__7A672E12",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__CartItem__Servic__797309D9",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderDet__3214EC07114169AF", x => x.Id);
                    table.ForeignKey(
                        name: "FK__OrderDeta__Mater__04E4BC85",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__OrderDeta__Order__02FC7413",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__OrderDeta__Servi__03F0984C",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Promotion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    SaleOff = table.Column<int>(type: "int", nullable: false),
                    NewPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Promotio__3214EC0745F5DF31", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Promotion__Servi__71D1E811",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServiceMaterial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ServiceM__3214EC077BF357C1", x => x.Id);
                    table.ForeignKey(
                        name: "FK__ServiceMa__Mater__6D0D32F4",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__ServiceMa__Servi__6C190EBB",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderItemId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(3,1)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Feedback__3214EC07E5213B77", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Feedback__OrderI__07C12930",
                        column: x => x.OrderItemId,
                        principalTable: "OrderDetail",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AssetURL",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessId = table.Column<int>(type: "int", nullable: true),
                    FeedbackId = table.Column<int>(type: "int", nullable: true),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: false),
                    IsImage = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AssetURL__3214EC07292B8E5F", x => x.Id);
                    table.ForeignKey(
                        name: "FK__AssetURL__Busine__0C85DE4D",
                        column: x => x.BusinessId,
                        principalTable: "BusinessProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__AssetURL__Feedba__0D7A0286",
                        column: x => x.FeedbackId,
                        principalTable: "Feedback",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__AssetURL__Servic__0E6E26BF",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Account__5C7E359E71751182",
                table: "Account",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Account__A9D105347F159896",
                table: "Account",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountAddress_AccountId",
                table: "AccountAddress",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetURL_BusinessId",
                table: "AssetURL",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetURL_FeedbackId",
                table: "AssetURL",
                column: "FeedbackId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetURL_ServiceId",
                table: "AssetURL",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessBranch_BusinessId",
                table: "BusinessBranch",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "UQ__Business__5C7E359E40D15ED5",
                table: "BusinessProfile",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Business__737584F65C16A395",
                table: "BusinessProfile",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Business__819385B9D1697A93",
                table: "BusinessProfile",
                column: "OwnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Cart__349DA5A7E55EE33F",
                table: "Cart",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CartId",
                table: "CartItem",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_MaterialId",
                table: "CartItem",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_ServiceId",
                table: "CartItem",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_OrderItemId",
                table: "Feedback",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_AccountId",
                table: "Order",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_AddressId",
                table: "Order",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_MaterialId",
                table: "OrderDetail",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderId",
                table: "OrderDetail",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ServiceId",
                table: "OrderDetail",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderNotification_OrderId",
                table: "OrderNotification",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "UQ__PaymentM__737584F6FA15D1A7",
                table: "PaymentMethod",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Promotio__C51BB00B8A38A700",
                table: "Promotion",
                column: "ServiceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Service_BranchId",
                table: "Service",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_CategoryId",
                table: "Service",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "UQ__ServiceC__737584F60D748FEF",
                table: "ServiceCategory",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceMaterial_MaterialId",
                table: "ServiceMaterial",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceMaterial_ServiceId",
                table: "ServiceMaterial",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "UQ__Subscrip__737584F6DC09C378",
                table: "SubscriptionPack",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicket_AccountId",
                table: "SupportTicket",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicket_CategoryId",
                table: "SupportTicket",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicket_OrderId",
                table: "SupportTicket",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "UQ__TicketCa__737584F61AF30862",
                table: "TicketCategory",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_AccountId",
                table: "Transaction",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_MethodId",
                table: "Transaction",
                column: "MethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_PackId",
                table: "Transaction",
                column: "PackId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetURL");

            migrationBuilder.DropTable(
                name: "CartItem");

            migrationBuilder.DropTable(
                name: "OrderNotification");

            migrationBuilder.DropTable(
                name: "Promotion");

            migrationBuilder.DropTable(
                name: "ServiceMaterial");

            migrationBuilder.DropTable(
                name: "SupportTicket");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "TicketCategory");

            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropTable(
                name: "SubscriptionPack");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "AccountAddress");

            migrationBuilder.DropTable(
                name: "BusinessBranch");

            migrationBuilder.DropTable(
                name: "ServiceCategory");

            migrationBuilder.DropTable(
                name: "BusinessProfile");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
