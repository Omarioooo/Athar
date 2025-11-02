using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AtharPlatform.Models
{
    public class Context : IdentityDbContext<UserAccount, IdentityRole<int>, int>
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        // Users and Charities
        public DbSet<Donor> Donors { get; set; }
        public DbSet<Charity> Charities { get; set; }

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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Many-to-Many Relation with composite PK
            builder.Entity<NotificationReceiver>()
                .HasKey(a => new { a.NotificationId, a.ReceiverId });

            // Many-to-Many Relation with composite PK
            builder.Entity<NotificationReceiver>()
                .HasKey(nr => new { nr.NotificationId, nr.ReceiverId });

            builder.Entity<Notification>()
                .HasOne(n => n.Sender)
                .WithOne(ns => ns.Notification)
                .HasForeignKey<NotificationSender>(ns => ns.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);

            // put unique index
            builder.Entity<Follow>()
               .HasIndex(f => new { f.DonorId, f.CharityId })
               .IsUnique();

            builder.Entity<Subscription>()
       .HasIndex(s => new { s.DonorId, s.CharityId })
       .IsUnique();

        }
    }
}