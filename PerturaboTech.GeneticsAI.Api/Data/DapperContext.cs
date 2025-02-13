using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;
using Npgsql;

namespace PerturaboTech.GeneticsAI.Api.Data
{
    public class DapperContext(IOptions<DatabaseOptions> databaseOptions)
    {
        private readonly DatabaseOptions _databaseOptions = databaseOptions.Value;

        public IDbConnection CreateConnection()
            => new NpgsqlConnection(_databaseOptions.ConnectionString);
    }
}
