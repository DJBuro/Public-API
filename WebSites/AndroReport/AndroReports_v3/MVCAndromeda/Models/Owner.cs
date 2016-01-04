using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCAndromeda.Models
{
    public class Owner
    {
        public string Name { get; set; }

        public Dictionary<string, Store> Stores { get; set; }

        public Dictionary<string, List<string>> CountriesAndStores{get; set;}
        
        private List<string> countries;
        //----------------------------------------------------
        public List<string> Countries
        {
            get
            {
                return countries;
            }
        }
        //------------  Constructor----------------------------------------
        public Owner(string name, Dictionary<string, List<string>> countriesAndStores)
        {
            Name = name;
            CountriesAndStores = countriesAndStores;
            countries = new List<string>(CountriesAndStores.Keys);
            countries.Sort();
            Stores = new Dictionary<string, Store>();
            foreach (var cs in CountriesAndStores)
            {
                foreach (var storename in cs.Value)
                {
                    Stores.Add(storename, new Store(storename, cs.Key));
                }
            }
        }
        //-------------check is it contains the store---------------------------
        public bool hasStore(string storeName)
        {
            return Stores.Keys.Contains(storeName) ? true : false;
        }
        //-------------------------order by name of datafied----------------------------------
        public void orderby(int orderHow /* -1 desc, 1 asc*/, int orderWhat)
        {
            if (orderWhat == 0)
            {
                foreach (var country in CountriesAndStores.Keys)
                {
                    CountriesAndStores[country].Sort((string storeName1, string storeName2) => orderHow * storeName1.CompareTo(storeName2));
                }
            }
            else
            {
                foreach (var country in CountriesAndStores.Keys)
                {
                    CountriesAndStores[country].Sort((string storeName1, string storeName2) => 
                        orderHow * (
                        (long)(Stores[storeName1].StoreMeasures[orderWhat-1][0]==null?
                        orderHow==-1?Int64.MinValue:Int64.MaxValue
                        :Stores[storeName1].StoreMeasures[orderWhat-1][0])
                        ).CompareTo(
                        (long)(Stores[storeName2].StoreMeasures[orderWhat-1][0]==null?
                        orderHow==-1?Int64.MinValue:Int64.MaxValue
                        :Stores[storeName2].StoreMeasures[orderWhat-1][0]))
                        );
                }
            }
        }
        //---------------------------------------------
        public void Show()
        {
            Console.WriteLine("Owner: " + Name);

            foreach (var coun in CountriesAndStores)
            {
                Console.WriteLine("\n\rStores in country " + coun.Key);
                Console.WriteLine("---------------------------");
                foreach (var storename in coun.Value)
                {
                    Console.WriteLine(Stores[storename].ToString());
                }
            }

        }
    }
}
