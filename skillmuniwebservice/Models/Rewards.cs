using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class Rewards
    {
    }
    public class CouponsRedeemed
    {
        
        public int id_CouponsRedeemed { get; set; }
        public string CouponID { get; set; }
        public string WebsiteName { get; set; }
        public string CouponCode { get; set; }
        public string CouponDescription { get; set; }
        public string Link { get; set; }
        public string PointsUsed { get; set; }
        public string Image { get; set; }
        public string ExpiryDate { get; set; }
        public int usedmoney { get; set; }
    }

    public class MiscellaneousData1
    {
        public List<CouponsRedeemed> CouponsRedeemed { get; set; }
    }

    public class MiscellaneousData2
    {
        public string BalancePoints { get; set; }
    }

    public class RootObject
    {
        public string AccountID { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string PartnerCode { get; set; }
        public string ProviderCode { get; set; }
        public string RedeemType { get; set; }
        public string TransactionID { get; set; }
        public MiscellaneousData1 MiscellaneousData1 { get; set; }
        public MiscellaneousData2 MiscellaneousData2 { get; set; }
        public string TotalPoints { get; set; }
        public int id_user { get; set; }
        public int id_game { get; set; }
        public int id_org { get; set; }
    }
}