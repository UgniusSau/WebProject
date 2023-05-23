using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Blazelounge_v2.Data.Models;

public partial class DbContext2 : DbContext
{
    public DbContext2()
    {
    }

    public DbContext2(DbContextOptions<DbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BackgroundColor> BackgroundColors { get; set; }

    public virtual DbSet<Badge> Badges { get; set; }

    public virtual DbSet<Emote> Emotes { get; set; }

    public virtual DbSet<GamesStat> GamesStats { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<InventoryItem> InventoryItems { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemType> ItemTypes { get; set; }

    public virtual DbSet<NameColor> NameColors { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<Statistic> Statistics { get; set; }

    public virtual DbSet<StyleType> StyleTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<DailySpin> DailySpins { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=db.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BackgroundColor>(entity =>
        {
            entity.HasKey(e => e.IdBackgroundColor);

            entity.ToTable("Background_colors");

            entity.HasIndex(e => e.FkItemidItem, "IX_Background_colors_fk_Itemid_Item").IsUnique();

            entity.Property(e => e.IdBackgroundColor).HasColumnName("id_Background_color");
            entity.Property(e => e.FkItemidItem).HasColumnName("fk_Itemid_Item");
            entity.Property(e => e.RgbValue)
                .HasColumnType("varchar(255)")
                .HasColumnName("RGB_value");

        });

        modelBuilder.Entity<Badge>(entity =>
        {
            entity.HasKey(e => e.IdBadge);

            entity.HasIndex(e => e.FkItemidItem, "IX_Badges_fk_Itemid_Item").IsUnique();

            entity.Property(e => e.IdBadge).HasColumnName("id_Badge");
            entity.Property(e => e.FkItemidItem).HasColumnName("fk_Itemid_Item");
            entity.Property(e => e.Url).HasColumnType("varchar(255)");

        });

        modelBuilder.Entity<Emote>(entity =>
        {
            entity.HasKey(e => e.IdEmote);

            entity.HasIndex(e => e.FkItemidItem, "IX_Emotes_fk_Itemid_Item").IsUnique();

            entity.Property(e => e.IdEmote).HasColumnName("id_Emote");
            entity.Property(e => e.FkItemidItem).HasColumnName("fk_Itemid_Item");
            entity.Property(e => e.Url).HasColumnType("varchar(255)");
        });

        modelBuilder.Entity<GamesStat>(entity =>
        {
            entity.HasKey(e => e.IdGameStat);

            entity.ToTable("Games_stats");

            entity.Property(e => e.IdGameStat).HasColumnName("id_Game_stat");
            entity.Property(e => e.CurrencyGained)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("Currency_gained");
            entity.Property(e => e.FkStatisticsidStatistics).HasColumnName("fk_Statisticsid_Statistics");
            entity.Property(e => e.Game).HasColumnType("varchar(255)");
            entity.Property(e => e.Loses).HasColumnType("INT");
            entity.Property(e => e.TimesPlayed)
                .HasColumnType("INT")
                .HasColumnName("Times_Played");
            entity.Property(e => e.Wins).HasColumnType("INT");


        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.IdInventory);

            entity.HasIndex(e => e.FkProfileidProfile, "IX_Inventories_fk_Profileid_Profile").IsUnique();

            entity.Property(e => e.IdInventory).HasColumnName("id_Inventory");
            entity.Property(e => e.FkProfileidProfile).HasColumnName("fk_Profileid_Profile");


        });

        modelBuilder.Entity<InventoryItem>(entity =>
        {
            entity.HasKey(e => e.IdInventoryItems);

            entity.ToTable("Inventory_Items");

            entity.Property(e => e.IdInventoryItems).HasColumnName("id_Inventory_Items");
            entity.Property(e => e.FkInventoryidInventory).HasColumnName("fk_Inventoryid_Inventory");
            entity.Property(e => e.FkItemidItem).HasColumnName("fk_Itemid_Item");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.IdItem);

            entity.Property(e => e.IdItem).HasColumnName("id_Item");
            entity.Property(e => e.Description).HasColumnType("varchar(255)");
            entity.Property(e => e.FkShopidShop).HasColumnName("fk_Shopid_Shop");
            entity.Property(e => e.Name).HasColumnType("varchar(255)");
            entity.Property(e => e.Price).HasColumnType("decimal(8, 2)");
        });

        modelBuilder.Entity<ItemType>(entity =>
        {
            entity.HasKey(e => e.IdItemType);

            entity.ToTable("Item_types");

            entity.Property(e => e.IdItemType).HasColumnName("id_Item_type");
            entity.Property(e => e.Name)
                .HasColumnType("char(16)")
                .HasColumnName("name");
        });

        modelBuilder.Entity<NameColor>(entity =>
        {
            entity.HasKey(e => e.IdNameColor);

            entity.ToTable("Name_colors");

            entity.HasIndex(e => e.FkItemidItem, "IX_Name_colors_fk_Itemid_Item").IsUnique();

            entity.Property(e => e.IdNameColor).HasColumnName("id_Name_color");
            entity.Property(e => e.FkItemidItem).HasColumnName("fk_Itemid_Item");
            entity.Property(e => e.RgbValue)
                .HasColumnType("varchar(255)")
                .HasColumnName("RGB_value");

        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.IdProfile);

            entity.Property(e => e.IdProfile).HasColumnName("id_Profile");
            entity.Property(e => e.BaseBgColor)
                .HasColumnType("varchar(255)")
                .HasColumnName("Base_bg_color");
            entity.Property(e => e.BaseNameColor)
                .HasColumnType("varchar(255)")
                .HasColumnName("Base_name_color");
            entity.Property(e => e.Currency).HasColumnType("decimal(8, 2)");
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasKey(e => e.IdShop);

            entity.Property(e => e.IdShop).HasColumnName("id_Shop");
        });

        modelBuilder.Entity<Statistic>(entity =>
        {
            entity.HasKey(e => e.IdStatistics);

            entity.HasIndex(e => e.FkProfileidProfile, "IX_Statistics_fk_Profileid_Profile").IsUnique();

            entity.Property(e => e.IdStatistics).HasColumnName("id_Statistics");
            entity.Property(e => e.FkProfileidProfile).HasColumnName("fk_Profileid_Profile");

        });

        modelBuilder.Entity<StyleType>(entity =>
        {
            entity.HasKey(e => e.IdStyleType);

            entity.ToTable("Style_types");

            entity.Property(e => e.IdStyleType).HasColumnName("id_Style_type");
            entity.Property(e => e.Name)
                .HasColumnType("char(11)")
                .HasColumnName("name");
        });

        modelBuilder.Entity<DailySpin>(entity =>
        {
            entity.HasKey(e => e.UUID);

            entity.ToTable("Daily_spins");

            entity.Property(e => e.UUID).HasColumnName("uuid");
            entity.Property(e => e.SpinTime).HasColumnName("last_spin");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser);

            entity.HasIndex(e => e.FkProfileidProfile, "IX_Users_fk_Profileid_Profile").IsUnique();

            entity.Property(e => e.IdUser).HasColumnName("id_User");
            entity.Property(e => e.Admin).HasColumnType("boolean");
            entity.Property(e => e.Email).HasColumnType("varchar(255)");
            entity.Property(e => e.FirstName)
                .HasColumnType("varchar(255)")
                .HasColumnName("First_Name");
            entity.Property(e => e.FkProfileidProfile).HasColumnName("fk_Profileid_Profile");
            entity.Property(e => e.LastName)
                .HasColumnType("varchar(255)")
                .HasColumnName("Last_Name");
            entity.Property(e => e.PasswordHashed)
                .HasColumnType("varchar(255)")
                .HasColumnName("Password_Hashed");
            entity.Property(e => e.Username).HasColumnType("varchar(255)");
            entity.Property(e => e.Uuid)
                .HasColumnType("char(36)")
                .HasColumnName("UUID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
