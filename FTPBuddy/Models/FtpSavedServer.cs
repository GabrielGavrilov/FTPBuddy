using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColumnAttribute = SQLite.ColumnAttribute;
using TableAttribute = SQLite.TableAttribute;

namespace FTPBuddy.Models
{
    [Table("ftp_servers")]
    public class FtpSavedServer
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int ID { get; set; }

        [Column("domain")]
        public string Domain { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

    }
}
