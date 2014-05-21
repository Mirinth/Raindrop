using System.Web.Mvc;
using System.IO;

public class RaindropView : IView
{
   public RaindropView(string viewPath, string masterPath)
   {
       this.ViewPath = viewPath;
   }

   public string ViewPath { get; private set; }

   public void Render(ViewContext viewContext, TextWriter writer)
   {
       string filePath = viewContext.HttpContext.Server.MapPath(this.ViewPath);

       Raindrop.Raindrop template = new Raindrop.Raindrop(filePath);

       template.Apply(viewContext.ViewData, writer);
   }
}