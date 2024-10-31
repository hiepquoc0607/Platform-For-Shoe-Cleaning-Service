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

    public virtual DbSet<BusinessBranch> BusinessBranches { get; set; }

    public virtual DbSet<BusinessProfile> BusinessProfiles { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderNotification> OrderNotifications { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceCategory> ServiceCategories { get; set; }

    public virtual DbSet<ServiceMaterial> ServiceMaterials { get; set; }

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
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC07357E4740");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Phone, "UQ__Account__5C7E359EDC65A5CD").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Account__A9D1053420FAB0E5").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Fcmtoken)
                .HasColumnType("text")
                .HasColumnName("FCMToken");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl).HasColumnType("text");
            entity.Property(e => e.IsVerified).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.RefreshToken).HasColumnType("text");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AccountAddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AccountA__3214EC07E1569FF3");

            entity.ToTable("AccountAddress");

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Province).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ward).HasMaxLength(100);

            entity.HasOne(d => d.Account).WithMany(p => p.AccountAddresses)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AccountAd__Accou__412EB0B6");
        });

        modelBuilder.Entity<AssetUrl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AssetURL__3214EC0741B1990A");

            entity.ToTable("AssetURL");

            entity.Property(e => e.IsImage).HasDefaultValue(true);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Url).HasColumnType("text");

            entity.HasOne(d => d.Business).WithMany(p => p.AssetUrls)
                .HasForeignKey(d => d.BusinessId)
                .HasConstraintName("FK__AssetURL__Busine__7B5B524B");

            entity.HasOne(d => d.Feedback).WithMany(p => p.AssetUrls)
                .HasForeignKey(d => d.FeedbackId)
                .HasConstraintName("FK__AssetURL__Feedba__7C4F7684");

            entity.HasOne(d => d.Service).WithMany(p => p.AssetUrls)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__AssetURL__Servic__7D439ABD");
        });

        modelBuilder.Entity<BusinessBranch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Business__3214EC07F208C6EB");

            entity.ToTable("BusinessBranch");

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.EmployeeIds)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Province).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ward).HasMaxLength(100);

            entity.HasOne(d => d.Business).WithMany(p => p.BusinessBranches)
                .HasForeignKey(d => d.BusinessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BusinessB__Busin__4BAC3F29");
        });

        modelBuilder.Entity<BusinessProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Business__3214EC07EB9B4AEA");

            entity.ToTable("BusinessProfile");

            entity.HasIndex(e => e.Phone, "UQ__Business__5C7E359E5949A9FF").IsUnique();

            entity.HasIndex(e => e.Name, "UQ__Business__737584F6354FFDD7").IsUnique();

            entity.HasIndex(e => e.OwnerId, "UQ__Business__819385B998B9EAE4").IsUnique();

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
            entity.HasKey(e => e.Id).HasName("PK__Cart__3214EC0776E7108F");

            entity.ToTable("Cart");

            entity.HasIndex(e => e.AccountId, "UQ__Cart__349DA5A7A2092673").IsUnique();

            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Account).WithOne(p => p.Cart)
                .HasForeignKey<Cart>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart__AccountId__6383C8BA");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CartItem__3214EC079DBD0F73");

            entity.ToTable("CartItem");

            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartItem__CartId__66603565");

            entity.HasOne(d => d.Material).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.MaterialId)
                .HasConstraintName("FK__CartItem__Materi__68487DD7");

            entity.HasOne(d => d.Service).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__CartItem__Servic__6754599E");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC078DF2FA1C");

            entity.ToTable("Feedback");

            entity.Property(e => e.CreatedTime).HasColumnType("datetime");
            entity.Property(e => e.Rating).HasColumnType("decimal(3, 1)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.OrderItem).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.OrderItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__OrderI__76969D2E");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC075240B00D");

            entity.ToTable("Location");

            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Province).HasMaxLength(50);
            entity.Property(e => e.Ward).HasMaxLength(50);
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Material__3214EC0713DD21F1");

            entity.ToTable("Material");

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC07FCAD7DB9");

            entity.ToTable("Order");

            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.DeliveredFee).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DeliveredTime).HasColumnType("datetime");
            entity.Property(e => e.OrderPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ShippingCode).HasColumnType("text");
            entity.Property(e => e.ShippingUnit).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Account).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__AccountId__6C190EBB");

            entity.HasOne(d => d.Address).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK__Order__AddressId__6D0D32F4");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3214EC075E5C27DF");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Material).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.MaterialId)
                .HasConstraintName("FK__OrderDeta__Mater__72C60C4A");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__70DDC3D8");

            entity.HasOne(d => d.Service).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__OrderDeta__Servi__71D1E811");
        });

        modelBuilder.Entity<OrderNotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderNot__3214EC0747CAFDB7");

            entity.ToTable("OrderNotification");

            entity.Property(e => e.NotificationTime).HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderNotifications)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderNoti__Order__0B91BA14");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentM__3214EC07F31E0723");

            entity.ToTable("PaymentMethod");

            entity.HasIndex(e => e.Name, "UQ__PaymentM__737584F67EB39B3D").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Promotio__3214EC07FCD1E9BE");

            entity.ToTable("Promotion");

            entity.HasIndex(e => e.ServiceId, "UQ__Promotio__C51BB00B36DE195E").IsUnique();

            entity.Property(e => e.NewPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Service).WithOne(p => p.Promotion)
                .HasForeignKey<Promotion>(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promotion__Servi__5FB337D6");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Service__3214EC07AE51A6D7");

            entity.ToTable("Service");

            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Rating).HasColumnType("decimal(3, 1)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Branch).WithMany(p => p.Services)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Service__BranchI__5441852A");

            entity.HasOne(d => d.Category).WithMany(p => p.Services)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Service__Categor__534D60F1");
        });

        modelBuilder.Entity<ServiceCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ServiceC__3214EC07C1A80AAB");

            entity.ToTable("ServiceCategory");

            entity.HasIndex(e => e.Name, "UQ__ServiceC__737584F69019FF67").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ServiceMaterial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ServiceM__3214EC077FF79FD5");

            entity.ToTable("ServiceMaterial");

            entity.HasOne(d => d.Material).WithMany(p => p.ServiceMaterials)
                .HasForeignKey(d => d.MaterialId)
                .HasConstraintName("FK__ServiceMa__Mater__5AEE82B9");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceMaterials)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServiceMa__Servi__59FA5E80");
        });

        modelBuilder.Entity<SubscriptionPack>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subscrip__3214EC076827D139");

            entity.ToTable("SubscriptionPack");

            entity.HasIndex(e => e.Name, "UQ__Subscrip__737584F688E082C5").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<SupportTicket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SupportT__3214EC07E486F11C");

            entity.ToTable("SupportTicket");

            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Category).WithMany(p => p.SupportTickets)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SupportTi__Categ__160F4887");

            entity.HasOne(d => d.Moderator).WithMany(p => p.SupportTicketModerators)
                .HasForeignKey(d => d.ModeratorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SupportTi__Moder__151B244E");

            entity.HasOne(d => d.Order).WithMany(p => p.SupportTickets)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__SupportTi__Order__17036CC0");

            entity.HasOne(d => d.User).WithMany(p => p.SupportTicketUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SupportTi__UserI__14270015");
        });

        modelBuilder.Entity<TicketCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TicketCa__3214EC077E8EA34A");

            entity.ToTable("TicketCategory");

            entity.HasIndex(e => e.Name, "UQ__TicketCa__737584F68402F5D6").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC0767F85638");

            entity.ToTable("Transaction");

            entity.Property(e => e.Balance).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProcessTime).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Accou__06CD04F7");

            entity.HasOne(d => d.Method).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.MethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Metho__07C12930");

            entity.HasOne(d => d.Pack).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.PackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__PackI__08B54D69");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
