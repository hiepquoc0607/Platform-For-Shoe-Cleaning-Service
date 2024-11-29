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
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Gender = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Dob = table.Column<DateOnly>(type: "date", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    IsGoogle = table.Column<bool>(type: "bit", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshExpireTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    OTP = table.Column<int>(type: "int", nullable: true),
                    OTPExpiredTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    FCMToken = table.Column<string>(type: "text", nullable: true),
                    CreatedByOwnerId = table.Column<int>(type: "int", nullable: true),
                    Role = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Status = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Account__3214EC0770E0D6DA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leaderboard",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessIds = table.Column<string>(type: "text", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    IsMonth = table.Column<bool>(type: "bit", nullable: false),
                    IsYear = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Leaderbo__3214EC079DC65B14", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlatformStatistic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<decimal>(type: "decimal(13,1)", nullable: false),
                    Date = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Platform__3214EC071613A28C", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ServiceC__3214EC07F387B084", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPack",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Period = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Subscrip__3214EC078980A5C2", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TicketCa__3214EC073A6A13C5", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    WardCode = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    Ward = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false),
                    District = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AccountA__3214EC07FB73C6B6", x => x.Id);
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
                    table.PrimaryKey("PK__Business__3214EC077603EEF7", x => x.Id);
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
                    table.PrimaryKey("PK__Cart__3214EC0795405A6B", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Cart__AccountId__6D0D32F4",
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
                    PackName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ProcessTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentMethod = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Status = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Transact__3214EC079B517EC8", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Transacti__Accou__1BC821DD",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(3,1)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    OrderedNum = table.Column<int>(type: "int", nullable: false),
                    FeedbackedNum = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Service__3214EC07BC32246A", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Service__Categor__534D60F1",
                        column: x => x.CategoryId,
                        principalTable: "ServiceCategory",
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
                    CanceledTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    PendingTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    ApprovedTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    RevievedTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    ProcessingTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    StoragedTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    ShippingTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeliveredTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    FinishedTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    AbandonedTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsAutoReject = table.Column<bool>(type: "bit", nullable: false),
                    OrderPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DeliveredFee = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ShippingUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShippingCode = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Order__3214EC0745BC6AC9", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Order__AccountId__75A278F5",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Order__AddressId__76969D2E",
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
                    WardCode = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    Ward = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false),
                    District = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                    table.PrimaryKey("PK__Business__3214EC07AC25614A", x => x.Id);
                    table.ForeignKey(
                        name: "FK__BusinessB__Busin__4BAC3F29",
                        column: x => x.BusinessId,
                        principalTable: "BusinessProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BusinessStatistic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Business__3214EC0771C002E4", x => x.Id);
                    table.ForeignKey(
                        name: "FK__BusinessS__Busin__22751F6C",
                        column: x => x.BusinessId,
                        principalTable: "BusinessProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Material",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Material__3214EC07EAB229C0", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Material__Servic__5AEE82B9",
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
                    Status = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Promotio__3214EC07D713764C", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Promotion__Servi__693CA210",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServiceProcess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    Process = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProcessOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ServiceP__3214EC07140644E3", x => x.Id);
                    table.ForeignKey(
                        name: "FK__ServicePr__Servi__571DF1D5",
                        column: x => x.ServiceId,
                        principalTable: "Service",
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
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsProviderNoti = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderNot__3214EC07CBA2C999", x => x.Id);
                    table.ForeignKey(
                        name: "FK__OrderNoti__Order__1EA48E88",
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
                    ModeratorId = table.Column<int>(type: "int", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    ParentTicketId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsParentTicket = table.Column<bool>(type: "bit", nullable: false),
                    AutoClosedTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SupportT__3214EC0705D06273", x => x.Id);
                    table.ForeignKey(
                        name: "FK__SupportTi__Categ__0A9D95DB",
                        column: x => x.CategoryId,
                        principalTable: "TicketCategory",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__SupportTi__Moder__09A971A2",
                        column: x => x.ModeratorId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__SupportTi__Order__0B91BA14",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__SupportTi__UserI__08B54D69",
                        column: x => x.UserId,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BranchService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BranchSe__3214EC07716B5253", x => x.Id);
                    table.ForeignKey(
                        name: "FK__BranchSer__Branc__5FB337D6",
                        column: x => x.BranchId,
                        principalTable: "BusinessBranch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__BranchSer__Servi__5EBF139D",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CartItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    MaterialIds = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CartItem__3214EC07D688C4C6", x => x.Id);
                    table.ForeignKey(
                        name: "FK__CartItem__Branch__70DDC3D8",
                        column: x => x.BranchId,
                        principalTable: "BusinessBranch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__CartItem__CartId__6FE99F9F",
                        column: x => x.CartId,
                        principalTable: "Cart",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__CartItem__Servic__71D1E811",
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
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    MaterialIds = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessState = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderDet__3214EC07BCEF2DFE", x => x.Id);
                    table.ForeignKey(
                        name: "FK__OrderDeta__Branc__7A672E12",
                        column: x => x.BranchId,
                        principalTable: "BusinessBranch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__OrderDeta__Order__797309D9",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__OrderDeta__Servi__7B5B524B",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BranchMaterial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    Storage = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BranchMa__3214EC07B97E8A3F", x => x.Id);
                    table.ForeignKey(
                        name: "FK__BranchMat__Branc__6477ECF3",
                        column: x => x.BranchId,
                        principalTable: "BusinessBranch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__BranchMat__Mater__6383C8BA",
                        column: x => x.MaterialId,
                        principalTable: "Material",
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
                    Reply = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsValidContent = table.Column<bool>(type: "bit", nullable: false),
                    IsValidAsset = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Feedback__3214EC07BB4CF31F", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Feedback__OrderI__00200768",
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
                    MaterialId = table.Column<int>(type: "int", nullable: true),
                    TicketId = table.Column<int>(type: "int", nullable: true),
                    OrderDetailId = table.Column<int>(type: "int", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AssetURL__3214EC07FA85CCE7", x => x.Id);
                    table.ForeignKey(
                        name: "FK__AssetURL__Busine__0F624AF8",
                        column: x => x.BusinessId,
                        principalTable: "BusinessProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__AssetURL__Feedba__10566F31",
                        column: x => x.FeedbackId,
                        principalTable: "Feedback",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__AssetURL__Materi__123EB7A3",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__AssetURL__OrderD__14270015",
                        column: x => x.OrderDetailId,
                        principalTable: "OrderDetail",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__AssetURL__Servic__114A936A",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__AssetURL__Ticket__1332DBDC",
                        column: x => x.TicketId,
                        principalTable: "SupportTicket",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Account__5C7E359E23AECA46",
                table: "Account",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Account__A9D1053477E1BF0B",
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
                name: "IX_AssetURL_MaterialId",
                table: "AssetURL",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetURL_OrderDetailId",
                table: "AssetURL",
                column: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetURL_ServiceId",
                table: "AssetURL",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetURL_TicketId",
                table: "AssetURL",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchMaterial_BranchId",
                table: "BranchMaterial",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchMaterial_MaterialId",
                table: "BranchMaterial",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchService_BranchId",
                table: "BranchService",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchService_ServiceId",
                table: "BranchService",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessBranch_BusinessId",
                table: "BusinessBranch",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "UQ__Business__5C7E359EB674E9E9",
                table: "BusinessProfile",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Business__737584F6C104F18F",
                table: "BusinessProfile",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Business__819385B9643CBCBA",
                table: "BusinessProfile",
                column: "OwnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessStatistic_BusinessId",
                table: "BusinessStatistic",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "UQ__Cart__349DA5A72E58B93C",
                table: "Cart",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_BranchId",
                table: "CartItem",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CartId",
                table: "CartItem",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_ServiceId",
                table: "CartItem",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "UQ__Feedback__57ED068011673F48",
                table: "Feedback",
                column: "OrderItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Material_ServiceId",
                table: "Material",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_AccountId",
                table: "Order",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_AddressId",
                table: "Order",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_BranchId",
                table: "OrderDetail",
                column: "BranchId");

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
                name: "UQ__Promotio__C51BB00BFA7DC882",
                table: "Promotion",
                column: "ServiceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Service_CategoryId",
                table: "Service",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "UQ__ServiceC__737584F68346D265",
                table: "ServiceCategory",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceProcess_ServiceId",
                table: "ServiceProcess",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "UQ__Subscrip__737584F63173AB58",
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
                name: "UQ__TicketCa__737584F6D9A92F26",
                table: "TicketCategory",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_AccountId",
                table: "Transaction",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetURL");

            migrationBuilder.DropTable(
                name: "BranchMaterial");

            migrationBuilder.DropTable(
                name: "BranchService");

            migrationBuilder.DropTable(
                name: "BusinessStatistic");

            migrationBuilder.DropTable(
                name: "CartItem");

            migrationBuilder.DropTable(
                name: "Leaderboard");

            migrationBuilder.DropTable(
                name: "OrderNotification");

            migrationBuilder.DropTable(
                name: "PlatformStatistic");

            migrationBuilder.DropTable(
                name: "Promotion");

            migrationBuilder.DropTable(
                name: "ServiceProcess");

            migrationBuilder.DropTable(
                name: "SubscriptionPack");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "SupportTicket");

            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "TicketCategory");

            migrationBuilder.DropTable(
                name: "BusinessBranch");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "BusinessProfile");

            migrationBuilder.DropTable(
                name: "AccountAddress");

            migrationBuilder.DropTable(
                name: "ServiceCategory");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
