namespace DbProject.Dtos;

public class NewOrderDto
{
    public HashSet<OrderRowDto> OrderRows { get; set; } = new HashSet<OrderRowDto>();
}
