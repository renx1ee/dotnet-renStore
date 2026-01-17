using System.ComponentModel.DataAnnotations;

namespace RenStore.WebApi.ViewModels;

public class CategoryViewModel
{
    [Required]
    public Guid Id { get; set; }
    [Display(Name = "Category Name*")]
    [Required(ErrorMessage = "Please enter Category Name!")]
    public string? Name { get; set; }
    [Display(Name = "Category Description*")]
    [Required(ErrorMessage = "Please enter Category Description!")]
    public string? Description { get; set; }
    [Display(Name = "Category Image*")]
    [Required(ErrorMessage = "Please enter Category Image!")]
    public string? ImagePath { get; set; }
}