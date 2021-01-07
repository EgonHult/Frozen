using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Frozen.UnitTests.Sessions
{
    class TestSession : ISession
    {
        private Dictionary<string, byte[]> _store = new Dictionary<string, byte[]>(StringComparer.OrdinalIgnoreCase);

        public IEnumerable<string> Keys { get { return _store.Keys; } }

        public bool IsAvailable => throw new NotImplementedException();

        public string Id => throw new NotImplementedException();

        public void Clear()
        {
            _store.Clear();
        }

        public Task CommitAsync()
        {
            return Task.FromResult(0);
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task LoadAsync()
        {
            return Task.FromResult(0);
        }

        public Task LoadAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            _store.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            _store[key] = value;
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            return _store.TryGetValue(key, out value);
        }
    }
}
