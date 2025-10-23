using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AtharPlatform.Models
{
    public class Context : IdentityDbContext<UserAccount, IdentityRole<int>, int>
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        // Users and Charities
        public DbSet<Donor> Donors { get; set; }
        public DbSet<Charity> Charities { get; set; }

        // Subscriptions
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscribtionType> SubscriptionTypes { get; set; }

        // Content & Reactions
        public DbSet<Content> Contents { get; set; }
        public DbSet<Reaction> Reactions { get; set; }

        // Campaigns
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignContent> CampaignContents { get; set; }

        // Donations
        public DbSet<Donation> Donations { get; set; }
        public DbSet<MaterialDonation> MaterialDonations { get; set; }
        public DbSet<CampaignDonation> CampaignDonations { get; set; }
        public DbSet<CharityDonation> CharityDonations { get; set; }

        // Notifications
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationSender> SendNotifications { get; set; }
        public DbSet<NotificationReceiver> ReceiveNotifications { get; set; }

        // Volunteer & Vendor
        public DbSet<VolunteerApplication> VolunteerForm { get; set; }
        public DbSet<CharityVolunteer> CharityVolunteers { get; set; }
        public DbSet<VendorOffers> VendorForms { get; set; }
        public DbSet<CharityVendorOffer> CharityVendorOffers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<NotificationReceiver>()
                .HasKey(a => new { a.NotificationId, a.ReceiverId });
        }

    }
}
