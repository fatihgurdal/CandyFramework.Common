using CandyFramework.Common.Converter;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandyFramework.Common.Providers
{
    public class DataConnectionProvider
    {
        /// <summary>
        /// Project sınıfında tanımlanan Ürün, Örnek adı ve Provider'a göre regeditten veya file system'den connection string okumak için kullanılır.
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString()
        {
            if (Project.Provider == DataConnectionEnum.Regedit)
            {
                #region - Regedit -
                var regeditPath = $"SOFTWARE\\CandyFramework\\{Project.ProjectName}\\{Project.InstanceName}";

                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(regeditPath))
                {
                    if (key != null)
                    {
                        Object o = key.GetValue("ConnectionString");
                        if (o != null)
                        {
                            return o.ToString();
                        }
                        else
                        {
                            throw new KeyNotFoundException("CF Error: Not found ConnectionString Regedit KeyName");

                        }
                    }
                    else
                    {
                        throw new KeyNotFoundException("CF Error: Not found Connection String");
                    }
                } 
                #endregion

            }
            else if (Project.Provider == DataConnectionEnum.ReadFile)
            {
                #region - File System -
                var fileName = $"{Project.ProjectName}_{Project.InstanceName}.txt";

                var fileText = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, fileName));

                var connectionObject = JsonSerializer.JSONDeserialize<dynamic>(fileText);

                return connectionObject.ConnectionString; 
                #endregion
            }
            else
            {
                throw new NotSupportedException("Not supprted Connection Provider. Supported providers Regedit and ReadFile");
            }


        }
    }
}
