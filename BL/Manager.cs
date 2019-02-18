using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using kdgparking.DAL;
using kdgparking.BL.Domain;

namespace kdgparking.BL
{
    public class Manager : IManager
    {
        private readonly IRepository repo;

        public Manager()
        {
            // testdata in Seed()
            repo = new kdgparking.DAL.Repository();
        }

        public Person AddPersoon(Person person)
        {
            throw new NotImplementedException();
        }

        public Person GetPersonen()
        {
            throw new NotImplementedException();
        }
    }
}
