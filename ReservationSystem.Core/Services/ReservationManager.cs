using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReservationSystem.Core.Exceptions;
using ReservationSystem.Core.Models;

namespace ReservationSystem.Core.Services
{
    /// <summary>
    /// Valdo rezervacijų kolekciją: pridėjimą, šalinimą, paiešką, filtravimą, failų operacijas.
    /// Taip pat leidžia iteruoti per rezervacijas naudojant foreach (IEnumerable)
    /// ir iškelia įvykius, kai rezervacijos keičiasi.
    /// </summary>
    public class ReservationManager : IEnumerable<Reservation>
    {
        private readonly List<Reservation> _reservations = new();
        private readonly Dictionary<int, Reservation> _byId = new();

        // -------------------------
        // Įvykiai
        // -------------------------
        public event EventHandler<Reservation>? ReservationAdded;
        public event EventHandler<Reservation>? ReservationDeleted;
        public event EventHandler<Reservation>? ReservationUpdated;

        // -------------------------
        // CRUD metodai
        // -------------------------

        /// <summary>
        /// Prideda vieną ar kelias rezervacijas.
        /// Iškelia įvykį ReservationAdded.
        /// </summary>
        public void Add(params Reservation[] items)
        {
            foreach (var r in items)
            {
                if (_byId.ContainsKey(r.Id))
                {
                    _reservations.Remove(_byId[r.Id]);
                    _byId[r.Id] = r;
                    ReservationUpdated?.Invoke(this, r); // atnaujinta rezervacija
                }
                else
                {
                    _reservations.Add(r);
                    _byId[r.Id] = r;
                    ReservationAdded?.Invoke(this, r); // nauja rezervacija
                }
            }
        }

        public IEnumerable<Reservation> GetAll() => _reservations;

        public IEnumerable<Reservation> Filter(Func<Reservation, bool> predicate)
            => _reservations.Where(predicate);

        public IEnumerable<Reservation> GetByDateRange(DateTime start, DateTime end)
        {
            foreach (var r in _reservations)
                if (r.Date >= start && r.Date <= end)
                    yield return r;
        }

        public Reservation FindOrThrow(int id)
        {
            if (_byId.TryGetValue(id, out var r))
                return r;

            throw new ReservationNotFoundException(id);
        }

        public bool Delete(int id)
        {
            if (_byId.TryGetValue(id, out var r))
            {
                _byId.Remove(id);
                _reservations.Remove(r);
                ReservationDeleted?.Invoke(this, r); // iškeliame įvykį
                return true;
            }
            return false;
        }

        /// <summary>
        /// Atnaujina rezervaciją pagal ID.
        /// Iškelia įvykį ReservationUpdated.
        /// </summary>
        public bool Update(Reservation updated)
        {
            if (_byId.ContainsKey(updated.Id))
            {
                _reservations.Remove(_byId[updated.Id]);
                _reservations.Add(updated);
                _byId[updated.Id] = updated;
                ReservationUpdated?.Invoke(this, updated);
                return true;
            }
            return false;
        }

        public void SaveToFile(string path)
        {
            var lines = _reservations.Select(r =>
                $"{r.Id};{r.Date:yyyy-MM-dd};{r.Duration};{r.Title};{r.Status}");
            File.WriteAllLines(path, lines);
        }

        public void LoadFromFile(string path)
        {
            if (!File.Exists(path))
                return;

            _reservations.Clear();
            _byId.Clear();

            var lines = File.ReadAllLines(path);

            foreach (var line in lines)
            {
                if (ReservationParser.TryParse(line, out var r) && r != null)
                    Add(r);
            }
        }

        public IEnumerable<Reservation> FilterByStatus(ReservationStatus status)
        {
            foreach (var r in _reservations)
                if (r.Status.HasFlag(status))
                    yield return r;
        }

        // -------------------------
        // IEnumerable implementacija
        // -------------------------
        public IEnumerator<Reservation> GetEnumerator() => new ReservationEnumerator(_reservations);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class ReservationEnumerator : IEnumerator<Reservation>
        {
            private readonly List<Reservation> _list;
            private int _index = -1;

            public ReservationEnumerator(List<Reservation> list) => _list = list;
            public Reservation Current => _list[_index];
            object IEnumerator.Current => Current;
            public bool MoveNext() { _index++; return _index < _list.Count; }
            public void Reset() => _index = -1;
            public void Dispose() { }
        }
    }
}
