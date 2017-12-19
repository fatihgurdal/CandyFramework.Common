using CandyFramework.Common.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandyFramework.Common
{
    public class Project
    {
        /// <summary>
        /// Projenizin adı
        /// </summary>
        public static string ProjectName { get; private set; } = null;
        /// <summary>
        /// Eğer bir ürünü birden fazla instance/örnek ile kurulumu için
        /// </summary>
        public static string InstanceName { get; private set; } = null;
        /// <summary>
        /// Veri tabanı bağlantı bilgisini hangi sağlayıcıdan okunsun?
        /// </summary>
        public static DataConnectionEnum Provider { get; private set; } = DataConnectionEnum.None;
        private static bool _setProduct = false;
        public static void SetProduct(string projectName, string instanceName, DataConnectionEnum _provider)
        {
            if (string.IsNullOrEmpty(projectName) || string.IsNullOrEmpty(instanceName) || _provider == DataConnectionEnum.None)
            {
                throw new ArgumentNullException("Project or Version not null");
            }
            if (_setProduct == false)
            {
                throw new ArgumentNullException("The product is already assigned !");
            }
            ProjectName = projectName;
            InstanceName = instanceName;
            Provider = _provider;
            _setProduct = true;
        }
    }
}
