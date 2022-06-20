using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace КП5
{
    class HashTable
    {
        private readonly byte _maxSize = 255;
        private Dictionary<int, List<Item>> _items = null;
        public IReadOnlyCollection<KeyValuePair<int, List<Item>>> Items => _items?.ToList()?.AsReadOnly();
        public HashTable()
        {
            _items = new Dictionary<int, List<Item>>(_maxSize);
        }
        public void Insert(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key.Length > _maxSize)
            {
                throw new ArgumentException($"Максимальная длинна ключа составляет {_maxSize} символов.", nameof(key));
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            var item = new Item(key, value);

            var hash = GetHashMD5(item.Key);
            int hashKey = BitConverter.ToInt32(hash, 0);

            List<Item> hashTableItem = null;
            if (_items.ContainsKey(hashKey))
            {
                hashTableItem = _items[hashKey];

                var oldElementWithKey = hashTableItem.SingleOrDefault(i => i.Key == item.Key);
                if (oldElementWithKey != null)
                {
                    throw new ArgumentException($"Хеш-таблица уже содержит элемент с ключом {key}. Ключ должен быть уникален.", nameof(key));
                }

                _items[hashKey].Add(item);
            }
            else
            {
                hashTableItem = new List<Item> { item };

                _items.Add(hashKey, hashTableItem);
            }
        }
        public void Delete(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key.Length > _maxSize)
            {
                throw new ArgumentException($"Максимальная длинна ключа составляет {_maxSize} символов.", nameof(key));
            }

            var hash = GetHashMD5(key);
            int hashKey = BitConverter.ToInt32(hash, 0);

            if (!_items.ContainsKey(hashKey))
            {
                return;
            }
            
            var hashTableItem = _items[hashKey];

            var item = hashTableItem.SingleOrDefault(i => i.Key == key);

            if (item != null)
            {
                hashTableItem.Remove(item);
            }
        }
        public string Search(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key.Length > _maxSize)
            {
                throw new ArgumentException($"Максимальная длинна ключа составляет {_maxSize} символов.", nameof(key));
            }

            var hash = GetHashMD5(key);
            int hashKey = BitConverter.ToInt32(hash, 0);

            if (!_items.ContainsKey(hashKey))
            {
                return null;
            }

            var hashTableItem = _items[hashKey];

            if (hashTableItem != null)
            {
                var item = hashTableItem.SingleOrDefault(i => i.Key == key);

                if (item != null)
                {
                    return item.Value;
                }
            }

            return null;
        }
        private byte[] GetHashMD5(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value.Length > _maxSize)
            {
                throw new ArgumentException($"Максимальная длинна ключа составляет {_maxSize} символов.", nameof(value));
            }

            var tmpSource = ASCIIEncoding.ASCII.GetBytes(value);
            var hash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            return hash;
        }
        private byte[] GetHashSHA256(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value.Length > _maxSize)
            {
                throw new ArgumentException($"Максимальная длинна ключа составляет {_maxSize} символов.", nameof(value));
            }

            var tmpSource = ASCIIEncoding.ASCII.GetBytes(value);
            var hash = new SHA256CryptoServiceProvider().ComputeHash(tmpSource);
            return hash;
        }
    }
}
