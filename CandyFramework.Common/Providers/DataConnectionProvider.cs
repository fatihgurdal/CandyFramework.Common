using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandyFramework.Common.Providers
{
    public class DataConnectionProvider
    {
        public static string GetConnectionString()
        {
            if (true)
            {

            }
            var regeditPath = "Software\\Wow6432Node\\MySQL AB\\MySQL Connector\\Net";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(regeditPath))
            {
                if (key != null)
                {
                    Object o = key.GetValue("Version");
                    if (o != null)
                    {
                        Version version = new Version(o as String);  //"as" because it's REG_SZ...otherwise ToString() might be safe(r)
                                                                     //do what you like with version
                    }
                }
            }

        }
    }
}
