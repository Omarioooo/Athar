using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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

            // Composite PK for NotificationReceiver (many-to-many Notification <-> Receiver)
            builder.Entity<NotificationReceiver>()
                .HasKey(nr => new { nr.NotificationId, nr.ReceiverId });

            // 1:1 Notification -> NotificationSender
            builder.Entity<Notification>()
                .HasOne(n => n.Sender)
                .WithOne(ns => ns.Notification)
                .HasForeignKey<NotificationSender>(ns => ns.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique indices for Follow and Subscription to prevent duplicates
            builder.Entity<Follow>()
                .HasIndex(f => new { f.DonorId, f.CharityId })
                .IsUnique();

            builder.Entity<Subscription>()
                .HasIndex(s => new { s.DonorId, s.CharityId })
                .IsUnique();

            // Charity 1:1 external info
            builder.Entity<Charity>()
                .HasOne(c => c.ScrapedInfo)
                .WithOne(e => e.Charity)
                .HasForeignKey<CharityExternalInfo>(e => e.CharityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}