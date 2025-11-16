namespace AtharPlatform.Models.Enum
{
    public enum NotificationsTypeEnum
    {
        NewCharity = 1,  // Send this notification to the admin to check the new charity signIn
        AdminApproved = 2, // Send this notification to the charity to notifiy it is accepted
        AdminRejected = 3, // Send this notification to the charity to notifiy it is accepted
        NewCampagin = 4, // Send this notification to charity subscribers to notifiy them there is a new campain
        NewSubscriber = 5, // Send this notitfication to charity to notifiy it there is a new subscriber
        NewFollower =  6// Send this notitfication to charity to notifiy it there is a new follower
    }
}
