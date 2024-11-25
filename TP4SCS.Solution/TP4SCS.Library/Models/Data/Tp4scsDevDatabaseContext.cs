using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TP4SCS.Library.Models.Data;

public partial class Tp4scsDevDatabaseContext : DbContext
{
    public Tp4scsDevDatabaseContext()
    {
    }

    public Tp4scsDevDatabaseContext(DbContextOptions<Tp4scsDevDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountAddress> AccountAddresses { get; set; }

    public virtual DbSet<AssetUrl> AssetUrls { get; set; }

    public virtual DbSet<BranchMaterial> BranchMaterials { get; set; }

    public virtual DbSet<BranchService> BranchServices { get; set; }

    public virtual DbSet<BusinessBranch> BusinessBranches { get; set; }

    public virtual DbSet<BusinessProfile> BusinessProfiles { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderNotification> OrderNotifications { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceCategory> ServiceCategories { get; set; }

    public virtual DbSet<ServiceProcess> ServiceProcesses { get; set; }

    public virtual DbSet<SubscriptionPack> SubscriptionPacks { get; set; }

    public virtual DbSet<SupportTicket> SupportTickets { get; set; }

    public virtual DbSet<TicketCategory> TicketCategories { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        IConfiguration configuration = builder.Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC0733362AA6");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Phone, "UQ__Account__5C7E359E837C3EF7").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Account__A9D105349719CF6F").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Fcmtoken)
                .HasColumnType("text")
                .HasColumnName("FCMToken");
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl).HasColumnType("text");
            entity.Property(e => e.IsVerified).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash).HasColumnType("text");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.RefreshExpireTime).HasColumnType("datetime");
            entity.Property(e => e.RefreshToken).HasColumnType("text");
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AccountAddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AccountA__3214EC07F8150685");

            entity.ToTable("AccountAddress");

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.District).HasMaxLength(50);
            entity.Property(e => e.Province).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Ward).HasMaxLength(50);
            entity.Property(e => e.WardCode)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.AccountAddresses)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AccountAd__Accou__412EB0B6");
        });

        modelBuilder.Entity<AssetUrl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AssetURL__3214EC074CDDBA36");

            entity.ToTable("AssetURL");

            entity.Property(e => e.Type)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Url).HasColumnType("text");

            entity.HasOne(d => d.Business).WithMany(p => p.AssetUrls)
                .HasForeignKey(d => d.BusinessId)
                .HasConstraintName("FK__AssetURL__Busine__114A936A");

            entity.HasOne(d => d.Feedback).WithMany(p => p.AssetUrls)
                .HasForeignKey(d => d.FeedbackId)
                .HasConstraintName("FK__AssetURL__Feedba__123EB7A3");

            entity.HasOne(d => d.Material).WithMany(p => p.AssetUrls)
                .HasForeignKey(d => d.MaterialId)
                .HasConstraintName("FK__AssetURL__Materi__14270015");

            entity.HasOne(d => d.OrderDetail).WithMany(p => p.AssetUrls)
                .HasForeignKey(d => d.OrderDetailId)
                .HasConstraintName("FK__AssetURL__OrderD__160F4887");

            entity.HasOne(d => d.Service).WithMany(p => p.AssetUrls)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__AssetURL__Servic__1332DBDC");

            entity.HasOne(d => d.Ticket).WithMany(p => p.AssetUrls)
                .HasForeignKey(d => d.TicketId)
                .HasConstraintName("FK__AssetURL__Ticket__151B244E");
        });

        modelBuilder.Entity<BranchMaterial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BranchMa__3214EC074CEBE989");

            entity.ToTable("BranchMaterial");

            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Branch).WithMany(p => p.BranchMaterials)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BranchMat__Branc__6477ECF3");

            entity.HasOne(d => d.Material).WithMany(p => p.BranchMaterials)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BranchMat__Mater__6383C8BA");
        });

        modelBuilder.Entity<BranchService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BranchSe__3214EC074C6FE037");

            entity.ToTable("BranchService");

            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Branch).WithMany(p => p.BranchServices)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BranchSer__Branc__5FB337D6");

            entity.HasOne(d => d.Service).WithMany(p => p.BranchServices)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BranchSer__Servi__5EBF139D");
        });

        modelBuilder.Entity<BusinessBranch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Business__3214EC076B7A762E");

            entity.ToTable("BusinessBranch");

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.District).HasMaxLength(50);
            entity.Property(e => e.EmployeeIds)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Province).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ward).HasMaxLength(50);
            entity.Property(e => e.WardCode)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Business).WithMany(p => p.BusinessBranches)
                .HasForeignKey(d => d.BusinessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BusinessB__Busin__4BAC3F29");
        });

        modelBuilder.Entity<BusinessProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Business__3214EC07716BBE8E");

            entity.ToTable("BusinessProfile");

            entity.HasIndex(e => e.Phone, "UQ__Business__5C7E359E9AD5951E").IsUnique();

            entity.HasIndex(e => e.Name, "UQ__Business__737584F6A022281C").IsUnique();

            entity.HasIndex(e => e.OwnerId, "UQ__Business__819385B99B8BCB8C").IsUnique();

            entity.Property(e => e.ExpiredTime).HasColumnType("datetime");
            entity.Property(e => e.ImageUrl).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Rating).HasColumnType("decimal(3, 1)");
            entity.Property(e => e.RegisteredTime).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Owner).WithOne(p => p.BusinessProfile)
                .HasForeignKey<BusinessProfile>(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BusinessP__Owner__47DBAE45");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cart__3214EC07C797B876");

            entity.ToTable("Cart");

            entity.HasIndex(e => e.AccountId, "UQ__Cart__349DA5A7AB3A0EF6").IsUnique();

            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Account).WithOne(p => p.Cart)
                .HasForeignKey<Cart>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart__AccountId__6D0D32F4");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CartItem__3214EC07222D5C0E");

            entity.ToTable("CartItem");

            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Branch).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartItem__Branch__70DDC3D8");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartItem__CartId__6FE99F9F");

            entity.HasOne(d => d.Material).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.MaterialId)
                .HasConstraintName("FK__CartItem__Materi__72C60C4A");

            entity.HasOne(d => d.Service).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__CartItem__Servic__71D1E811");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC07207082AD");

            entity.ToTable("Feedback");

            entity.HasIndex(e => e.OrderItemId, "UQ__Feedback__57ED0680B60E1939").IsUnique();

            entity.Property(e => e.CreatedTime).HasColumnType("datetime");
            entity.Property(e => e.Rating).HasColumnType("decimal(3, 1)");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.OrderItem).WithOne(p => p.Feedback)
                .HasForeignKey<Feedback>(d => d.OrderItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__OrderI__02084FDA");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Material__3214EC0761926726");

            entity.ToTable("Material");

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Service).WithMany(p => p.Materials)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Material__Servic__5AEE82B9");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC07458C2918");

            entity.ToTable("Order");

            entity.Property(e => e.AbandonedTime).HasColumnType("datetime");
            entity.Property(e => e.ApprovedTime).HasColumnType("datetime");
            entity.Property(e => e.CanceledTime).HasColumnType("datetime");
            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.DeliveredFee).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DeliveredTime).HasColumnType("datetime");
            entity.Property(e => e.FinishedTime).HasColumnType("datetime");
            entity.Property(e => e.OrderPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PendingTime).HasColumnType("datetime");
            entity.Property(e => e.ProcessingTime).HasColumnType("datetime");
            entity.Property(e => e.RevievedTime).HasColumnType("datetime");
            entity.Property(e => e.ShippingCode).HasColumnType("text");
            entity.Property(e => e.ShippingTime).HasColumnType("datetime");
            entity.Property(e => e.ShippingUnit).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.StoragedTime).HasColumnType("datetime");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Account).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__AccountId__76969D2E");

            entity.HasOne(d => d.Address).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK__Order__AddressId__778AC167");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3214EC07AE2BF67E");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProcessState).HasMaxLength(100);

            entity.HasOne(d => d.Branch).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Branc__7B5B524B");

            entity.HasOne(d => d.Material).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.MaterialId)
                .HasConstraintName("FK__OrderDeta__Mater__7D439ABD");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__7A672E12");

            entity.HasOne(d => d.Service).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__OrderDeta__Servi__7C4F7684");
        });

        modelBuilder.Entity<OrderNotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderNot__3214EC07B75CDC43");

            entity.ToTable("OrderNotification");

            entity.Property(e => e.NotificationTime).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderNotifications)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderNoti__Order__208CD6FA");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Promotio__3214EC07FBA8AB1A");

            entity.ToTable("Promotion");

            entity.HasIndex(e => e.ServiceId, "UQ__Promotio__C51BB00BD0FBD68C").IsUnique();

            entity.Property(e => e.NewPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Service).WithOne(p => p.Promotion)
                .HasForeignKey<Promotion>(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promotion__Servi__693CA210");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Service__3214EC07B33F8AC0");

            entity.ToTable("Service");

            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Rating).HasColumnType("decimal(3, 1)");
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Category).WithMany(p => p.Services)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Service__Categor__534D60F1");
        });

        modelBuilder.Entity<ServiceCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ServiceC__3214EC07AC92D7C7");

            entity.ToTable("ServiceCategory");

            entity.HasIndex(e => e.Name, "UQ__ServiceC__737584F67FFB482A").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ServiceProcess>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ServiceP__3214EC0792BCA9B4");

            entity.ToTable("ServiceProcess");

            entity.Property(e => e.Process).HasMaxLength(100);

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceProcesses)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServicePr__Servi__571DF1D5");
        });

        modelBuilder.Entity<SubscriptionPack>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subscrip__3214EC0777AFA6D8");

            entity.ToTable("SubscriptionPack");

            entity.HasIndex(e => e.Name, "UQ__Subscrip__737584F6EBEC67F6").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<SupportTicket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SupportT__3214EC0729FBFA56");

            entity.ToTable("SupportTicket");

            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Category).WithMany(p => p.SupportTickets)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SupportTi__Categ__0C85DE4D");

            entity.HasOne(d => d.Moderator).WithMany(p => p.SupportTicketModerators)
                .HasForeignKey(d => d.ModeratorId)
                .HasConstraintName("FK__SupportTi__Moder__0B91BA14");

            entity.HasOne(d => d.Order).WithMany(p => p.SupportTickets)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__SupportTi__Order__0D7A0286");

            entity.HasOne(d => d.User).WithMany(p => p.SupportTicketUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SupportTi__UserI__0A9D95DB");
        });

        modelBuilder.Entity<TicketCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TicketCa__3214EC07BFB62ED9");

            entity.ToTable("TicketCategory");

            entity.HasIndex(e => e.Name, "UQ__TicketCa__737584F64A41C0EE").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC07FE275E50");

            entity.ToTable("Transaction");

            entity.Property(e => e.Balance).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PackName).HasMaxLength(50);
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ProcessTime).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Accou__1DB06A4F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
