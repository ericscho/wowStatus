using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WowStatus.Models
{

    public class Emblem
    {
        public int icon { get; set; }
        public string iconColor { get; set; }
        public int iconColorId { get; set; }
        public int border { get; set; }
        public string borderColor { get; set; }
        public int borderColorId { get; set; }
        public string backgroundColor { get; set; }
        public int backgroundColorId { get; set; }
    }

    public class Criterion
    {
        public int id { get; set; }
        public string description { get; set; }
        public int orderIndex { get; set; }
        public int max { get; set; }
    }

    public class Achievement
    {
        public int id { get; set; }
        public string title { get; set; }
        public int points { get; set; }
        public string description { get; set; }
        public List<object> rewardItems { get; set; }
        public string icon { get; set; }
        public List<Criterion> criteria { get; set; }
        public bool accountWide { get; set; }
        public int factionId { get; set; }
    }

    public class News
    {
        public string type { get; set; }
        public string character { get; set; }
        public object timestamp { get; set; }
        public int itemId { get; set; }
        public string context { get; set; }
        public List<object> bonusLists { get; set; }
        public Achievement achievement { get; set; }

        public DateTime getRealDate()
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            return origin.AddMilliseconds(long.Parse(this.timestamp.ToString()));
        }

        public string getTypeDescr()
        {
            string value = string.Empty;

            switch (this.type)
            {
                case "playerAchievement":
                    value = " has achieved ";
                    break;

                case "itemLoot":
                    value = " looted item ";
                    break;

                case "itemPurchase":
                    value = " purchased item ";
                    break;

                case "itemCraft":
                    value = " crafted item ";
                    break;

                default:
                    break;
            }

            return value;
        }

        public string getAchievement()
        {
            if (achievement != null)
            {
                return string.Format("{0} - {1}", achievement.title, achievement.description);
            }
            else
                return "onbekend";
        }
    }

    public class GuildInfo
    {
        public long lastModified { get; set; }
        public string name { get; set; }
        public string realm { get; set; }
        public string battlegroup { get; set; }
        public int level { get; set; }
        public int side { get; set; }
        public int achievementPoints { get; set; }
        public Emblem emblem { get; set; }
        public List<News> news { get; set; }
    }
}