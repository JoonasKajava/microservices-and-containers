using System.ComponentModel.DataAnnotations;

namespace Reservation;

public class ReservationOptions
{
    public static string SectionName => "Reservation";

    [Required]
    public required string DatabaseUrl { get; set; }


    [Required]
    public required string InventoryServiceUrl { get; set; }

    public string PublisherSocketBind { get; set; } = "tcp://127.0.0.1:5556";
}