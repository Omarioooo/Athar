using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AtharPlatform.Models
{
    public class Context : IdentityDbContext<UserAccount>
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        // Users and Charities
        public DbSet<User> Users { get; set; }
        public DbSet<Charity> Charities { get; set; }

        // Roles
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }

        // Subscriptions
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscribtionType> SubscriptionTypes { get; set; }

        // Content & Reactions
        public DbSet<Content> Contents { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<UserContetReaction> UserContentReactions { get; set; }

        // Campaigns
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CharityCampaign> CharityCampaigns { get; set; }
        public DbSet<CampaignContent> CampaignContents { get; set; }

        // Donations
        public DbSet<Donation> Donations { get; set; }
        public DbSet<MaterialDonation> MaterialDonations { get; set; }
        public DbSet<CampaignDonation> CampaignDonations { get; set; }
        public DbSet<CharityDonation> CharityDonations { get; set; }

        // Notifications
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<SendNotification> SendNotifications { get; set; }
        public DbSet<ReceiveNotification> ReceiveNotifications { get; set; }

        // Volunteer & Vendor
        public DbSet<VolunteerForm> VolunteerForm { get; set; }
        public DbSet<CharityVolunteer> CharityVolunteers { get; set; }
        public DbSet<VendorForm> VendorForms { get; set; }
        public DbSet<CharityVendorOffer> CharityVendorOffers { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Composite key for UserRoles
            builder.Entity<UserRoles>()
                   .HasIndex(ur => new { ur.AccountId, ur.RoleId })
                   .IsUnique();

         
        }
    }
}
