using Cafe.Domain.Enums;
namespace Cafe.Application.Models.Orders;

public sealed record AddOnChoice(AddOn Name, string? Option = null);

