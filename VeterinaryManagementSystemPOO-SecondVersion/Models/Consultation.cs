public class Consultation
{
    public int Id { get; set; }
    public int PetId { get; set; }
    public DateTime Date { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty;
}