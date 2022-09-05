using Esoteric.Finance.Abstractions.Settings;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Esoteric.Finance.Data.Tools
{
    public class EsotericFinanceEncryptionProvider : IEncryptionProvider
    {
        private readonly string _key;

        public EsotericFinanceEncryptionProvider(IOptions<AppSettings> settings)
        {
            _key = settings.Value.Data.ColumnEncryptionKey
                ?? throw new ArgumentException("Value.Data.ColumnEncryptionKey is null", nameof(settings));
        }

        public TStore? Encrypt<TStore, TModel>(TModel dataToEncrypt, Func<TModel, byte[]> converter, Func<Stream, TStore> encoder)
        {
            if (converter is null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            if (encoder is null)
            {
                throw new ArgumentNullException(nameof(encoder));
            }

            byte[] inputArray = converter(dataToEncrypt);
            if (inputArray is null || inputArray.Length == 0)
            {
                return default;
            }

            return encoder(Encrypt(inputArray));
        }

        public TModel? Decrypt<TStore, TModel>(TStore dataToDecrypt, Func<TStore, byte[]> decoder, Func<Stream, TModel> converter)
        {
            if (decoder is null)
            {
                throw new ArgumentNullException(nameof(decoder));
            }

            if (converter is null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            byte[] inputArray = decoder(dataToDecrypt);
            if (inputArray is null || inputArray.Length == 0)
            {
                return default;
            }


            return converter(Decrypt(inputArray));
        }

        public Stream Encrypt(byte[] input)
        {
            using var cryptoServiceProvider = TripleDESCng.Create();

            var cryptoTransform = cryptoServiceProvider.CreateEncryptor();
            var target = new MemoryStream();
            var stream = new CryptoStream(target, cryptoTransform, CryptoStreamMode.Write);
            stream.Write(input, 0, input.Length);
            stream.FlushFinalBlock();

            return new MemoryStream(target.ToArray());
        }

        public Stream Decrypt(byte[] input)
        {
            using var cryptoServiceProvider = TripleDESCng.Create();

            var cryptoTransform = cryptoServiceProvider.CreateDecryptor();
            var memoryStream = new MemoryStream(input);
            var stream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read);

            var ms = new MemoryStream();
            stream.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }
    }
}
