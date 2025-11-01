using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AtharPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpsController : ControllerBase
    {
        private readonly Context _context;

        public OpsController(Context context)
        {
            _context = context;
        }

        [HttpGet("campaign-columns")]
        public async Task<IActionResult> GetCampaignColumns()
        {
            try
            {
                await using var conn = _context.Database.GetDbConnection();
                if (conn.State != System.Data.ConnectionState.Open)
                    await conn.OpenAsync();

                await using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT name FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Campaigns') ORDER BY name";
                var cols = new List<string>();
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    cols.Add(reader.GetString(0));
                }
                return Ok(new { database = conn.Database, dataSource = conn.DataSource, columns = cols });
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.ToString());
            }
        }

        [HttpGet("migrations")]
        public async Task<IActionResult> GetMigrations()
        {
            try
            {
                var applied = await _context.Database.GetAppliedMigrationsAsync();
                var pending = await _context.Database.GetPendingMigrationsAsync();
                return Ok(new { applied, pending });
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.ToString());
            }
        }

        [HttpGet("test-select")]
        public async Task<IActionResult> TestSelect()
        {
            try
            {
                await using var conn = _context.Database.GetDbConnection();
                if (conn.State != System.Data.ConnectionState.Open)
                    await conn.OpenAsync();

                await using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT TOP (1) [ExternalId], [SupportingCharitiesJson] FROM [dbo].[Campaigns] ORDER BY [Id]";
                await using var reader = await cmd.ExecuteReaderAsync();
                string? externalId = null;
                string? supporters = null;
                if (await reader.ReadAsync())
                {
                    externalId = reader.IsDBNull(0) ? null : reader.GetString(0);
                    supporters = reader.IsDBNull(1) ? null : reader.GetString(1);
                }
                return Ok(new { ok = true, externalId, supporters });
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.ToString());
            }
        }
    }
}
