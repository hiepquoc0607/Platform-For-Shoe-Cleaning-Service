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
                    table.PrimaryKey("PK__Account__3214EC07357E4740", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ward = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Province = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Location__3214EC075240B00D", x => x.Id);
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
                    table.PrimaryKey("PK__Material__3214EC0713DD21F1", x => x.Id);
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
                    table.PrimaryKey("PK__PaymentM__3214EC07F31E0723", x => x.Id);
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
                    table.PrimaryKey("PK__ServiceC__3214EC07C1A80AAB", x => x.Id);
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
                    table.PrimaryKey("PK__Subscrip__3214EC076827D139", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TicketCa__3214EC077E8EA34A", x => x.Id);
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
                    table.PrimaryKey("PK__AccountA__3214EC07E1569FF3", x => x.Id);
                    table.ForeignKey(
                        name: "FK__AccountAd__Accou__412EB0B6",
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
                    ToTalServiceNum = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    RegisteredTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ExpiredTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Business__3214EC07EB9B4AEA", x => x.Id);
                    table.ForeignKey(
                        name: "FK__BusinessP__Owner__47DBAE45",
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
                    table.PrimaryKey("PK__Cart__3214EC0776E7108F", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Cart__AccountId__6383C8BA",
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
                    table.PrimaryKey("PK__Transact__3214EC0767F85638", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Transacti__Accou__06CD04F7",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Transacti__Metho__07C12930",
                        column: x => x.MethodId,
                        principalTable: "PaymentMethod",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Transacti__PackI__08B54D69",
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
                    table.PrimaryKey("PK__Order__3214EC07FCAD7DB9", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Order__AccountId__6C190EBB",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Order__AddressId__6D0D32F4",
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
                    table.PrimaryKey("PK__Business__3214EC07F208C6EB", x => x.Id);
                    table.ForeignKey(
                        name: "FK__BusinessB__Busin__4BAC3F29",
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
                    table.PrimaryKey("PK__OrderNot__3214EC0747CAFDB7", x => x.Id);
                    table.ForeignKey(
                        name: "FK__OrderNoti__Order__0B91BA14",
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
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ModeratorId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SupportT__3214EC07E486F11C", x => x.Id);
                    table.ForeignKey(
                        name: "FK__SupportTi__Categ__160F4887",
                        column: x => x.CategoryId,
                        principalTable: "TicketCategory",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__SupportTi__Moder__151B244E",
                        column: x => x.ModeratorId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__SupportTi__Order__17036CC0",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__SupportTi__UserI__14270015",
                        column: x => x.UserId,
                        principalTable: "Account",
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
                    table.PrimaryKey("PK__Service__3214EC07AE51A6D7", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Service__BranchI__5441852A",
                        column: x => x.BranchId,
                        principalTable: "BusinessBranch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Service__Categor__534D60F1",
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
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    MaterialId = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CartItem__3214EC079DBD0F73", x => x.Id);
                    table.ForeignKey(
                        name: "FK__CartItem__CartId__66603565",
                        column: x => x.CartId,
                        principalTable: "Cart",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__CartItem__Materi__68487DD7",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__CartItem__Servic__6754599E",
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
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    MaterialId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderDet__3214EC075E5C27DF", x => x.Id);
                    table.ForeignKey(
                        name: "FK__OrderDeta__Mater__72C60C4A",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__OrderDeta__Order__70DDC3D8",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__OrderDeta__Servi__71D1E811",
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
                    table.PrimaryKey("PK__Promotio__3214EC07FCD1E9BE", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Promotion__Servi__5FB337D6",
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
                    table.PrimaryKey("PK__ServiceM__3214EC077FF79FD5", x => x.Id);
                    table.ForeignKey(
                        name: "FK__ServiceMa__Mater__5AEE82B9",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__ServiceMa__Servi__59FA5E80",
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
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsValidContent = table.Column<bool>(type: "bit", nullable: false),
                    IsValidAsset = table.Column<bool>(type: "bit", nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Feedback__3214EC078DF2FA1C", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Feedback__OrderI__76969D2E",
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
                    table.PrimaryKey("PK__AssetURL__3214EC0741B1990A", x => x.Id);
                    table.ForeignKey(
                        name: "FK__AssetURL__Busine__7B5B524B",
                        column: x => x.BusinessId,
                        principalTable: "BusinessProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__AssetURL__Feedba__7C4F7684",
                        column: x => x.FeedbackId,
                        principalTable: "Feedback",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__AssetURL__Servic__7D439ABD",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Account__5C7E359EDC65A5CD",
                table: "Account",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Account__A9D1053420FAB0E5",
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
                name: "UQ__Business__5C7E359E5949A9FF",
                table: "BusinessProfile",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Business__737584F6354FFDD7",
                table: "BusinessProfile",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Business__819385B998B9EAE4",
                table: "BusinessProfile",
                column: "OwnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Cart__349DA5A7A2092673",
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
                name: "UQ__PaymentM__737584F67EB39B3D",
                table: "PaymentMethod",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Promotio__C51BB00B36DE195E",
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
                name: "UQ__ServiceC__737584F69019FF67",
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
                name: "UQ__Subscrip__737584F688E082C5",
                table: "SubscriptionPack",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicket_CategoryId",
                table: "SupportTicket",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicket_ModeratorId",
                table: "SupportTicket",
                column: "ModeratorId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicket_OrderId",
                table: "SupportTicket",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicket_UserId",
                table: "SupportTicket",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ__TicketCa__737584F68402F5D6",
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
                name: "Location");

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
