using Postgrest.Models;
using Postgrest.Attributes;

using System;
namespace newsLetterAPI.Models;

[Table("newsletter")]

public class Newsletter : BaseModel
{
    [PrimaryKey("id", false)]

    public long Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("read_time")]
    public int ReadTime { get; set; }

    [Column("created_at")]
    public DateTime CreateedAt { get; set; }
}


