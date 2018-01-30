using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using WowStatus.Models;

using Flurl;
using Flurl.Http;
using Newtonsoft.Json;

namespace WowStatus.Models
{
    public class StatusController : Controller
    {

        private static System.Uri GetBaseAddress()
        {
            var baseAddress = new Uri("http://eu.battle.net/api/wow/");
            return baseAddress;
        }

        //
        // GET: /Status/
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetStatus()
        {
            string apiKey = System.Configuration.ConfigurationManager.AppSettings["apikey"] ;

            string urlValues = "get+status=hello world";
            string testStr = Uri.EscapeDataString(urlValues);
 
            // NWE api            
            // 
            HttpResponseMessage wowResponse = new HttpResponseMessage();
            string content = "";

            try
            {
                // fraaier met httpclient -> geen exception, maar dan weer geen flurl ... (denk ik)

                wowResponse = await "https://eu.api.battle.net/"
                          .AppendPathSegments("wow", "realm", "status")
                          .SetQueryParam("locale", "en_GB")
                          .SetQueryParam("apikey", apiKey)
                          .GetAsync();

                // .GetJsonAsync<clsHunter>();

                // fields = pets for battle pets

                content = await wowResponse.Content.ReadAsStringAsync();

            }
            catch(Exception ex) 
            {
                ViewBag.error = "Error !";
                System.IO.File.WriteAllText(@"c:\tmp\wow_status.error", ex.Message);
                return View("Error");
            }

            ViewBag.realmStatus = new Realm() { name= "Onbekend" };

            RealmResponse realmStatus = JsonConvert.DeserializeObject<RealmResponse>(content);
            
            System.IO.File.WriteAllText(@"c:\tmp\realms.json",content);

            // zoeken naar aggramar
            foreach (Realm realm in realmStatus.realms) 
            {
                if (realm.name.ToLower().Contains("aggramar"))
                {
                    ViewBag.realmStatus = realm;
                }
            }

            ViewBag.realmStatus = realmStatus.realms.Find(p => p.name.ToLower().Contains("aggramar"));

            var newsRequest = await "https://eu.api.battle.net/"
                    .AppendPathSegments("wow", "guild", "aggramar", "sabre")
                    .SetQueryParam("locale", "en_GB")
                    .SetQueryParam("fields", "news")
                    .SetQueryParam("apikey", apiKey)
                    .GetAsync();

            var ginfoContent = await newsRequest.Content.ReadAsStringAsync();
            GuildInfo ginfo = JsonConvert.DeserializeObject<GuildInfo>(ginfoContent);
            System.IO.File.WriteAllText(@"c:\tmp\ginfo.json", ginfoContent);
            ViewBag.ginfo = ginfo;

            IEnumerable<string> values;
            int allotted = 0;
            int used = 0;

            if (newsRequest.Headers.TryGetValues("X-Plan-Qps-Allotted", out values))
            {
                string session = values.First();
                allotted = int.Parse(session);
                // Console.WriteLine("QPS Allotted : " + session);
            }

            if (newsRequest.Headers.TryGetValues("X-Plan-Qps-Current", out values))
            {
                string session = values.First();
                used = int.Parse(session);
                //Console.WriteLine("QPS Allotted : " + session);
            }

            ViewBag.qpsData = "QPS Used / Allotted : " + used + " / " + allotted;

            if (newsRequest.Headers.TryGetValues("X-Plan-Quota-Allotted", out values))
            {
                string session = values.First();
                allotted = int.Parse(session);
                // Console.WriteLine("QPS Allotted : " + session);
            }

            if (newsRequest.Headers.TryGetValues("X-Plan-Quota-Current", out values))
            {
                string session = values.First();
                used = int.Parse(session);
                //Console.WriteLine("QPS Allotted : " + session);
            }

            ViewBag.quotaData = "Quota Used / Allotted : " + used + " / " + allotted;

            return View();
        }
	}
}