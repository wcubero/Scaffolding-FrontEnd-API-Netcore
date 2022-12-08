using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceltec.Presupuesto.Models.Minion
{
    public partial class Minion
    {

        public IEnumerable<Minion> GetMinions(Minion model,AppSettings appSetting)
        {
            //puede ser query bk o sp, según rq
            string query = $@"
            SELECT * FROM Minion 
            WHERE Nombre = '{model.Nombre}'
            ";

            return Extensions.Execute_Query<Minion>(query, appSetting.ConnectionString);
        }

    }
}
