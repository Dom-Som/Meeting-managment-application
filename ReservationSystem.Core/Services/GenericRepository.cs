using System;
using System.Collections.Generic;
using System.Linq;
using ReservationSystem.Core.Models;

namespace ReservationSystem.Core.Services
{
    /// <summary>
    /// Bendrasis (generic) saugyklos tipas rezervacijoms.
    /// </summary>
    public class GenericRepository<T> where T : BaseReservation
    {
        private readonly List<T> _items = new();

        public void Add(T item) => _items.Add(item);

        public IEnumerable<T> GetAll() => _items;

        public IEnumerable<T> Filter(Func<T, bool> predicate) => _items.Where(predicate);
    }
}
