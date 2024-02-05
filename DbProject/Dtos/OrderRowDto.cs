namespace DbProject.Dtos;

public class OrderRowDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
}
