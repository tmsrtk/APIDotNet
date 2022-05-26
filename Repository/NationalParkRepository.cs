using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIDotNet.Data;
using APIDotNet.Entities;
using APIDotNet.Repository.IRepository;

namespace APIDotNet.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly DataContext _db;
        public NationalParkRepository(DataContext db)
        {
            _db = db;
        }
        public bool CreateNationalPark(NationalPark nationalPark)
        {
            _db.Add(nationalPark);
            return Save();
        }

        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            _db.Remove(nationalPark);
            return Save();
        }

        public NationalPark GetNationalPark(int nationalParkId)
        {
            return _db.NationalParks.FirstOrDefault(a => a.Id == nationalParkId);
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            return _db.NationalParks.OrderBy(a => a.Name).ToList();
        }

        public bool NationalParkExists(string name)
        {
            bool value = _db.NationalParks.Any(a => a.Name.ToLower().Trim() == name.Trim());
            return value;
        }

        public bool NationalParkExists(int nationalParkId)
        {
            return _db.NationalParks.Any(a => a.Id == nationalParkId);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            _db.NationalParks.Update(nationalPark);
            return Save();
        }
    }
}