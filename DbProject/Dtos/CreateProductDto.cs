namespace DbProject.Dtos;

public class CreateProductDto
{
    public int Id { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal ProductPrice { get; set; }
    public string ManufacureName { get; set; } = null!;
    public string CategoryName { get; set; } = null!;
    public string Ingress {  get; set; } = null!;
    public string? DescriptionText { get; set; }


    public int DescriptionId { get; set; }
    public int CategoryId { get; set; }
    public int ManufactureId { get; set; }

}
