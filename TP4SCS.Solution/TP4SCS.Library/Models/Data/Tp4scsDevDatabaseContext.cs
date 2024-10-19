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

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderNotification> OrderNotifications { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceCategory> ServiceCategories { get; set; }

    public virtual DbSet<Statistic> Statistics { get; set; }

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
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC07F457E10C");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Phone, "UQ__Account__5C7E359E2FC9193A").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Account__A9D105349B6F33FE").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ExpiredTime)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime");
            entity.Property(e => e.Fcmtoken)
                .HasColumnType("text")
                .HasColumnName("FCMToken");
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl).HasColumnType("text");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
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
            entity.HasKey(e => e.Id).HasName("PK__AccountA__3214EC0708B71FF0");

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
                .HasConstraintName("FK__AccountAd__Accou__7F2BE32F");
        });

        modelBuilder.Entity<AssetUrl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AssetURL__3214EC075A946315");

            entity.ToTable("AssetURL");

            entity.Property(e => e.IsImage).HasDefaultValue(true);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Url).HasColumnType("text");

            entity.HasOne(d => d.BusinessProfile).WithMany(p => p.AssetUrls)
                .HasForeignKey(d => d.BusinessProfileId)
                .HasConstraintName("FK__AssetURL__Busine__797309D9");

            entity.HasOne(d => d.Feedback).WithMany(p => p.AssetUrls)
                .HasForeignKey(d => d.FeedbackId)
                .HasConstraintName("FK__AssetURL__Feedba__7A672E12");

            entity.HasOne(d => d.Service).WithMany(p => p.AssetUrls)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__AssetURL__Servic__7B5B524B");
        });

        modelBuilder.Entity<BusinessBranch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Business__3214EC07F039126D");

            entity.ToTable("BusinessBranch");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.BranchName).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.EmployeeIds)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Province).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ward).HasMaxLength(100);

            entity.HasOne(d => d.Owner).WithMany(p => p.BusinessBranches)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BusinessB__Owner__52593CB8");
        });

        modelBuilder.Entity<BusinessProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Business__3214EC0718F928C4");

            entity.ToTable("BusinessProfile");

            entity.HasIndex(e => e.Phone, "UQ__Business__5C7E359E1B70DECC").IsUnique();

            entity.HasIndex(e => e.Name, "UQ__Business__737584F6AF9BE15B").IsUnique();

            entity.Property(e => e.ExpireTime).HasColumnType("datetime");
            entity.Property(e => e.ImageUrl).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Rating).HasColumnType("decimal(3, 1)");
            entity.Property(e => e.RegisterTime).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Owner).WithMany(p => p.BusinessProfiles)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BusinessP__Owner__6A30C649");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cart__3214EC072A696188");

            entity.ToTable("Cart");

            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Account).WithMany(p => p.Carts)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart__AccountId__60A75C0F");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CartItem__3214EC0718AEA0EA");

            entity.ToTable("CartItem");

            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartItem__CartId__6383C8BA");

            entity.HasOne(d => d.Service).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartItem__Servic__6477ECF3");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC07DF01AEE1");

            entity.ToTable("Feedback");

            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.Rating).HasColumnType("decimal(3, 1)");

            entity.HasOne(d => d.OrderItem).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.OrderItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__OrderI__74AE54BC");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC07B2BCABC4");

            entity.ToTable("Order");

            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.DeliveredTime).HasColumnType("datetime");
            entity.Property(e => e.Note).HasColumnType("text");
            entity.Property(e => e.OrderPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ShipFee).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ShippingCode)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ShippingUnit).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Account).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__AccountId__6E01572D");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3214EC07EC0ECB4A");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__70DDC3D8");

            entity.HasOne(d => d.Service).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Servi__71D1E811");
        });

        modelBuilder.Entity<OrderNotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderNot__3214EC073452DA30");

            entity.ToTable("OrderNotification");

            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.NotificationTime).HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderNotifications)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderNoti__Order__0B91BA14");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentM__3214EC07252E1227");

            entity.ToTable("PaymentMethod");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Promotio__3214EC07CE34FD20");

            entity.ToTable("Promotion");

            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.NewPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Service).WithMany(p => p.Promotions)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promotion__Servi__5DCAEF64");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Service__3214EC07850D7AB6");

            entity.ToTable("Service");

            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Rating).HasColumnType("decimal(3, 1)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Branch).WithMany(p => p.Services)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Service__BranchI__59FA5E80");

            entity.HasOne(d => d.Category).WithMany(p => p.Services)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Service__Categor__59063A47");
        });

        modelBuilder.Entity<ServiceCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ServiceC__3214EC0737126168");

            entity.ToTable("ServiceCategory");

            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Statistic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Statisti__3214EC078730AED5");

            entity.ToTable("Statistic");

            entity.Property(e => e.Raise).HasColumnType("decimal(3, 1)");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Value).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<SubscriptionPack>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subscrip__3214EC079C84DCBB");

            entity.ToTable("SubscriptionPack");

            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<SupportTicket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SupportT__3214EC0701A742CA");

            entity.ToTable("SupportTicket");

            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.SupportTickets)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SupportTi__Accou__123EB7A3");

            entity.HasOne(d => d.Category).WithMany(p => p.SupportTickets)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SupportTi__Categ__1332DBDC");

            entity.HasOne(d => d.Order).WithMany(p => p.SupportTickets)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__SupportTi__Order__14270015");
        });

        modelBuilder.Entity<TicketCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TicketCa__3214EC070CF6B22A");

            entity.ToTable("TicketCategory");

            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC07ECA0DBCB");

            entity.ToTable("Transaction");

            entity.Property(e => e.Balance).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Description).HasColumnType("text");
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
