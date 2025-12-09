using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AtharPlatform.Models
{
    public class Context : IdentityDbContext<UserAccount, IdentityRole<int>, int>
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        // Users and Charities
        public DbSet<Donor> Donors { get; set; }
        public DbSet<Charity> Charities { get; set; }
        public DbSet<CharityExternalInfo> CharityExternalInfos { get; set; }

        // Subscriptions
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        // Content & Reactions
        public DbSet<Content> Contents { get; set; }
        public DbSet<Reaction> Reactions { get; set; }

        // Campaigns
        public DbSet<Campaign> Campaigns { get; set; }

        // Donations
        public DbSet<Donation> Donations { get; set; }
        public DbSet<MaterialDonation> MaterialDonations { get; set; }
        public DbSet<CharityMaterialDonation> CharityMaterialDonations { get; set; }
        public DbSet<CampaignDonation> CampaignDonations { get; set; }
        public DbSet<CharityDonation> CharityDonations { get; set; }

        // Notifications
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationSender> Sender { get; set; }
        public DbSet<NotificationReceiver> Receivers { get; set; }

        // Volunteer & Vendor
        public DbSet<VolunteerApplication> VolunteerForm { get; set; }
        public DbSet<CharityVolunteer> CharityVolunteers { get; set; }
        public DbSet<VendorOffers> VendorForms { get; set; }
        public DbSet<CharityVendorOffer> CharityVendorOffers { get; set; }

        // noen
        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);

        //    // Charity 1:1 external info
        //    builder.Entity<Charity>()
        //        .HasOne(c => c.ScrapedInfo)
        //        .WithOne(e => e.Charity)
        //        .HasForeignKey<CharityExternalInfo>(e => e.CharityId)
        //        .OnDelete(DeleteBehavior.Cascade);

        //    // Many-to-Many Relation with composite PK
        //    builder.Entity<NotificationReceiver>()
        //        .HasKey(nr => new { nr.NotificationId, nr.ReceiverId });

        //    builder.Entity<Notification>()
        //        .HasOne(n => n.Sender)
        //        .WithOne(ns => ns.Notification)
        //        .HasForeignKey<NotificationSender>(ns => ns.NotificationId)
        //        .OnDelete(DeleteBehavior.Cascade);

        //    // put unique index
        //    builder.Entity<Follow>()
        //       .HasIndex(f => new { f.DonorId, f.CharityId })
        //       .IsUnique();

        //    builder.Entity<Subscription>()
        //      .HasIndex(s => new { s.DonorId, s.CharityId })
        //      .IsUnique();

        //    // Configure delete behaviors to avoid multiple cascade paths
        //    builder.Entity<Follow>()
        //        .HasOne(f => f.Donor)
        //        .WithMany(d => d.Follows)
        //        .HasForeignKey(f => f.DonorId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    builder.Entity<Follow>()
        //        .HasOne(f => f.Charity)
        //        .WithMany(c => c.Follows)
        //        .HasForeignKey(f => f.CharityId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    builder.Entity<Subscription>()
        //        .HasOne(s => s.Donor)
        //        .WithMany(d => d.Subscriptions)
        //        .HasForeignKey(s => s.DonorId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    builder.Entity<Subscription>()
        //        .HasOne(s => s.Charity)
        //        .WithMany(c => c.Subscriptions)
        //        .HasForeignKey(s => s.CharityId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    // Configure Donation relationships to avoid multiple cascade paths
        //    builder.Entity<Donation>()
        //        .HasMany(d => d.CampaignDonations)
        //        .WithOne(cd => cd.Donation)
        //        .HasForeignKey(cd => cd.DonationId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    builder.Entity<Donation>()
        //        .HasMany(d => d.CharityDonations)
        //        .WithOne(cd => cd.Donation)
        //        .HasForeignKey(cd => cd.DonationId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    builder.Entity<CampaignDonation>()
        //        .HasOne(cd => cd.Campaign)
        //        .WithMany(c => c.CampaignDonations)
        //        .HasForeignKey(cd => cd.CampaignId)
        //        .OnDelete(DeleteBehavior.Cascade);

        //    // Reactions: prevent cascade from Donor + Content simultaneously
        //    builder.Entity<Reaction>()
        //        .HasOne(r => r.Donor)
        //        .WithMany(d => d.Reactions)
        //        .HasForeignKey(r => r.DonorID)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    builder.Entity<Reaction>()
        //        .HasOne(r => r.Content)
        //        .WithMany(c => c.Reactions)
        //        .HasForeignKey(r => r.ContentID)
        //        .OnDelete(DeleteBehavior.Cascade); // only content cascade allowed

        //    // Campaign validation: must have either Image or ImageUrl, but not both
        //    //builder.Entity<Campaign>()
        //    //    .ToTable(c => c.HasCheckConstraint(
        //    //        "CK_Campaign_ImageSource",
        //    //        "(\"Image\" IS NOT NULL AND \"ImageUrl\" IS NULL) OR (\"Image\" IS NULL AND \"ImageUrl\" IS NOT NULL)"
        //    //    ));
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);


            builder.Entity<NotificationReceiver>()
                .HasKey(nr => new { nr.NotificationId, nr.ReceiverId });

            builder.Entity<Notification>()
                .HasOne(n => n.Sender)
                .WithOne(ns => ns.Notification)
                .HasForeignKey<NotificationSender>(ns => ns.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);

            // IdentityUserLogin<int> composite key
            builder.Entity<IdentityUserLogin<int>>(entity =>
            {
                entity.HasKey(l => new { l.LoginProvider, l.ProviderKey });
            });

            // IdentityUserRole<int> composite key
            builder.Entity<IdentityUserRole<int>>(entity =>
            {
                entity.HasKey(r => new { r.UserId, r.RoleId });
            });

            // IdentityUserToken<int> composite key
            builder.Entity<IdentityUserToken<int>>(entity =>
            {
                entity.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
            });

            // IdentityRoleClaim<int> primary key
            builder.Entity<IdentityRoleClaim<int>>(entity =>
            {
                entity.HasKey(rc => rc.Id);
            });

            // IdentityUserClaim<int> primary key
            builder.Entity<IdentityUserClaim<int>>(entity =>
            {
                entity.HasKey(uc => uc.Id);
            });



        }

    }
}